using UnityEngine;
using Player;

public class PowerUpHealthPotion : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            int playerhealth = collider.gameObject.GetComponent<Knight>().getPlayerHealth();

            if (playerhealth != 100)
            {
                collider.gameObject.GetComponent<Knight>().setPlayerHealth(playerhealth + 25);
                Destroy(gameObject);
            }
        }
    }

}
