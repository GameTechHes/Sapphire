using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Network
{
    public class InputController : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static bool fetchInput = true;
    
    private Player _player;
    private NetworkInputData _frameworkInput;
    private Vector2 _moveDelta;

    public void OnMove(InputValue value)
    {
        _moveDelta = value.Get<Vector2>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            _player.SetDirection(input.moveDirection.normalized);
        }
        _player.Move();
    }

    public override void Spawned()
    {
        _player = GetComponent<Player>();
        if (Object.HasInputAuthority)
        {
            Runner.AddCallbacks(this);
            GetComponent<PlayerInput>().enabled = true;
        }

        Debug.Log("Spawned [" + this + "] IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer + " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (_player != null && _player.Object != null && fetchInput)
        {
            _frameworkInput.moveDirection = _moveDelta.normalized;
        }
        
        input.Set(_frameworkInput);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}

public struct NetworkInputData : INetworkInput
{
    public Vector2 moveDirection;
}
}
