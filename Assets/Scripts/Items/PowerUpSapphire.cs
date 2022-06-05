using Sapphire;

namespace Items
{
    public class PowerUpSapphire : PowerUpBase
    {
        protected override void ApplyEffects(Player player)
        {
            var sapphireController = player.GetComponent<SapphireController>();
            if (sapphireController != null)
            {
                FindObjectOfType<AudioManager>().Play("CollectingSapphire");
                sapphireController.RPC_AddSapphire();
                gameObject.SetActive(false);
                RPC_Despawn();
            }
        }
    }
}