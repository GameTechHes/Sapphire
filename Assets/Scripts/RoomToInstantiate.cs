using UnityEngine;

public struct RoomToInstantiate
{ 
    public GameObject prefab;
    public int xIndex;
    public int zIndex;

    public RoomToInstantiate(GameObject prefab, int xIndex, int zIndex)
    {
        this.prefab = prefab;
        this.xIndex = xIndex;
        this.zIndex = zIndex;
    }
}