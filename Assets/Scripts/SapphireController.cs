using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SapphireController : MonoBehaviour
{
    public int totalSapphire;
    private int sapphireCounter = 0;
    public Text sapphireText;

    void setText(){
        sapphireText.text = sapphireCounter.ToString() + " / " + totalSapphire.ToString();
    }

    void Start()
    {
        setText();
    }

    public void AddSapphire()
    {
        sapphireCounter += 1;
        setText();
        Debug.Log("Sapphire collected!");
    }
}
