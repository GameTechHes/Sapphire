using Fusion;
using Sapphire;
using UnityEngine;

public class FireBall : NetworkBehaviour
{
    public GameObject explosionEffet;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().PlayerType == PlayerType.KNIGHT)
            {
                Player player = other.GetComponent<Player>();
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

        var obj = Instantiate(explosionEffet, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("Fireball");
        Destroy(obj, 1);
        if (Object != null && Object.IsValid)
            Runner.Despawn(Object);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        if (Object != null && Object.IsValid)
            Runner.Despawn(Object);
    }
}