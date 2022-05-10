using Cinemachine;
using StarterAssets;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Knight : MonoBehaviour
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

        void Start()
        {
            _health = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetProgress(_health);
            _controller = GetComponent<Animator>();
            _inputs = GetComponent<StarterAssetsInputs>();
            _cinemachine3RdPersonFollow = followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void Update()
        {
            healthBar.SetProgress(_health);

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
            if(value.isPressed){
                FindObjectOfType<AudioManager>().Play("AimingBow");
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Spell"))
                _health -= 25;
        }

        public int getPlayerHealth()
        {
            return this._health;
        }

        public void setPlayerHealth(int newHealth)
        {
            if (newHealth > 100)
                this._health = 100;
            else
                this._health = newHealth;
        }
    }
}