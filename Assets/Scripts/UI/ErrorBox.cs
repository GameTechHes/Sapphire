using TMPro;
using UnityEngine;

public class ErrorBox : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    private static ErrorBox instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public static void Message(string message)
    {
        instance.text.text = message;
        instance.gameObject.SetActive(true);
    }

    public void OnClose()
    {
        instance.gameObject.SetActive(false);
    }
}
