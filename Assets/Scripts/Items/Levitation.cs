using System.Collections;
using UnityEngine;

namespace Items
{
    public class Levitation : MonoBehaviour
    {
        public bool shouldWait;
        public float degreesPerSecond;
        public float frequency = 1f;
        public float amplitude = 0.5f;

        // Position Storage Variables
        private Vector3 posOffset;
        private Vector3 tempPos;
        private float timeEllapsed;
        private bool levitate;

        void Start()
        {
            // Store the starting position & rotation of the object
            posOffset = transform.position;
            if (shouldWait)
                StartCoroutine(SetLevitation());
            else
                levitate = true;
        }

        void Update()
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            if (levitate)
            {
                tempPos = posOffset;
                tempPos.y += Mathf.Sin(timeEllapsed * Mathf.PI * frequency) * amplitude;

                transform.position = tempPos;
                timeEllapsed += Time.deltaTime;
            }
            // Float up/down with a Sin()
        }

        IEnumerator SetLevitation()
        {
            float waitTime = Random.Range(1f, 2f);
            yield return new WaitForSeconds(waitTime);
            levitate = true;
            timeEllapsed = 0;
        }
    }
}