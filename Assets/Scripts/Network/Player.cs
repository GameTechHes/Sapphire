using Fusion;
using UnityEngine;

namespace Network
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject _followCamera;
        
        private NetworkCharacterControllerPrototype _cc;
        
        public static Player local { get; set; }

        [Networked] private Vector2 moveDirection { get; set; }

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
        }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                local = this;
            }
            else
            {
                _followCamera.SetActive(false);
            }
        }

        public void Move()
        {
            _cc.Move(new Vector3(moveDirection.x, 0, moveDirection.y));
        }

        public void SetDirection(Vector2 direction)
        {
            moveDirection = direction;
        }
    }
}