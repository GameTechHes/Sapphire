using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class SapphireController : MonoBehaviour
    {
        public Text sapphireText;

        private int _totalSapphire;
        private int _sapphireCounter;

        void Start()
        {
            _totalSapphire = FindObjectsOfType<PowerUpSapphire>().Length;
            SetText();
        }

        public void AddSapphire()
        {
            _sapphireCounter += 1;
            SetText();
        }

        private void SetText(){
            sapphireText.text = _sapphireCounter + " / " + _totalSapphire;
        }
    }
}
