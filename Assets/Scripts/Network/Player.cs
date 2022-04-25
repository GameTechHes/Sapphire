using Fusion;
using UnityEngine;

namespace Network
{
    public class Player : NetworkBehaviour
    {
        public GameObject[] objectsToDisable;
        private void Start()
        {
            if (Object.HasInputAuthority)
            {
                RPC_DisableComponents();
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_DisableComponents(RpcInfo info = default)
        {
            if (!info.Source.IsNone && info.Source != Runner.Simulation.LocalPlayer)
            {
                foreach (var obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}