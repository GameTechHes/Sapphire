using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> rooms;

    public Corridor corridor;

    public int roomCount;

    // Start is called before the first frame update
    void Start()
    {
        Corridor lastCorridor = null;


        for (int i = 0; i < roomCount; i++)
        {
            
            Room generatedRoom =
                Instantiate(rooms[i].gameObject).GetComponent<Room>();
            if (lastCorridor != null)
            {
                generatedRoom.transform.rotation = lastCorridor.end.rotation * generatedRoom.doors[0].rotation;
                generatedRoom.transform.position = lastCorridor.end.transform.position -
                                                   generatedRoom.doors[0].position;
            }

            Transform roomDoor;
            if (generatedRoom.doors.Count > 1)
            {
                roomDoor = generatedRoom.doors[1];
            }
            else
            {
                roomDoor = generatedRoom.doors[0];
            }
            roomDoor.Translate(Vector3.forward * (corridor.length / 2), Space.Self);
            if (i < roomCount - 1)
            {
                lastCorridor = Instantiate(corridor, roomDoor);
            }
        }
    }


// Update is called once per frame
    void Update()
    {
    }
}