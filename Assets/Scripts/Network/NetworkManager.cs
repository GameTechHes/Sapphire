using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using StarterAssets;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    private NetworkRunner _runner;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private StarterAssetsInputs _inputs;

    public void HostGame()
    {
        StartGameWithMode(GameMode.Host);
    }

    public void JoinGame()
    {
        StartGameWithMode(GameMode.Client);
    }

    private async void StartGameWithMode(GameMode mode)
    {
        _inputs.cursorLocked = true;

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = 1,
            SceneObjectProvider = GetComponent<NetworkSceneManagerDefault>()
        });
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        var inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        if (inputManager != null)
        {
            _inputs = inputManager.GetComponent<StarterAssetsInputs>();
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            print("Player join !");
            var networkObj = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            
            _spawnedCharacters.Add(player, networkObj);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        print("Player left !");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData
        {
            move = _inputs.move,
            look = _inputs.look,
            jump = _inputs.jump
        };
        input.Set(data);
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