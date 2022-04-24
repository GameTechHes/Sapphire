using UnityEngine;

namespace Items
{
    public class PowerUpSapphire : PowerUpBase
    {
        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<SapphireController>().AddSapphire();
                Destroy(gameObject);
            }
        }
    }
}