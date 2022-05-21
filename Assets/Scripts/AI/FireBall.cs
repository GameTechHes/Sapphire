using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        var obj = Instantiate(explosionEffet, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("Fireball");
        Destroy(obj, 1);

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DespawnArrow()
    {
        Runner.Despawn(Object);
    }
}