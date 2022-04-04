using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<Room> roomPrefabs;

    public Corridor corridorPrefab;

    public int totalRoomCount;

    private int roomCount;
    private const float SCALE_FACTOR = 18.0f * 2.0f;

    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    // Start is called before the first frame update
    void Start()
    {
        InstantiateRoom(roomPrefabs[0], 0, 0, new Vector2Int(0, 0));
    }

    private void InstantiateRoom(Room roomPrefab, int x, int y, Vector2Int origin)
    {
        if (rooms.ContainsKey(new Vector2Int(x, y)))
        {
            return;
        }

        roomCount++;
        var rotationIndex = GetNewRoomRotation(roomPrefab, x, y);
        if (rotationIndex == null)
        {
            return;
        }

        var rotation = Quaternion.AngleAxis(rotationIndex.Value * 90, Vector3.up);
        var room = Instantiate(roomPrefab, new Vector3(x * SCALE_FACTOR, 0.0f, y * SCALE_FACTOR),
            rotation).GetComponent<Room>();
        Debug.Log("Instantiate " + room.gameObject.name + $" at {x},{y}");
        room.rotation = rotationIndex.Value;

        rooms.Add(new Vector2Int(x, y), room);

        if (roomPrefab.GetActiveDoors().Count > 1)
        {
            for (int i = 0; i < roomPrefab.isDoorActive.Count; i++)
            {
                if (roomPrefab.isDoorActive[i] && roomCount < totalRoomCount)
                {
                    var corridor = Instantiate(corridorPrefab, room.doorsTransforms[i]).GetComponent<Corridor>();
                    corridor.transform.position += corridor.transform.position - corridor.start.position;
                    var xFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.right));
                    var yFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.forward));
                    Debug.Log(Vector3.Dot(corridor.end.transform.forward, Vector3.forward) + ", " + Vector3.Dot(corridor.end.transform.forward, Vector3.right));
                    // TODO: Add randomness
                    var roomPrefabToInstantiate = roomPrefabs[Mathf.Min(roomCount, roomPrefabs.Count - 1)];
                    InstantiateRoom(roomPrefabToInstantiate, x + xFactor, y + yFactor,
                        new Vector2Int(-xFactor, -yFactor));
                }
            }
        }
    }

    private int? GetNewRoomRotation(Room room, int xIndex, int yIndex)
    {
        var adjRooms = GetAdjacentRooms(xIndex, yIndex);

        if (adjRooms.Count(a => a != null) == 0)
        {
            return 0;
        }

        // For each orientations
        for (int i = 0; i < 4; i++)
        {
            // For each adjacent rooms
            for (int j = 0; j < 4; j++)
            {
                // If adjacent room is not null
                if (adjRooms[j] != null)
                {
                    // Check if adjacent doors are both active
                    if (room.isDoorActive[(i + j) % room.isDoorActive.Count] &&
                        adjRooms[j].GetDoorsInOrder()[(i + 2) % 4])
                    {
                        return i;
                    }
                }
            }
            // Return rotation of first match
        }

        return null;
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
}