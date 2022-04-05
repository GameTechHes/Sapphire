using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArrowAmmo : PowerUpBase
{
    GameObject LaunchOrigin;
    shoot shootScript;
    // Start is called before the first frame update
    void Start()
    {
        LaunchOrigin = GameObject.Find("LaunchOrigin");
        shootScript = LaunchOrigin.GetComponent<shoot>();
    }

    void OnCollisionEnter(Collision collision)
    {
        shootScript.ajustAmmoCount(10);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
