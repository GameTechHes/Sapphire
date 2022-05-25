using TMPro;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{
    private TMP_Text _countdown;

    private void Awake()
    {
        _countdown = GetComponent<TMP_Text>();
    }

    public void SetText(string text)
    {
        _countdown.text = text;
    }
}
