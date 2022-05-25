using Sapphire;

namespace Items
{
    public class PowerUpHealthPotion : PowerUpBase
    {
        protected override void ApplyEffects(Player player)
        {
            if (player.Health != 100)
            {
                FindObjectOfType<AudioManager>().Play("CollectingPotion");
                player.RPC_AddHealth(25);
                gameObject.SetActive(false);
                RPC_Despawn();
            }
        }
    }
}