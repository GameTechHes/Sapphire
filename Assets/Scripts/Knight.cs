using UnityEngine;
using UnityEngine.InputSystem;

public class Knight : MonoBehaviour
{
    private int _health;
    public int maxHealth = 100;

    public HealthBar healthBar;
    private float counter;
    private Animator _controller;

    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetProgress(_health);
        _controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1)
        {
            counter = 0;
            _health -= 1;
            healthBar.SetProgress(_health);
        }
    }

    void OnAim(InputValue value)
    {
        _controller.SetBool("Aim", value.isPressed);
    }
}