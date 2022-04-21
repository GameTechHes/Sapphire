using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


/* IMPORTANT */
/*
 *   This Script is executed before every other script
 *   Take this is consideration for every modifications
*/
public class BakeBaby : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }


    public void BakeAll(){
        Debug.Log("BUILDING");
        surface.BuildNavMesh();
    }
}
