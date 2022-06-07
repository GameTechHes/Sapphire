using Items;
using UnityEngine;

namespace Sapphire
{
    public class Wizard : Player
    {
        public override void Spawned()
        {
            base.Spawned();
            if(Object.HasInputAuthority)
                _uiManager.Crosshair.SetActive(true);
        }

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
        
        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (GetInput(out NetworkInputData input))
            {
                if (Object.HasInputAuthority)
                {
                    _uiManager.Crosshair.SetActive(input.aim);
                }
            }
        }
    }
}