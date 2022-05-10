using Fusion;
using Player;
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

        private ThirdPersonController _controller;
        private float _respawnInSeconds = -1;

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

            _controller = GetComponentInChildren<ThirdPersonController>();

            PlayerManager.AddPlayer(this);

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

            // DontDestroyOnLoad(gameObject);

            Debug.Log("Spawned [" + this + "] IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer +
                      " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
            {
                if (_respawnInSeconds >= 0)
                    CheckRespawn();
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            PlayerManager.RemovePlayer(this);
        }

        public void CheckRespawn()
        {
            if (_respawnInSeconds > 0)
                _respawnInSeconds -= Runner.DeltaTime;
            
            if (_respawnInSeconds <= 0)
            {
                Debug.Log("Player respawned");
                _controller.GetComponent<NetworkTransform>().transform.position = new Vector3(0.0f, 100.0f, 0.0f);
                _respawnInSeconds = -1;
            }
        }

        public void Respawn(float inSeconds)
        {
            _respawnInSeconds = inSeconds;
        }

        public void TriggerDespawn()
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