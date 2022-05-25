using Fusion;
using Sapphire;
using UnityEngine;

public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;
    public const int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent self collision with the wizard
        if (other.GetComponent<Wizard>() != null)
            return;

        var obj = Instantiate(explosionEffet, transform.position, transform.rotation);
        Destroy(obj, 1);
        FindObjectOfType<AudioManager>().Play("Fireball");

        if (Object.HasStateAuthority)
            RPC_Despawn();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        if (Object != null && Object.IsValid)
            Runner.Despawn(Object);
    }
}