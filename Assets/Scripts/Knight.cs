using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knight : MonoBehaviour
{
    private int _health;
    public int maxHealth = 100;

    public HealthBar healthBar;
    public CinemachineVirtualCamera followCamera;
    public StarterAssetsInputs inputs;
    private Animator _controller;

    public float AimSpeed = 4.0f;
    private float _cameraDistance = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetProgress(_health);
        _controller = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inputs.aim)
        {
            _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Time.deltaTime * AimSpeed);
            followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = _cameraDistance;
        }
        else
        {
            _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Time.deltaTime * AimSpeed);
            followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = _cameraDistance;
        }
    }

    void OnAim(InputValue value)
    {
        _controller.SetBool("Aim", value.isPressed);
    }
}