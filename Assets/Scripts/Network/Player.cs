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

        public int playerID { get; private set; }

        public void InitNetworkState()
        {
            _health = MAX_HEALTH;
        }

        public override void Spawned()
        {
            healthBar.SetMaxHealth(MAX_HEALTH);
            healthBar.SetProgress(_health);
            playerID = Object.InputAuthority;
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
                // Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            PlayerManager.RemovePlayer(this);
        }


        public async void TriggerDespawn()
        {
            PlayerManager.RemovePlayer(this);

            if (Object == null)
            {
                return;
            }

            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}