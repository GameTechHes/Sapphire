using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    public class Room : MonoBehaviour
    {
        public GameObject doorModel;

        [Header("Doors transforms")] public List<Transform> doorsTransforms;

        [Header("Doors activation")] public List<bool> isDoorActive;

        void Start()
        {
            for (var i = 0; i < isDoorActive.Count; i++)
            {
                if (!isDoorActive[i] && doorsTransforms[i] != null)
                {
                    var door = Instantiate(doorModel, doorsTransforms[i]);
                    door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
                }
            }
        }

        public List<Transform> GetActiveDoors()
        {
            var activeDoors = new List<Transform>();
            for (int i = 0; i < isDoorActive.Count; i++)
            {
                if (isDoorActive[i]) activeDoors.Add(doorsTransforms[i]);
            }

            return activeDoors;
        }

        /***
         * result[0] => +x direction
         * result[1] => +y direction
         * result[2] => -x direction
         * result[3] => -y direction
         */
        public List<Transform> GetDoorsInOrder()
        {
            var rotationAmount = Mathf.RoundToInt(Quaternion.Angle(Quaternion.identity, transform.rotation) / 90) % 4;
            var result = new List<Transform>();
            for (var i = 0; i < doorsTransforms.Count; i++)
            {
                result.Add(doorsTransforms[(i + rotationAmount) % doorsTransforms.Count]);
            }

            return result;
        }
    }
}