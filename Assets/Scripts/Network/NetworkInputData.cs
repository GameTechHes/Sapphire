using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 move;
    public Vector2 look;
    public NetworkBool jump;
}
