using Sapphire;

namespace Items
{
    public class PowerUpArrowAmmo : PowerUpBase
    {
        public const int ammoReward = 10;

        protected override void ApplyEffects(Player player)
        {
            // TODO: merge knight in one root gameobject
            // player.GetComponent<Shoot>().AddAmmo(ammoReward);
            FindObjectOfType<AudioManager>().Play("CollectingAmmo");
            if (Object != null && Object.IsValid)
                Runner.Despawn(Object);
        }
    }
}