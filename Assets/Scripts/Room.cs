using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject doorModel;

    [Header("Doors transforms")] public List<Transform> doors;

    [Header("Doors activation")] public List<bool> isDoorActive;

    private int xIndex;
    private int zIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < isDoorActive.Count; i++)
        {
            if (!isDoorActive[i] && doors[i] != null)
            {
                var door = Instantiate(doorModel, doors[i]);
                door.transform.Translate(Vector3.left * 0.675f + Vector3.back * 0.1f, Space.Self);
            }
        }
    }

    public List<Transform> GetActiveDoors()
    {
        var activeDoors = new List<Transform>();
        for (int i = 0; i < isDoorActive.Count; i++)
        {
            if (isDoorActive[i]) activeDoors.Add(doors[i]);
        }

        return activeDoors;
    }

    /**
     * res[0] = +x direction
     * res[1] = +y direction
     * res[2] = -x direction
     * res[3] = -y direction
     */
    public List<Transform> GetDoorsInOrder()
    {
        var rotationAmount = Mathf.RoundToInt(Quaternion.Angle(Quaternion.identity, transform.rotation) / 90) % 4;
        var result = new List<Transform>();
        for (var i = 0; i < isDoorActive.Count; i++)
        {
            result.Add(doors[(i + rotationAmount) % doors.Count]);
        }

        return result;
    }

    public void SetIndexes(int xIndex, int zIndex)
    {
        this.xIndex = xIndex;
        this.zIndex = zIndex;
    }

}