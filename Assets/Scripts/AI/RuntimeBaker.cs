using Unity.AI.Navigation;
using UnityEngine;


/* IMPORTANT */
/*
 *   This Script is executed before every other script
 *   Take this is consideration for every modifications
*/
namespace AI
{
    public class RuntimeBaker : MonoBehaviour
    {
        private NavMeshSurface surface;

        void Start()
        {
            surface = GetComponent<NavMeshSurface>();
        }

        public void BakeAll()
        {
            surface.BuildNavMesh();
        }
    }
}