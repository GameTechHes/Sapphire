using Cinemachine;
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

        [Networked] private int _health { get; set; }

        public override void Spawned()
        {
            _health = MAX_HEALTH;
            healthBar.SetMaxHealth(MAX_HEALTH);
            healthBar.SetProgress(_health);
            if (!Object.HasInputAuthority)
            {
                var cams = GetComponentsInChildren<Camera>();
                foreach (var cam in cams)
                {
                    cam.gameObject.SetActive(false);
                }

                var virtCam = GetComponentInChildren<CinemachineVirtualCamera>();
                if (virtCam != null)
                {
                    virtCam.gameObject.SetActive(false);
                }
                var canvas = GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
            else
            {
                var playerInput = GetComponentInChildren<PlayerInput>();
                if (playerInput != null)
                {
                    playerInput.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}