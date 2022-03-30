using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> roomPrefabs;

    public Corridor corridorPrefab;

    public int totalRoomCount;

    private int roomCount = 0;
    private const float SCALE_FACTOR = 18.0f * 2.0f;
    
    private Stack<RoomToInstantiate> roomsToInstantiate = new Stack<RoomToInstantiate>();

    // Start is called before the first frame update
    void Start()
    {
        InstantiateRoom(roomPrefabs[0], 0, 0);
        // grid = new Room[gridSize.x, gridSize.y];
        //
        //
        // for (int i = 0; i < gridSize.x; i++)
        // {
        //     for (int j = 0; j < gridSize.y; j++)
        //     {
        //         if (roomCount == totalRoomCount)
        //         {
        //             break;
        //         }
        //
        //         roomCount++;
        //         var room = Instantiate(roomPrefabs[0], new Vector3(i * SCALE_FACTOR, 0.0f, j * SCALE_FACTOR),
        //             Quaternion.identity).GetComponent<Room>();
        //         foreach (var door in room.GetActiveDoors())
        //         {
        //             var corridor = Instantiate(corridorPrefab, door).GetComponent<Corridor>();
        //             corridor.transform.position += corridor.transform.position - corridor.start.position;
        //         }
        //     }
        }

        void InstantiateRoom(Room roomPrefab, int x, int z)
        {
            roomCount++;
            var room = Instantiate(roomPrefabs[0], new Vector3(x * SCALE_FACTOR, 0.0f, z * SCALE_FACTOR),
                Quaternion.identity).GetComponent<Room>();
            foreach (var door in room.GetActiveDoors())
            {
                var corridor = Instantiate(corridorPrefab, door).GetComponent<Corridor>();
                corridor.transform.position += corridor.transform.position - corridor.start.position;
                var xFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.forward));
                var zFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.right));
                if (roomCount < totalRoomCount)
                {
                    InstantiateRoom(roomPrefabs[0], x + xFactor, z + zFactor);
                }
            }
        }

        // Corridor lastCorridor = null;
        //
        //
        // for (int i = 0; i < roomCount; i++)
        // {
        //     
        //     Room generatedRoom =
        //         Instantiate(rooms[i].gameObject).GetComponent<Room>();
        //     if (lastCorridor != null)
        //     {
        //         generatedRoom.transform.rotation = lastCorridor.end.rotation * generatedRoom.doors[0].rotation;
        //         generatedRoom.transform.position = lastCorridor.end.transform.position -
        //                                            generatedRoom.doors[0].position;
        //     }
        //
        //     Transform roomDoor;
        //     if (generatedRoom.doors.Count > 1)
        //     {
        //         roomDoor = generatedRoom.doors[1];
        //     }
        //     else
        //     {
        //         roomDoor = generatedRoom.doors[0];
        //     }
        //     roomDoor.Translate(Vector3.forward * (corridor.length / 2), Space.Self);
        //     if (i < roomCount - 1)
        //     {
        //         lastCorridor = Instantiate(corridor, roomDoor);
        //     }
        // }
    // }


// Update is called once per frame
    void Update()
    {
    }
}