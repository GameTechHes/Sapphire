using Fusion;
using UnityEngine;
using Sapphire;

namespace Items
{
    public class Arrow : NetworkBehaviour
    {
        private const float InitialSpeed = 35;
        public const int damage = 10;

        const float G = 9.81f;
        [Networked] private Vector3 velocity { get; set; }

        public void InitNetworkState()
        {
            velocity = transform.forward * InitialSpeed;
        }

        private void updateVelocity()
        {
            // velocity.y = velocity.magnitude * Mathf.Sin(horizontal_angle) - G * Time.deltaTime;
            var veloY = velocity.y + G * Runner.DeltaTime;
            velocity = new Vector3(velocity.x, veloY, velocity.z);
        }

        private void updateRotation()
        {
            Quaternion fallingEffect = Quaternion.LookRotation(velocity, transform.up);
            Quaternion rotatingEffect = Quaternion.AngleAxis(20, Vector3.forward);
            transform.rotation =
                fallingEffect * rotatingEffect; // * Quaternion.LookRotation(new Vector3(0,0,30), Vector3.right);
            // transform.rotation = Quaternion.LookRotation(new Vector3(0,30,0), Vector3.right);
        }

        private void updatePosition()
        {
            transform.position -= velocity * Runner.DeltaTime;
        }

        public override void Spawned()
        {
            Debug.Log("Arrow spawned");
            velocity = transform.forward * InitialSpeed;
        }

        public override void FixedUpdateNetwork()
        {
            updateVelocity();
            updateRotation();
            updatePosition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Object != null && Object.IsValid)
                Runner.Despawn(Object);
        }
    }
}