using Fusion;
using Sapphire;
using UnityEngine;

public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;
    public const int DAMAGE = 10;

    private void OnTriggerEnter(Collider other)
    {

        // Prevent self collision with the wizard
        if (other.GetComponent<Wizard>() != null || other.GetComponent<Bot>() != null)
            return;


        if (other.GetComponent<Player>() && other.GetComponent<NetworkObject>().HasInputAuthority)
        {
            Player.Local._uiManager.triggerFlash();
        }
        var obj = Instantiate(explosionEffet, transform.position, transform.rotation);
        Destroy(obj, 1);
        FindObjectOfType<AudioManager>().Play("Fireball");

        if (Object != null && Object.IsValid)
            Runner.Despawn(Object);
    }
}