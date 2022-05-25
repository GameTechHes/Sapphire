using System.Linq;
using Fusion;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour
{
    [Networked] private TickTimer gameTimer { get; set; }
    public Text timerText;
    private float duration = 60.0f;

    public override void FixedUpdateNetwork()
    {
        if (!Runner)
            return;

        var activePlayers = Runner.ActivePlayers;
        // Deux joueurs sont connectés
        if (activePlayers.Count() >= 2)
        {
            // Si le timer n'est pas lancé ni
            if (!gameTimer.IsRunning)
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, duration);
            }
            else
            {
                timerText.text = ((int) gameTimer.RemainingTime(Runner)).ToString();
            }
        }
        else
        {
            if (!gameTimer.Expired(Runner))
            {
                timerText.text = duration.ToString();
            }
        }
    }
}