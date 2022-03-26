using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> rooms;

    public Corridor corridor;
    // Start is called before the first frame update
    void Start()
    {
        var firstRoom = Instantiate(rooms[0].gameObject).GetComponent<Room>();
        foreach (var roomDoor in firstRoom.doors)
        {
            roomDoor.Translate(Vector3.forward * (corridor.length / 2), Space.Self);
            var newCorridor = Instantiate(corridor, roomDoor);
            var newRoom = Instantiate(rooms[1]);
            newRoom.transform.Rotate(Vector3.up, 180);
            newRoom.transform.Translate(newRoom.transform.position - newCorridor.end.transform.position -
                                        (Quaternion.AngleAxis(180, Vector3.up) * newRoom.doors[0].position));
            Debug.Log(newCorridor.end.transform.position);
            Debug.Log(newRoom.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
