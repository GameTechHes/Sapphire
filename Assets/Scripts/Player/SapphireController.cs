using Fusion;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Sapphire
{
    public class SapphireController : NetworkBehaviour
    {

        private int _totalSapphire;

        [Networked(OnChanged = nameof(OnCountChange))]
        private int _sapphireCounter { get; set; }

        public override void Spawned()
        {
            _sapphireCounter = 0;
            _totalSapphire = FindObjectsOfType<PowerUpSapphire>().Length;
            if (Object.HasInputAuthority)
            {
                Player.Local._uiManager.SetSapphire(_sapphireCounter, _totalSapphire);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_AddSapphire()
        {
            _sapphireCounter += 1;
        }

        public static void OnCountChange(Changed<SapphireController> changed)
        {
            changed.Behaviour.OnCountChange();
        }

        private void OnCountChange()
        {
            if (Object.HasInputAuthority)
                Player.Local._uiManager.SetSapphire(_sapphireCounter, _totalSapphire);
        }

        public bool CheckEndGame()
        {
            return _sapphireCounter >= _totalSapphire;
        }
    }
}