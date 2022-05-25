using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour
{
    [Networked] private TickTimer _startingTimer { get; set; }
    [Networked] private TickTimer _gameTimer { get; set; }
    
    public Text timerText;
    public StartingCountdown uiCountdown;
    private bool started = false;

    public override void FixedUpdateNetwork()
    {
        if (_startingTimer.IsRunning)
        {
            // timerText.text = ((int) gameTimer.RemainingTime(Runner)).ToString();
            var remainingTime = _startingTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                uiCountdown.SetText(((int) _startingTimer.RemainingTime(Runner) + 1).ToString());
            }
        }

        if (_startingTimer.Expired(Runner))
        {
            // timerText.text = "GO !";
            uiCountdown.SetText("Gooo !");
            StartCoroutine(HideCountdown());
            GameManager.instance.StartGame();
        }

        if (_gameTimer.IsRunning)
        {
            var remainingTime = _gameTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                timerText.text = ((int) _gameTimer.RemainingTime(Runner) + 1).ToString();
            }
        }
        
        if (_gameTimer.Expired(Runner))
        {
            timerText.text = "Time's up !";
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

    private IEnumerator HideCountdown()
    {
        yield return new WaitForSeconds(3.0f);
        uiCountdown.gameObject.SetActive(false);
    }
}