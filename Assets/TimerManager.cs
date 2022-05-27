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
                UIManager.Instance.CountDownText.text = ((int)remainingTime + 1).ToString();
                //uiCountdown.SetText(((int) _startingTimer.RemainingTime(Runner) + 1).ToString());
            }
        }

        if (_startingTimer.Expired(Runner))
        {
            // timerText.text = "GO !";
            // uiCountdown.SetText("Gooo !");
            UIManager.Instance.CountDownText.text = "Gooo !";

            StartCoroutine(UIManager.Instance.HideCountdown());
            GameManager.instance.StartGame();
        }

        if (_gameTimer.IsRunning)
        {
            var remainingTime = _gameTimer.RemainingTime(Runner);
            if (remainingTime != null)
            {
                UIManager.Instance.TimerUIText.text = ((int)remainingTime).ToString();
                //timerText.text = ((int) _gameTimer.RemainingTime(Runner) + 1).ToString();
            }
        }
        
        if (_gameTimer.Expired(Runner))
        {
            UIManager.Instance.TimerUIText.text = "-";

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
            _gameTimer = TickTimer.CreateFromSeconds(Runner, gameDurationInSeconds);
    }

    public bool CheckEndGame()
    {
        return _gameTimer.Expired(Runner);
    }
}