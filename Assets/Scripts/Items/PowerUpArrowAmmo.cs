using Sapphire;

namespace Items
{
    public class PowerUpArrowAmmo : PowerUpBase
    {
        public const int ammoReward = 10;

        protected override void ApplyEffects(Player player)
        {
            var shoot = player.GetComponent<Shoot>();
            if (shoot != null)
            {
                shoot.RPC_AddAmmo(ammoReward);
                FindObjectOfType<AudioManager>().Play("CollectingAmmo");
                gameObject.SetActive(false);
                RPC_Despawn();
            }
        }
    }
}