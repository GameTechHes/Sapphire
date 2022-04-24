using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using StarterAssets;

public class MapControllerScript : MonoBehaviour
{
    private bool displayMap = false;
    private bool canToggle = true;
    private float toggleTime = 0.2f;
    private float minSize = 15;
    private float maxSize = 200;

    GameObject minicamUI;
    public Camera minicam;

    public StarterAssetsInputs starterAssetsInputs;
    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        minicamUI = GameObject.Find("MiniCameraUI");
        minicamUI.SetActive(displayMap);
        //controller = PlayerArmature.GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if((starterAssetsInputs.resumeGame || starterAssetsInputs.displayMap) && canToggle){
            starterAssetsInputs.displayMap = false;
            StartCoroutine(toggleMiniMap());
        }
    }

    private void ResumeGame(){
			if(starterAssetsInputs.resumeGame){
                starterAssetsInputs.resumeGame = false;
				playerInput.actions.FindActionMap("Player").Enable();
            	playerInput.actions.FindActionMap("UI").Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
			}
		}
    
    public void foo(){
        Debug.Log("Click");    
    }

    public void OnScrollWheel(){
        Vector2  vec = Mouse.current.scroll.ReadValue();
        float scrollingUnit = 20;
        float scroll = vec.y;
        if(scroll > 0 && minicam.orthographicSize - scrollingUnit > minSize){
            scrollingUnit = -20;
        }
        else if(scroll < 0 && minicam.orthographicSize + scrollingUnit < maxSize){
            scrollingUnit = 20;
        }
        else{
            scrollingUnit = 0;
        }
        minicam.orthographicSize += scrollingUnit;
    } 

    IEnumerator toggleMiniMap()
    {
        displayMap = !displayMap;
        //Debug.Log(displayMap);
        minicamUI.SetActive(displayMap);
        //PlayerArmature.GetComponent<ThirdPersonController>().enabled = !displayMap;
        if(displayMap){
            starterAssetsInputs.StopPlayerMovement();
            playerInput.actions.FindActionMap("Player").Disable();
            playerInput.actions.FindActionMap("UI").Enable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else{
            ResumeGame();
        }
        canToggle = false;
        yield return new WaitForSeconds(toggleTime);
        canToggle = true;
    }
}
