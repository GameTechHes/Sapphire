using Items;
using UnityEngine;

namespace Sapphire
{
    public class Wizard : Player
    {
        private void OnTriggerEnter(Collider other)
        {
            var arrow = other.gameObject.GetComponent<Arrow>();
            if (arrow != null && Object.HasInputAuthority)
            {
                var ran = Random.Range(1, 4);
                FindObjectOfType<AudioManager>().Play("Hurt_" + ran);
                RPC_AddHealth(-Arrow.DAMAGE);

                // Delete in local to prevent multiple trigger
                Destroy(other);
            }
        }
    }
}