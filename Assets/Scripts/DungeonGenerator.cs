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
        // var firstRoom = Instantiate(rooms[0].gameObject).GetComponent<Room>();
        // foreach (var roomDoor in firstRoom.doors)
        // {
        //     roomDoor.Translate(Vector3.forward * (corridor.length / 2), Space.Self);
        //     var newCorridor = Instantiate(corridor, roomDoor);
        //     var newRoom = Instantiate(rooms[1], - newCorridor.end.transform.position -
        //                                         (Quaternion.AngleAxis(180, Vector3.up) * newRoom.doors[0].position));
        //     newRoom.transform.Rotate(Vector3.up, 180);
        //     newRoom.transform.Translate();
        //     Debug.Log(newCorridor.end.transform.position);
        //     Debug.Log(newRoom.transform.position);
        // }


        Corridor lastCorridor = null;


        for (int i = 0; i < roomCount; i++)
        {
            
            Room generatedRoom =
                Instantiate(rooms[i].gameObject).GetComponent<Room>();
            if (lastCorridor != null)
            {
                Debug.Log(generatedRoom.transform.rotation);
                generatedRoom.transform.rotation = lastCorridor.end.rotation * generatedRoom.doors[0].rotation;
                generatedRoom.transform.position = lastCorridor.end.transform.position -
                                                   generatedRoom.doors[0].position;
                Debug.Log(lastCorridor.end.rotation);
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
            // var newRoom = Instantiate(rooms[1]);
            // newRoomRotation = Quaternion.AngleAxis(180, Vector3.up);

        }
    }


// Update is called once per frame
    void Update()
    {
    }
}