using Items;
using Sapphire;

public class PowerUpHealthPotion : PowerUpBase
{
    protected override void ApplyEffects(Player player)
    {
        // int playerhealth = obj.gameObject.GetComponent<Knight>().GetPlayerHealth();
        //
        // if (playerhealth != 100)
        // {
        //     obj.gameObject.GetComponent<Knight>().SetPlayerHealth(playerhealth + 25);
        //     Runner.Despawn(Object);
        // }
        if(Object != null && Object.IsValid)
            Runner.Despawn(Object);
    }
}
