using Fusion;
using Sapphire;

public class TimerManager : NetworkBehaviour
{
    [Networked] private TickTimer _startingTimer { get; set; }
    [Networked] private TickTimer _gameTimer { get; set; }

    public int gameDurationInSeconds;

    public override void FixedUpdateNetwork()
    {
        if (_startingTimer.IsRunning)
        {
            var remainingTime = _startingTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                Player.Local._uiManager.CountDownText.text = ((int) remainingTime + 1).ToString();
                //uiCountdown.SetText(((int) _startingTimer.RemainingTime(Runner) + 1).ToString());
            }
        }

        if (_startingTimer.Expired(Runner))
        {
            GameManager.instance.StartGame();
        }

        if (_gameTimer.IsRunning)
        {
            var remainingTime = _gameTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                Player.Local._uiManager.TimerUIText.text = ((int) remainingTime).ToString();
                //timerText.text = ((int) _gameTimer.RemainingTime(Runner) + 1).ToString();
            }
        }

        if (_gameTimer.Expired(Runner))
        {
            Player.Local._uiManager.TimerUIText.text = "-";

            //timerText.text = "Time's up !";
        }
    }

    public void StartTimer()
    {
        _startingTimer = TickTimer.CreateFromSeconds(Runner, 5);
    }

    public void StartGameTimer()
    {
        _gameTimer = TickTimer.CreateFromSeconds(Runner, gameDurationInSeconds);
    }

    public bool CheckEndGame()
    {
        return _gameTimer.Expired(Runner);
    }
}