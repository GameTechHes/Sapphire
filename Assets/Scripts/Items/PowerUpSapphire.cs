using Sapphire;

namespace Items
{
    public class PowerUpSapphire : PowerUpBase
    {
        protected override void ApplyEffects(Player player)
        {
            // player.GetComponent<SapphireController>().AddSapphire();
            FindObjectOfType<AudioManager>().Play("CollectingSapphire");
            if(Object != null && Object.IsValid)
                Runner.Despawn(Object);
        }
    }
}