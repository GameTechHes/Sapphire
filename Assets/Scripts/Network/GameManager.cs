using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sapphire;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Player _playerKnightPrefab;
    [SerializeField] private Player _playerWizardPrefab;

    private LevelManager _levelManager;
    private NetworkRunner _runner;
    public static GameManager instance { get; private set; }

    [Networked] private PlayerRef _wizard { get; set; }
    [Networked] private PlayerRef _knight { get; set; }

    public Queue<PlayerRef> PlayerQueue = new Queue<PlayerRef>();

    public enum PlayState
    {
        LOBBY,
        INGAME,
    }
    [Networked] private PlayState _networkedPlayState { get; set; }

    public static PlayState playState
    {
        get => (instance != null && instance.Object != null && instance.Object.IsValid)
            ? instance._networkedPlayState
            : PlayState.LOBBY;
        set
        {
            if (instance != null && instance.Object != null && instance.Object.IsValid)
                instance._networkedPlayState = value;
        }
    }

    public override void Spawned()
    {
        instance = this;

        if (Object.HasStateAuthority)
        {
            _levelManager = FindObjectOfType<LevelManager>(true);
            _runner = FindObjectOfType<NetworkRunner>();
            _levelManager.LoadLevel(2);
        }
        
        print("Player count: " + Player.Players.Count);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && Player.Players.Count == 2 && Player.Players.All(p => p.IsReady))
        {
            _levelManager.LoadLevel(2);
        }
    }

    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        GameObject LobbySpawnPoint = GameObject.Find("Lobby/KnightSpawnPoint");
        if(LobbySpawnPoint != null){
            Debug.Log("Spawned player on :" + LobbySpawnPoint.transform.position.ToString());
        runner.Spawn(_playerKnightPrefab, LobbySpawnPoint.transform.position, LobbySpawnPoint.transform.rotation, playerRef);

        }
        runner.Spawn(_playerKnightPrefab, Vector3.zero, Quaternion.identity, playerRef);
    }
}