using UnityEngine;

namespace Items
{
    public class Arrow : MonoBehaviour
    {
        public const float v0 = 25;

        const float G = 9.81f;
        private Vector3 velocity;



        private void updateVelocity(){
            // velocity.y = velocity.magnitude * Mathf.Sin(horizontal_angle) - G * Time.deltaTime;
            velocity.y = velocity.y + G * Time.deltaTime;
        }
        private void updateRotation(){
            Quaternion fallingEffect = Quaternion.LookRotation(velocity, transform.up);
            Quaternion rotatingEffect = Quaternion.AngleAxis(20, Vector3.forward);
            transform.rotation = fallingEffect * rotatingEffect ;// * Quaternion.LookRotation(new Vector3(0,0,30), Vector3.right);
            // transform.rotation = Quaternion.LookRotation(new Vector3(0,30,0), Vector3.right);
        }
        private void updatePosition(){
            transform.position -= velocity * Time.deltaTime;
        }
        void Start()
        {
            velocity = transform.forward * v0;
        }
        private void Update(){
            updateVelocity();
            updateRotation();
            updatePosition();
        }
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
