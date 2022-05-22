using UnityEngine;

namespace Sapphire
{
    public class Wizard : Player
    {
        public override void Spawned()
        {
            base.Spawned();
            if (Object.HasInputAuthority)
                SetUI();
        }

        public void SetUI()
        {
            int baseXPosition = 90;
            int baseYPosition = 100;

            var timerUI = GameObject.Find("TimeLeft");
            var sbiresUI = GameObject.Find("Sbires");
            var sapphireUI = GameObject.Find("Sapphires");
            var ammoUI = GameObject.Find("Ammo");

            if (Object.HasInputAuthority)
            {
                GameObject.Find("Sapphires").SetActive(false);
                GameObject.Find("Ammo").SetActive(false);
            }

            timerUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition);
            sapphireUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 100);


        }
    }
}