using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [Networked] private TickTimer gameTimer { get; set; }
    public Text timerText;
    private float duration = 15.0f;

    public override void FixedUpdateNetwork()
    {
        var activePlayers = Runner.ActivePlayers;
        // Deux joueurs sont connectés
        if (activePlayers.Count() >= 2)
        {
            // Si le timer n'est pas lamcé ni
            if (!gameTimer.IsRunning)
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, duration);
            }
            else
            {
                timerText.text = ((int)gameTimer.RemainingTime(Runner)).ToString();
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
