using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Knight : MonoBehaviour
    {
        public float aimSpeed = 4.0f;
        public CinemachineVirtualCamera followCamera;
        
        private StarterAssetsInputs _inputs;
        private Animator _controller;
        private float _cameraDistance = 4.0f;
        private Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;

        void Start()
        {
            _controller = GetComponent<Animator>();
            _inputs = GetComponent<StarterAssetsInputs>();
            _cinemachine3RdPersonFollow = followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void Update()
        {
            if (_inputs.aim)
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Time.deltaTime * aimSpeed);
                _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 1.0f, Time.deltaTime * aimSpeed);
            }
            else
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Time.deltaTime * aimSpeed);
                _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 0.5f, Time.deltaTime * aimSpeed);
            }
        }

        void OnAim(InputValue value)
        {
            _controller.SetBool("Aim", value.isPressed);
        }
    }
}