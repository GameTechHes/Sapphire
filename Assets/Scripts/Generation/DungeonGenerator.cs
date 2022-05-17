using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Generation
{
    public class DungeonGenerator : MonoBehaviour
    {
        public Room centralRoomPrefab;
        public List<Room> roomPrefabs;

        public Corridor corridorPrefab;

        [FormerlySerializedAs("totalRoomCount")]
        public int maxRooms;

        private int _roomCount;
        private const float ScaleFactor = 18.0f * 2.0f;

        private readonly Dictionary<Vector2Int, Room> _rooms = new Dictionary<Vector2Int, Room>();


        void Start()
        {
            InstantiateRoom(centralRoomPrefab, 0, 0);
            var bake = GetComponent<RuntimeBaker>();

            /***
            * We can do this because the RuntimeBaker script is executed before
            * So we are sure that bake is initialized
            */
            bake.BakeAll();
        }

        private void InstantiateRoom(Room roomPrefab, int x, int y)
        {
            if (_rooms.ContainsKey(new Vector2Int(x, y)))
            {
                return;
            }

            _roomCount++;
            var rotationIndex = GetNewRoomRotation(roomPrefab, x, y);
            if (rotationIndex == null)
            {
                return;
            }

            var rotation = Quaternion.AngleAxis(rotationIndex.Value * 90, Vector3.up);
            var room = Instantiate(roomPrefab, new Vector3(x * ScaleFactor, 0.0f, y * ScaleFactor),
                rotation).GetComponent<Room>();

            _rooms.Add(new Vector2Int(x, y), room);

            if (roomPrefab.GetActiveDoors().Count > 1)
            {
                for (int i = 0; i < roomPrefab.isDoorActive.Count; i++)
                {
                    if (roomPrefab.isDoorActive[i] && _roomCount < maxRooms)
                    {
                        var corridor = Instantiate(corridorPrefab, room.doorsTransforms[i]).GetComponent<Corridor>();
                        corridor.transform.position += corridor.transform.position - corridor.start.position;
                        var xFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.right));
                        var yFactor = Mathf.RoundToInt(Vector3.Dot(corridor.end.transform.forward, Vector3.forward));
                        var roomPrefabToInstantiate = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
                        InstantiateRoom(roomPrefabToInstantiate, x + xFactor, y + yFactor);
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
            if (_rooms.ContainsKey(coord))
                adjRooms[0] = _rooms[coord];
            coord = new Vector2Int(xIndex, yIndex + 1);
            if (_rooms.ContainsKey(coord))
                adjRooms[1] = _rooms[coord];
            coord = new Vector2Int(xIndex - 1, yIndex);
            if (_rooms.ContainsKey(coord))
                adjRooms[2] = _rooms[coord];
            coord = new Vector2Int(xIndex, yIndex - 1);
            if (_rooms.ContainsKey(coord))
                adjRooms[3] = _rooms[coord];


            return adjRooms;
        }
    }
}