using UnityEngine;

public class PowerUpSapphire : PowerUpBase
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<SapphireController>().AddSapphire();
            Destroy(gameObject);
        }
    }
}