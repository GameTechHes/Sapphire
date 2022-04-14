using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using StarterAssets;
using UnityEngine.UI;

public class MapControllerScript : MonoBehaviour
{
    private bool displayMap = false;
    private bool canToggle = true;
    private float toggleTime = 0.2f;

    GameObject minicamUI;
    Canvas canvas;
    public Camera minicam;

    GameObject PlayerArmature;
    private StarterAssetsInputs _input;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        minicamUI = GameObject.Find("MiniCameraUI");
        minicamUI.SetActive(displayMap);
        //controller = PlayerArmature.GetComponent<ThirdPersonController>();
        PlayerArmature = GameObject.Find("PlayerArmature");
        _input = PlayerArmature.GetComponent<StarterAssetsInputs>();
        _playerInput = PlayerArmature.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if((_input.resumeGame || _input.displayMap) && canToggle){
            _input.displayMap = false;
            Debug.Log(displayMap);
            StartCoroutine(toggleMiniMap());
        }
    }

    private void ResumeGame(){
			if(_input.resumeGame){
                _input.resumeGame = false;
				_playerInput.actions.FindActionMap("Player").Enable();
            	_playerInput.actions.FindActionMap("UI").Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
			}
		}


    IEnumerator toggleMiniMap()
    {
        displayMap = !displayMap;
        //Debug.Log(displayMap);
        minicamUI.SetActive(displayMap);
        //PlayerArmature.GetComponent<ThirdPersonController>().enabled = !displayMap;
        if(displayMap){
            _input.StopPlayerMovement();
            _playerInput.actions.FindActionMap("Player").Disable();
            _playerInput.actions.FindActionMap("UI").Enable();
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
