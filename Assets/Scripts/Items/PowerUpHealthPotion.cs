using Sapphire;

namespace Items
{
    public class PowerUpHealthPotion : PowerUpBase
    {
        protected override void ApplyEffects(Player player)
        {
            if (player.Health != 100)
            {
                player.RPC_AddHealth(25);
                gameObject.SetActive(false);
                RPC_Despawn();
            }
        }
    }
}