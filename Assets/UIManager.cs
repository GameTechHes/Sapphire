using Sapphire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using System;

public class UIManager : NetworkBehaviour
{

    [Header("Timers")]
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject countDownUI;
    [SerializeField] private Text timerUIText;
    [SerializeField] private TextMeshProUGUI countDownText;

    [Header("Sapphires")]
    [SerializeField] private GameObject sapphireUI;
    [SerializeField] private Text _sapphireText;

    [Header("Sbires")]
    [SerializeField] private GameObject sbiresUI;
    [SerializeField] private Text sbireCounter;

    [Header("Ammo")]
    [SerializeField] private GameObject ammoUI;
    [SerializeField] private Text _ammoText;


    [Header("Health")]
    [SerializeField] private GameObject healthUI;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthCount;

    [Header("Lobby")]
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private TMP_Text _knightText;
    [SerializeField] private TMP_Text _wizardText;
    [SerializeField] private TMP_Text _wizardReady;
    [SerializeField] private TMP_Text _knightReady;

    [Header("Minimap")]
    [SerializeField] private GameObject minimapUI;
    private GameObject knightIcon;


    [Header("End game messages")]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text gameOverText;

  
    public TMP_Text KnightText { get => _knightText; set => _knightText = value; }
    public TMP_Text WizardText { get => _wizardText; set => _wizardText = value; }
    public TMP_Text WizardReady { get => _wizardReady; set => _wizardReady = value; }
    public TMP_Text KnightReady { get => _knightReady; set => _knightReady = value; }
    public GameObject TimerUI { get => timerUI; set => timerUI = value; }
    public GameObject CountDownUI { get => countDownUI; set => countDownUI = value; }
    public GameObject SbiresUI { get => sbiresUI; set => sbiresUI = value; }
    public GameObject SapphireUI { get => sapphireUI; set => sapphireUI = value; }
    public GameObject AmmoUI { get => ammoUI; set => ammoUI = value; }
    public GameObject LobbyUI { get => lobbyUI; set => lobbyUI = value; }
    public GameObject MinimapUI { get => minimapUI; set => minimapUI = value; }
    public GameObject HealthUI { get => healthUI; set => healthUI = value; }
    public Slider HealthSlider { get => healthSlider; set => healthSlider = value; }
    public TextMeshProUGUI HealthCount { get => healthCount; set => healthCount = value; }
    public Text TimerUIText { get => timerUIText; set => timerUIText = value; }
    public Text SbireCounter { get => sbireCounter; set => sbireCounter = value; }
    public TextMeshProUGUI CountDownText { get => countDownText; set => countDownText = value; }
    public Text AmmoText { get => _ammoText; set => _ammoText = value; }
    public Text SapphireText { get => _sapphireText; set => _sapphireText = value; }
    public GameObject VictoryUI { get => victoryUI; set => victoryUI = value; }
    public GameObject GameOverUI { get => gameOverUI; set => gameOverUI = value; }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Victory(NetworkBool hasKnightWin, string victoryMessage, string gameOverMessage)
    {
        Type t = hasKnightWin ? typeof(Knight) : typeof(Wizard);

        if(Player.Local.GetType() == t)
        {
            victoryText.text = victoryMessage;
            StartCoroutine(DisplayVictory());
        }
        else
        {
            gameOverText.text = gameOverMessage;
            StartCoroutine(DisplayGameOver());
        }
    }
    public void SetUIPosition()
    {
        int baseXPosition = 90;
        int baseYPosition = 100;

        if (Player.Local.GetType() == typeof(Knight))
        {
            GameObject.Find("Sbires").SetActive(false);

            AmmoUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 200);
            SapphireUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 100);
            TimerUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition);


        }
        else
        {
            GameObject.Find("Sapphires").SetActive(false);
            GameObject.Find("Ammo").SetActive(false);

            TimerUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition);
            SapphireUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 100);
        }

        HealthUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 100);
        VictoryUI.SetActive(false);
        GameOverUI.SetActive(false);

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ShowKnightPosition()
    {
        if (Player.Local.GetType() == typeof(Wizard))
        {
            StartCoroutine(HideIcon());
        }
    }
    public IEnumerator HideIcon()
    {
        knightIcon = GameObject.FindGameObjectWithTag("KnightIcon");
        knightIcon.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(5.0f);
        knightIcon.GetComponent<MeshRenderer>().enabled = false;
    }
    public IEnumerator HideCountdown()
    {
        yield return new WaitForSeconds(3.0f);
        CountDownUI.SetActive(false);
    }

    public void SetSapphire(int current, int total)
    {
        SapphireText.text = current + " / " + total;
    }
    public void SetHealth(int health, int maxHealth)
    {
        if (health >= 0 && HealthSlider != null)
        {
            HealthSlider.value = health / (float)maxHealth;
            HealthCount.text = health.ToString();
        }
    }

    public void HideLobbyUI()
    {
        LobbyUI.SetActive(false);
    }

    public void MinimapDisplay(bool shouldDisplay)
    {
        MinimapUI.SetActive(shouldDisplay);
    }

    public void DisplayMinimap()
    {
        MinimapUI.SetActive(true);
    }

    //Désolé pour ce nom de fonction
    public void DisplayWizzardLostKnightWon()
    {
        if(Player.Local.GetType() == typeof(Knight)){
            VictoryUI.SetActive(true);
        }
        else
        {
            GameOverUI.SetActive(true);
        }
    }

    public void DisplayKnightLostWizzardWon()
    {
        if (Player.Local.GetType() == typeof(Knight))
        {
            GameOverUI.SetActive(true);
        }
        else
        {
            VictoryUI.SetActive(true);

        }
    }

    private IEnumerator DisplayGameOver()
    {
        GameOverUI.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        GameOverUI.SetActive(false);
    }

    private IEnumerator DisplayVictory()
    {
        VictoryUI.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        VictoryUI.SetActive(false);
    }


}
