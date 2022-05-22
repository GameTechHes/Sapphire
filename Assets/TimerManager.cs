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
    public float durationInSeconds = 600.0f;

    private void FixedUpdate()
    {
        var activePlayers = Runner.ActivePlayers;
        if (activePlayers.Count() >= 2)
        {
            if (!gameTimer.IsRunning)
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, durationInSeconds);
            }
            else
            {
                timerText.text = ((int)gameTimer.RemainingTime(Runner)).ToString();
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
