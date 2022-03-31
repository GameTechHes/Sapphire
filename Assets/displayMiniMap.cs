using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class displayMiniMap : MonoBehaviour
{
    private bool displayMap = false;
    private bool canToggle = true;
    private float toggleTime = 0.2f;
    GameObject minicam;
    GameObject PlayerArmature;
    private StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start()
    {
        minicam = GameObject.Find("MiniCameraUI");
        minicam.SetActive(displayMap);
        PlayerArmature = GameObject.Find("PlayerArmature");
        //controller = PlayerArmature.GetComponent<ThirdPersonController>();
        PlayerArmature = GameObject.Find("PlayerArmature");
        _input = PlayerArmature.GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.displayMap && canToggle){
            _input.displayMap = false;
            Debug.Log(displayMap);
            StartCoroutine(toggleMiniMap());
        }
    }

    IEnumerator toggleMiniMap()
    {
        displayMap = !displayMap;
        //Debug.Log(displayMap);
        minicam.SetActive(displayMap);
        PlayerArmature.GetComponent<ThirdPersonController>().enabled = !displayMap;
        canToggle = false;
        yield return new WaitForSeconds(toggleTime);
        canToggle = true;
    }
}
