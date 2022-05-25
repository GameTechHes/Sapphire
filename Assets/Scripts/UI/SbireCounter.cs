using UnityEngine;
using UnityEngine.UI;

public class SbireCounter : MonoBehaviour
{
    private Text sbireText;

    void Start()
    {
        sbireText = GetComponent<Text>();
    }

    private void Update()
    {
        if (sbireText != null && GameManager.instance != null)
            sbireText.text = GameManager.instance.sbireNumber.ToString();
    }
}