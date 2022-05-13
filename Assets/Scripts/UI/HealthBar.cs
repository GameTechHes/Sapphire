using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class HealthBar : MonoBehaviour
    {
        public TextMeshProUGUI healthCount;

        private Slider _slider;
        private int _maxHealth;

        public void Awake()
        {
            _slider = GetComponent<Slider>();
            FindObjectOfType<AudioCharacter>().PlayHurtSFX();
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetProgress(int health)
        {
            if (health >= 0 && _slider != null)
            {
                _slider.value = health / (float)_maxHealth;
                healthCount.text = health.ToString();
            }
        }

        
    }
}