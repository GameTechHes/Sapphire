using Cinemachine;
using Fusion;
using UnityEngine;
using UserInterface;

namespace Sapphire
{
    public class Knight : NetworkBehaviour
    {
        public float aimSpeed = 4.0f;
        public CinemachineVirtualCamera followCamera;

        private Animator _controller;
        private float _cameraDistance = 4.0f;
        private Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;

        public const byte MaxHealth = 100;

        private HealthBar _healthBar;
        [Networked] private int Health { get; set; }

        [Networked] private NetworkBool isAiming { get; set; }

        public override void Spawned()
        {
            _controller = GetComponent<Animator>();
            if (Object.HasInputAuthority)
            {
                _healthBar = FindObjectOfType<HealthBar>();
                _healthBar.SetMaxHealth(MaxHealth);
                _healthBar.SetProgress(Health);
            }

            Health = MaxHealth;

            _cinemachine3RdPersonFollow = followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            isAiming = false;
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                isAiming = input.aim;
                _controller.SetBool("Aim", isAiming);
                if (Object.HasInputAuthority)
                {
                    if (input.aim)
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Runner.DeltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            1.0f,
                            Runner.DeltaTime * aimSpeed);
                    }
                    else
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Runner.DeltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            0.5f,
                            Runner.DeltaTime * aimSpeed);
                    }
                }
            }
            else
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Runner.DeltaTime * aimSpeed);
                _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 0.5f,
                    Runner.DeltaTime * aimSpeed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Spell"))
            {
                var ran = Random.Range(1, 4);
                FindObjectOfType<AudioManager>().Play("Hurt_" + ran);
                SetPlayerHealth(Health - 25);
            }
        }

        public int GetPlayerHealth()
        {
            return Health;
        }

        public void SetPlayerHealth(int newHealth)
        {
            Health = Mathf.Clamp(newHealth, 0, 100);
        }
    }
}