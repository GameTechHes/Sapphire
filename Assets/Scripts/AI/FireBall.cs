using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Sapphire;
public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().PlayerType == PlayerType.KNIGHT)
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.SetHealth(player.Health - damage);
                int rdm = Random.Range(1, 5);
                string sound = "Hurt_" + rdm.ToString();
                FindObjectOfType<AudioManager>().Play(sound);
            }
            else
            {
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
        Runner.Despawn(gameObject.GetComponent<NetworkObject>());
    }
}