using Cinemachine;
using Fusion;
using UnityEngine;

namespace Player
{
    public class Knight : NetworkBehaviour
    {
        public float aimSpeed = 4.0f;
        public CinemachineVirtualCamera followCamera;

        private Animator _controller;
        private float _cameraDistance = 4.0f;
        private Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;

        [Networked] private NetworkBool isAiming { get; set; }

        public override void Spawned()
        {
            _controller = GetComponent<Animator>();
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
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Time.deltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            1.0f,
                            Runner.DeltaTime * aimSpeed);
                    }
                    else
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Time.deltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            0.5f,
                            Runner.DeltaTime * aimSpeed);
                    }
                }
            }
        }
    }
}