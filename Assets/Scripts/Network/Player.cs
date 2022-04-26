using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UserInterface;

namespace Network
{
    public class Player : NetworkBehaviour
    {
        public const byte MAX_HEALTH = 100;
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GameObject[] objectsToDisable;

        [Networked] private int _health { get; set; }

        public override void Spawned()
        {
            _health = MAX_HEALTH;
            healthBar.SetMaxHealth(MAX_HEALTH);
            healthBar.SetProgress(_health);
            if (!Object.HasInputAuthority)
            {
                foreach (var obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                var playerInput = GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    playerInput.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}