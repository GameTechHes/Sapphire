using Sapphire;

namespace Items
{
    public class PowerUpSapphire : PowerUpBase
    {
        protected override void ApplyEffects(Player player)
        {
            if (player.Object.HasInputAuthority)
            {
                var sapphireController = player.GetComponent<SapphireController>();

                if (sapphireController != null)
                {
                    sapphireController.RPC_AddSapphire();
                }
            }

            if (player.Object.HasInputAuthority)
                FindObjectOfType<AudioManager>().Play("CollectingSapphire");

            if (Object != null && Object.IsValid)
                Runner.Despawn(Object);
        }
    }
}