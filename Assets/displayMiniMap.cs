using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class displayMiniMap : MonoBehaviour
{
    private bool displayMap = true;
    private bool canToggle = true;
    private float toggleTime = 0.2f;
    GameObject minicam;
    // Start is called before the first frame update
    void Start()
    {
        minicam = GameObject.Find("MiniCameraUI");
    }

    // Update is called once per frame
    void Update()
    {
        if( Keyboard.current.mKey.wasPressedThisFrame && canToggle){
        StartCoroutine(toggleMiniMap());
        }
    }

    IEnumerator toggleMiniMap()
    {
        displayMap = !displayMap;
        //Debug.Log(displayMap);
        minicam.SetActive(displayMap);
        canToggle = false;
        yield return new WaitForSeconds(toggleTime);
        canToggle = true;
    }
}
