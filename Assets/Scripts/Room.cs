using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject doorModel;

    [Header("Doors transforms")]
    public Transform door1;
    public Transform door2;
    public Transform door3;
    public Transform door4;

    [Header("Doors activation")]
    public bool isDoor1Active;
    public bool isDoor2Active;
    public bool isDoor3Active;
    public bool isDoor4Active;
    
    [Header("Doors initial state")]
    public bool isDoor1Present;
    public bool isDoor2Present;
    public bool isDoor3Present;
    public bool isDoor4Present;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!isDoor1Active || isDoor1Present)
        {
            var door = Instantiate(doorModel, door1);
            door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
        }
        if (!isDoor2Active || isDoor2Present)
        {
            var door = Instantiate(doorModel, door2);
            door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
        }
        if (!isDoor3Active || isDoor3Present)
        {
            var door = Instantiate(doorModel, door3);
            door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
        }
        if (!isDoor4Active || isDoor4Present)
        {
            var door = Instantiate(doorModel, door4);
            door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
        }
    }

    public List<Transform> GetActiveDoors()
    {
        var activeDoors = new List<Transform>();
        if(isDoor1Active) activeDoors.Add(door1);
        if(isDoor2Active) activeDoors.Add(door2);
        if(isDoor3Active) activeDoors.Add(door3);
        if(isDoor4Active) activeDoors.Add(door4);
        return activeDoors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
