using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Sapphire;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkSceneManagerBase
{
    public NetworkManager NetworkManager { get; set; }

    private Scene _loadedScene;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadLevel(int nextLevelIndex)
    {
        Runner.SetActiveScene(nextLevelIndex);
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        Debug.Log($"Switching Scene from {prevScene} to {newScene}");
        if (newScene <= 0)
        {
            finished(new List<NetworkObject>());
            yield break;
        }

        if (prevScene == 1 && newScene == 1)
        {
            yield return SceneManager.UnloadSceneAsync(newScene);
        }

        List<NetworkObject> sceneObjects = new List<NetworkObject>();
        if (newScene >= 1)
        {
            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
            _loadedScene = SceneManager.GetSceneByBuildIndex(newScene);
            Debug.Log($"Loaded scene {newScene}: {_loadedScene}");
            sceneObjects = FindNetworkObjects(_loadedScene, disable: false);
        }

        Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");
        finished(sceneObjects);

        // Delay one frame, so we're sure level objects has spawned locally
        yield return null;

        NetworkManager.SetConnectionStatus(NetworkManager.ConnectionStatus.Loaded, "");
        
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return null;
        
        foreach (var player in Player.Players)
        {
            print($"Respawning player: {player}");
            player.Respawn(0);
            yield return new WaitForSeconds(0.3f);
        }
    }
}