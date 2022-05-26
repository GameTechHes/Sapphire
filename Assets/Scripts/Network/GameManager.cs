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

    [Networked(OnChanged = nameof(OnSbireNumberChange))]
    public int sbireNumber { get; set; }

    [SerializeField] private NetworkObject botPrefab;

    private PlayerRef _knightPlayer;
    private Player _wizardPlayer;

    public enum PlayState
    {
        LOBBY,
        STARTING,
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

    private void OnSbireNumberChange()
    {
        Player.Local._uiManager.SbireCounter.text = sbireNumber.ToString();
    }

    public static void OnSbireNumberChange(Changed<GameManager> changed)
    {
        changed.Behaviour.OnSbireNumberChange();
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

            if (playState == PlayState.LOBBY && Player.Players.Count == 2 && Player.Players.All(p => p.IsReady))
            {
                FindObjectOfType<TimerManager>().StartTimer();
                playState = PlayState.STARTING;
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SpawnSbire(Vector3 position, Quaternion rotation)
    {
        Runner.Spawn(botPrefab, position, rotation);
    }


    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        if (!Object.HasStateAuthority)
            return;

        var count = Player.Players.Count;
        if (count == 0)
        {
            runner.Spawn(_playerKnightPrefab, Vector3.zero, Quaternion.identity, playerRef);
            _knightPlayer = playerRef;
        }
        else if (count == 1)
        {
            _wizardPlayer = runner.Spawn(_playerWizardPrefab, Vector3.zero, Quaternion.identity, playerRef);
        }
        else
        {
            Debug.LogError("Already 2 players in game");
        }
    }

    public void StartGame()
    {
        if (Object.HasStateAuthority && playState == PlayState.STARTING && _knightPlayer)
        {
            playState = PlayState.INGAME;
            var knight = Player.Get(_knightPlayer);
            var cc = knight.GetComponent<CharacterController>();
            var spawnpt = GameObject.Find("InitialKnightSpawnPoint");
            if (cc != null && spawnpt != null)
            {
                cc.enabled = false;
                knight.transform.position = spawnpt.transform.position;
                cc.enabled = true;
            }

            FindObjectOfType<TimerManager>().StartGameTimer();
        }
    }
}