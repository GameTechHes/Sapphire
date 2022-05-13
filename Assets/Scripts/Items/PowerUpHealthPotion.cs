using Sapphire;
using UnityEngine;

public class PowerUpHealthPotion : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            int playerhealth = collider.gameObject.GetComponent<Knight>().GetPlayerHealth();

            if (playerhealth != 100)
            {
                collider.gameObject.GetComponent<Knight>().SetPlayerHealth(playerhealth + 25);
                Destroy(gameObject);
            }
        }
    }

}
