using Sapphire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject timerUI;
    public GameObject countDownUI;
    public GameObject sbiresUI;
    public GameObject sapphireUI;
    public GameObject ammoUI;
    public GameObject lobbyUI;
    public GameObject minimapUI;

    public GameObject healthUI;
    public Slider _healthSlider;
    public TextMeshProUGUI healthCount;
    public Text timerUIText;
    public Text sbireCounter;

    public TextMeshProUGUI countDownText;

    [SerializeField] private TMP_Text _knightText;
    [SerializeField] private TMP_Text _wizardText;
    [SerializeField] private TMP_Text _wizardReady;
    [SerializeField] private TMP_Text _knightReady;


    public TMP_Text KnightText { get => _knightText; set => _knightText = value; }
    public TMP_Text WizardText { get => _wizardText; set => _wizardText = value; }
    public TMP_Text WizardReady { get => _wizardReady; set => _wizardReady = value; }
    public TMP_Text KnightReady { get => _knightReady; set => _knightReady = value; }

    public void SetUIPosition()
    {
        int baseXPosition = 90;
        int baseYPosition = 100;

        if (Player.Local.GetType() == typeof(Knight))
        {
            GameObject.Find("Sbires").SetActive(false);

            ammoUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 200);
            sapphireUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 100);
            timerUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition);

        }
        else
        {
            GameObject.Find("Sapphires").SetActive(false);
            GameObject.Find("Ammo").SetActive(false);

            timerUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition);
            sapphireUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(baseXPosition, baseYPosition + 100);
        }

        healthUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 100);

    }

    public void SetTimeLeft(float timeLeft)
    {
        timerUIText.text = ((int)timeLeft).ToString();
    }

    public void SetTimeLeft(string message)
    {
        timerUIText.text = message;
    }

    public void SetCountDown(float timeLeft)
    {
        countDownText.text = ((int)timeLeft).ToString();
    }

    public void SetCountDown(string message)
    {
        countDownText.text = message;
    }


    public IEnumerator HideCountdown()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("HideCountDown");
        countDownUI.SetActive(false);
    }

    public void SetHealth(int health, int maxHealth)
    {
        if (health >= 0 && _healthSlider != null)
        {
            _healthSlider.value = health / (float)maxHealth;
            healthCount.text = health.ToString();
        }
    }

    public void HideLobbyUI()
    {
        lobbyUI.SetActive(false);
    }

    public void MinimapDisplay(bool shouldDisplay)
    {
        minimapUI.SetActive(shouldDisplay);
    }

    public void DisplayMinimap()
    {
        minimapUI.SetActive(true);
    }
}
