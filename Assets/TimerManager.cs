using System.Collections;
using Fusion;
using Sapphire;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour
{
    [Networked] private TickTimer _startingTimer { get; set; }
    [Networked] private TickTimer _gameTimer { get; set; }
    
    private bool started = false;

    public override void FixedUpdateNetwork()
    {
        if (_startingTimer.IsRunning)
        {
            var remainingTime = _startingTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                Player.Local._uiManager.SetCountDown((float)remainingTime + 1);
                //uiCountdown.SetText(((int) _startingTimer.RemainingTime(Runner) + 1).ToString());
            }
        }

        if (_startingTimer.Expired(Runner))
        {
            // timerText.text = "GO !";
            // uiCountdown.SetText("Gooo !");
            Player.Local._uiManager.SetCountDown("Gooo !");

            StartCoroutine(Player.Local._uiManager.HideCountdown());
            GameManager.instance.StartGame();
        }

        if (_gameTimer.IsRunning)
        {
            var remainingTime = _gameTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                Player.Local._uiManager.SetTimeLeft((float)remainingTime);
                //timerText.text = ((int) _gameTimer.RemainingTime(Runner) + 1).ToString();
            }
        }
        
        if (_gameTimer.Expired(Runner))
        {
            Player.Local._uiManager.SetTimeLeft("Time's up !");

            //timerText.text = "Time's up !";
        }
    }

    public void StartTimer()
    {
        if(!_startingTimer.IsRunning)
            _startingTimer = TickTimer.CreateFromSeconds(Runner, 5);
    }

    public void StartGameTimer()
    {
        if(!_gameTimer.IsRunning)
            _gameTimer = TickTimer.CreateFromSeconds(Runner, 3 * 60);
    }
}