using System;
using Cinemachine;
using Fusion;
using Sapphire.UI;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Knight : NetworkBehaviour
    {
        public int maxHealth = 100;
        public float aimSpeed = 4.0f;
        public HealthBar healthBar;
        public CinemachineVirtualCamera followCamera;
        private StarterAssetsInputs _inputs;

        private int _health;
        private Animator _controller;
        private float _cameraDistance = 4.0f;
        private Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;
        private NetworkCharacterControllerPrototype _cc;

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
        }

        void Start()
        {
            _health = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetProgress(_health);
            _controller = GetComponent<Animator>();
            _inputs = GameObject.Find("InputManager").GetComponent<StarterAssetsInputs>();
            _cinemachine3RdPersonFollow = followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void Update()
        {
            if (_inputs.aim)
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Time.deltaTime * aimSpeed);
                _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 1.0f,
                    Time.deltaTime * aimSpeed);
            }
            else
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Time.deltaTime * aimSpeed);
                _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 0.5f,
                    Time.deltaTime * aimSpeed);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                _cc.Move(0.05f * data.direction * Runner.DeltaTime);
            }
        }

        void OnAim(InputValue value)
        {
            _controller.SetBool("Aim", value.isPressed);
        }
    }
}