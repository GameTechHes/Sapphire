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
                sapphireController.RPC_AddSapphire(Object);
                gameObject.SetActive(false);
            }
            FindObjectOfType<AudioManager>().Play("CollectingSapphire");
        }
    }
}