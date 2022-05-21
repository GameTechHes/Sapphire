using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Sapphire;
public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") ){
            if(other.gameObject.GetComponentInParent<Player>().PlayerType == PlayerType.WIZARD){
                return;
            }
        }
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