using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    public TextMeshProUGUI healthCount;

    private int _maxHealth;

    public void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public void SetProgress(int health)
    {
        if (health >= 0)
        {
            _slider.value = health / (float)_maxHealth;
            healthCount.text = health.ToString();
        }
    }
}