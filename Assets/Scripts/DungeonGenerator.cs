using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> roomPrefabs;

    public Corridor corridorPrefab;

    public int totalRoomCount;

    private int roomCount = 0;
    private const float SCALE_FACTOR = 18.0f * 2.0f;

    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

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

    private void InstantiateRoom(Room roomPrefab, int x, int y)
    {
        if (rooms.ContainsKey(new Vector2Int(x, y)))
        {
            return;
        }
        roomCount++;
        var rotation = GetNewRoomRotation(roomPrefab, x, y);
        var room = Instantiate(roomPrefab, new Vector3(x * SCALE_FACTOR, 0.0f, y * SCALE_FACTOR),
            rotation).GetComponent<Room>();
        room.SetIndexes(x, y);
        
        rooms.Add(new Vector2Int(x, y), room);

        if (roomPrefab.GetActiveDoors().Count > 1)
        {
            foreach (var door in room.GetActiveDoors())
            {
                if (roomCount < totalRoomCount)
                {
                    var corridor = Instantiate(corridorPrefab, door).GetComponent<Corridor>();
                    corridor.transform.position += corridor.transform.position - corridor.start.position;
                    var xFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.forward));
                    var zFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.right));
                    // TODO: Add randomness
                    var roomPrefabToInstantiate = roomPrefabs[Mathf.Min(roomCount, roomPrefabs.Count - 1)];
                    InstantiateRoom(roomPrefabToInstantiate, x + xFactor, y + zFactor);
                }
            }
        }
    }

    private Quaternion GetNewRoomRotation(Room room, int xIndex, int yIndex)
    {
        var adjRooms = GetAdjacentRooms(xIndex, yIndex);

        // For each orientation
        for (int i = 0; i < 4; i++)
        {
            var isOrientationValid = true;
            // For each adjacent room
            for (int j = 0; j < 4; j++)
            {
                // If adjacent room is not null
                if (adjRooms[j] != null)
                {
                    // Check if adjacent doors are both active
                    if (!(room.isDoorActive[(i + j) % room.isDoorActive.Count] &&
                        adjRooms[j].GetDoorsInOrder()[(i + 2) % 4]))
                    {
                        isOrientationValid = false;
                        break;
                    }
                }
            }

            if (isOrientationValid)
            {
                return Quaternion.AngleAxis(i * 90, Vector3.up);
            }
            // Return rotation of first match
        }

        return Quaternion.identity;
    }

    private Room[] GetAdjacentRooms(int xIndex, int yIndex)
    {
        var adjRooms = new Room[]
        {
            null, // +x 
            null, // +y
            null, // -x
            null // -y
        };
        var coord = new Vector2Int(xIndex + 1, yIndex);
        if (rooms.ContainsKey(coord))
            adjRooms[0] = rooms[coord];
        coord = new Vector2Int(xIndex, yIndex + 1);
        if (rooms.ContainsKey(coord))
            adjRooms[1] = rooms[coord];
        coord = new Vector2Int(xIndex - 1, yIndex);
        if (rooms.ContainsKey(coord))
            adjRooms[2] = rooms[coord];
        coord = new Vector2Int(xIndex, yIndex - 1);
        if (rooms.ContainsKey(coord))
            adjRooms[3] = rooms[coord];
        

        return adjRooms;
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
    
}