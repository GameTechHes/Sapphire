using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    GameObject PlayerArmature;
    // Start is called before the first frame update
    void Start()
    {
        PlayerArmature = GameObject.Find("PlayerArmature");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
