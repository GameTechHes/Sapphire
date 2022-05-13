using Sapphire;
using UnityEngine;

namespace Items
{
    public class PowerUpArrowAmmo : PowerUpBase
    {
        public int ammoReward = 10;

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<Shoot>().AddAmmo(ammoReward);
                FindObjectOfType<AudioManager>().Play("CollectingAmmo");
                Destroy(gameObject);
            }
        }
    }
}