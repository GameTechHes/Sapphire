using Unity.AI.Navigation;
using UnityEngine;


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
        Debug.Log("Baking nav mesh");
        surface.BuildNavMesh();
    }
}
