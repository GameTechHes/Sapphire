using Sapphire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject timerUI;
    public GameObject sbiresUI;
    public GameObject sapphireUI;
    public GameObject ammoUI;

    public GameObject healthUI;
    public Slider _healthSlider;
    public TextMeshProUGUI healthCount;


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

    public void SetHealth(int health, int maxHealth)
    {
        if (health >= 0 && _healthSlider != null)
        {
            _healthSlider.value = health / (float)maxHealth;
            healthCount.text = health.ToString();
        }
    }
}
