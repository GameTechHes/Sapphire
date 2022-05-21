using Fusion;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Sapphire
{
    public class SapphireController : NetworkBehaviour
    {
        private Text _sapphireText;

        private int _totalSapphire;
        
        [Networked(OnChanged = nameof(OnCountChange))] private int _sapphireCounter { get; set; }

        public override void Spawned()
        {
            _sapphireCounter = 0;
            _totalSapphire = FindObjectsOfType<PowerUpSapphire>().Length;
            _sapphireText = GameObject.Find("SapphireCounter").GetComponent<Text>();
            SetText();
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
            if(Object.HasInputAuthority)
                SetText();
        }

        private void SetText(){
            _sapphireText.text = _sapphireCounter + " / " + _totalSapphire;
        }
    }
}
