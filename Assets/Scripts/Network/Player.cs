using UnityEngine;
using UserInterface;

namespace Network
{
    public class Player : MonoBehaviour
    {
        public const byte MAX_HEALTH = 100;
        [SerializeField] private HealthBar healthBar;

        private int _health;
        
        void Start()
        {
            _health = MAX_HEALTH;
            healthBar.SetMaxHealth(MAX_HEALTH);
            healthBar.SetProgress(_health);
        }
    }
}

