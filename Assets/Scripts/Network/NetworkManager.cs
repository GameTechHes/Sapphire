using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using StarterAssets;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    private StarterAssetsInputs _starterAssetsInputs;
    private ConnectionStatus _status;

    private Action<NetworkRunner, ConnectionStatus, string> _connectionStatusChangeCallback;
    private Action<NetworkRunner> _spawnWorldCallback;
    private Action<NetworkRunner, PlayerRef> _spawnPlayerCallback;
    private Action<NetworkRunner, PlayerRef> _despawnPlayerCallback;

    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Failed,
        Connected,
        Loading,
        Loaded
    }

    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    public async void StartGame(GameMode mode,
        String roomName,
        INetworkSceneObjectProvider sceneLoader,
        Action<NetworkRunner, ConnectionStatus, string> onConnectStatusChange,
        Action<NetworkRunner> onSpawnWorld,
        Action<NetworkRunner, PlayerRef> onSpawnPlayer,
        Action<NetworkRunner, PlayerRef> onDespawnPlayer)
    {
        _connectionStatusChangeCallback = onConnectStatusChange;
        _spawnWorldCallback = onSpawnWorld;
        _spawnPlayerCallback = onSpawnPlayer;
        _despawnPlayerCallback = onDespawnPlayer;

        SetConnectionStatus(ConnectionStatus.Connecting, "");

        DontDestroyOnLoad(gameObject);

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = roomName,
            SceneObjectProvider = sceneLoader,
        });
    }

    public void SetConnectionStatus(ConnectionStatus status, string message)
    {
        _status = status;
        if (_connectionStatusChangeCallback != null)
            _connectionStatusChangeCallback(_runner, status, message);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log("Spawning Player");
            InstantiatePlayer(runner, player);
        }
    }

    private void InstantiatePlayer(NetworkRunner runner, PlayerRef playerref)
    {
        if (_spawnWorldCallback != null && runner.IsServer)
        {
            _spawnWorldCallback(runner);
            _spawnWorldCallback = null;
        }

        _spawnPlayerCallback(runner, playerref);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log("Despawning Player");
            _despawnPlayerCallback(runner, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (_starterAssetsInputs != null)
        {
            var inputData = new NetworkInputData
            {
                move = _starterAssetsInputs.move,
                look = _starterAssetsInputs.look,
                sprint = _starterAssetsInputs.sprint,
                aim = _starterAssetsInputs.aim,
            };
            if (_starterAssetsInputs.shoot)
            {
                inputData.shoot = true;
                _starterAssetsInputs.shoot = false;
            }

            if (_starterAssetsInputs.jump)
            {
                inputData.jump = true;
                _starterAssetsInputs.jump = false;
            }

            input.Set(inputData);
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server");
        SetConnectionStatus(ConnectionStatus.Connected, "");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from server");
        SetConnectionStatus(ConnectionStatus.Disconnected, "");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        request.Accept();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"Connect failed {reason}");
        SetConnectionStatus(ConnectionStatus.Failed, reason.ToString());
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SetConnectionStatus(ConnectionStatus.Disconnected, "");
        if (_runner != null && _runner.gameObject)
        {
            Destroy(_runner);
        }
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
    public Vector2 move;
    public Vector2 look;
    public NetworkBool jump;
    public NetworkBool sprint;
    public NetworkBool aim;
    public NetworkBool shoot;
}