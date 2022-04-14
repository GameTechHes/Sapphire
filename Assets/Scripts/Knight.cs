using UnityEngine;
using UnityEngine.InputSystem;

public class Knight : MonoBehaviour
{
    private int _health;
    public int maxHealth = 100;

    public HealthBar healthBar;
    private Animator _controller;

    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetProgress(_health);
        _controller = GetComponent<Animator>();
    }

    void OnAim(InputValue value)
    {
        _controller.SetBool("Aim", value.isPressed);
    }
}