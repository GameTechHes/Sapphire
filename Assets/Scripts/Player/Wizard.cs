using UnityEngine;

namespace Sapphire
{
    public class Wizard : Player
    {
        public override void Spawned()
        {
            base.Spawned();
            if (Object.HasInputAuthority)
            {
                GameObject.Find("Sapphires").SetActive(false);
                GameObject.Find("Ammo").SetActive(false);
            }
        }
    }
}