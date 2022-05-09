using UnityEngine;

namespace Items
{
    public class Arrow : MonoBehaviour
    {
        const float v0 = 2f;
        const float G = 9.81f;
        private Vector3 velocity;

        Rigidbody rb;

        private Vector3 updatePositionY(float time){
            
            return new Vector3(0,0,0);
        }
        private void tmpUpdatePosition(float time){
            transform.position -= velocity*time; 
        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            velocity = transform.forward * v0;
        }
        private void Update(){
            tmpUpdatePosition(Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
