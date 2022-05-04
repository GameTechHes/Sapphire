using Fusion;
using UnityEngine;
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
                    if (obj.TryGetComponent<Camera>(out var camera))
                    {
                        camera.enabled = false;
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}