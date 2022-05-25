using Items;
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