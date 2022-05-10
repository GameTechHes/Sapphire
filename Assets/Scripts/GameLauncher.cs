using Fusion;
using Sapphire;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] private GameManager _gameManagerPrefab;
    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _roomPanel;
    [SerializeField] private RectTransform _listPanel;
    [SerializeField] private TMP_InputField _room;
    [SerializeField] private TMP_InputField _username;
    [SerializeField] private Player _playerPrefab;

    private GameMode _gameMode;
    private NetworkManager.ConnectionStatus _status = NetworkManager.ConnectionStatus.Disconnected;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (_roomPanel != null && Keyboard.current != null && Keyboard.current.escapeKey.isPressed)
        {
            if (_roomPanel.gameObject.activeInHierarchy)
            {
                _roomPanel.gameObject.SetActive(false);
                _mainMenu.gameObject.SetActive(true);
            }
        }
    }

    void SetGameMode(GameMode gameMode)
    {
        _gameMode = gameMode;
        _mainMenu.gameObject.SetActive(false);
        _roomPanel.gameObject.SetActive(true);
    }

    public void OnHost() => SetGameMode(GameMode.Host);
    public void OnJoin() => SetGameMode(GameMode.Client);

    public void OnStartGame()
    {
        ClientInfo.Username = _username.text;
        ClientInfo.LobbyName = _room.text;
        
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager == null)
        {
            networkManager = new GameObject("NetworkManager").AddComponent<NetworkManager>();
        }

        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.NetworkManager = networkManager;

        networkManager.StartGame(_gameMode, ClientInfo.LobbyName, levelManager, OnConnectionStatusUpdate, OnSpawnWorld,
            OnSpawnPlayer, OnDespawnPlayer);

        _roomPanel.gameObject.SetActive(false);
        _listPanel.gameObject.SetActive(true);
    }

    private void OnConnectionStatusUpdate(NetworkRunner runner, NetworkManager.ConnectionStatus status, string reason)
    {
        if (!this)
            return;

        Debug.Log(status);

        if (status != _status)
        {
            switch (status)
            {
                case NetworkManager.ConnectionStatus.Disconnected:
                    // ErrorBox.Show("Disconnected!", reason, () => { });
                    break;
                case NetworkManager.ConnectionStatus.Failed:
                    // ErrorBox.Show("Error!", reason, () => { });
                    break;
            }
        }

        _status = status;
    }

    private void OnSpawnWorld(NetworkRunner runner)
    {
        Debug.Log("Spawning GameManager");
        runner.Spawn(_gameManagerPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);

        void InitNetworkState(NetworkRunner runner, NetworkObject world)
        {
            world.transform.parent = transform;
        }
    }

    private void OnSpawnPlayer(NetworkRunner runner, PlayerRef playerref)
    {
        if (GameManager.playState != GameManager.PlayState.LOBBY)
        {
            Debug.Log("Not Spawning Player - game has already started");
            return;
        }

        Debug.Log($"Spawning character for player {playerref}");
        runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, playerref, InitNetworkState);
        
        void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
        {
            var player = networkObject.gameObject.GetComponent<Player>();
            Debug.Log($"Initializing player {_username.text}");
            player.InitNetworkState();
        }
    }

    private void OnDespawnPlayer(NetworkRunner runner, PlayerRef playerref)
    {
        Debug.Log($"Despawning Player {playerref}");
        Sapphire.Player player = PlayerManager.Get(playerref);
        player.TriggerDespawn();
    }
}