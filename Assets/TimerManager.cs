using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour
{
    [Networked] private TickTimer gameTimer { get; set; }
    public Text timerText;
    public float durationInSeconds = 600.0f;

    public override void FixedUpdateNetwork()
    {
        if(!Runner)
            return;
        
        var activePlayers = Runner.ActivePlayers;
        if (activePlayers.Count() >= 2)
        {
            if (!gameTimer.IsRunning)
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, durationInSeconds);
            }
            else
            {
                timerText.text = ((int) gameTimer.RemainingTime(Runner)).ToString();
            }

            Debug.Log(gameTimer.RemainingTime(Runner).ToString());
        }
        else
        {
            if (!gameTimer.Expired(Runner))
            {
                timerText.text = durationInSeconds.ToString();
            }
        }
    }
}