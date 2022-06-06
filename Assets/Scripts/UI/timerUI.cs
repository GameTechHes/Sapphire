using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerUI : MonoBehaviour
{
    private GameManager gameManager;
    public Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
