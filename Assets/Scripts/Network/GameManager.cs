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
    public static GameManager instance { get; private set; }

    [Networked] public int sbireNumber { get; set; }

    [SerializeField] private NetworkObject botPrefab;

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
            _levelManager.LoadLevel(2);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            sbireNumber = FindObjectsOfType<FieldOfView>().Where(b => !b.IsDead).ToArray().Length;
        }
    }

    public void SpawnSbire(Vector3 position, Quaternion rotation)
    {
        if (Object.HasStateAuthority)
            Runner.Spawn(botPrefab, position, rotation);
    }


    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        if(!Object.HasStateAuthority)
            return;
        
        var count = Player.Players.Count;
        if (count == 0)
        {
            runner.Spawn(_playerKnightPrefab, Vector3.zero, Quaternion.identity, playerRef);
        } else if (count == 1)
        {
            runner.Spawn(_playerWizardPrefab, Vector3.zero, Quaternion.identity, playerRef);
        }
        else
        {
            Debug.LogError("Already 2 players in game");
        }
    }
}