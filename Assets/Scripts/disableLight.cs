using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPreCull ()
    {
       var allLights = FindObjectsOfType<Light>();
       foreach (var lig in allLights){
          lig.enabled = false;
       }
    }
     
    void OnPostRender ()
    {
       var allLights = FindObjectsOfType<Light>();
       foreach (var lig in allLights){
          lig.enabled = true;
       }
    }
}
