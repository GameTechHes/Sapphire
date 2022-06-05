using Fusion;
using Sapphire;
using UnityEngine;

namespace Items
{
    public abstract class PowerUpBase : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<NetworkObject>().HasInputAuthority)
                {
                    var player = other.GetComponent<Player>();
                    if (player)
                    {
                        Player.Local._uiManager.triggerFlash();
                        Player.Local._uiManager.RPC_ShowKnightPosition();
                        ApplyEffects(player);
                    }
                }
            }
        }

        protected abstract void ApplyEffects(Player player);

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        protected void RPC_Despawn()
        {
            if (Object != null && Object.IsValid)
                Runner.Despawn(Object);
        }
    }
}