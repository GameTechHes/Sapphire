// using Fusion;
// using FusionExamples.FusionHelpers;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class TitleScreen : MonoBehaviour, INetworkRunnerCallbacks
// {
//     [SerializeField] private NetworkPrefabRef _KnightPrefab;
//     [SerializeField] private NetworkPrefabRef _WizardPrefab;
//     [SerializeField] private TMP_InputField _room;
//
//     private GameMode _gameMode;
//
//     private void Awake()
//     {
//         DontDestroyOnLoad(this);
//     }
//
//     public void Create()
//     {
//         _gameMode = GameMode.Host;
//     }
//
//     public void Join()
//     {
//         _gameMode = GameMode.Client;
//     }
//
//     public void Quit()
//     {
//         Application.Quit();
//     }
//
//     public void LaunchGame()
//     {
//         NetworkRunner runner = FindObjectOfType<NetworkRunner>();
//         if (runner != null && !runner.IsShutdown)
//         {
//             // Calling with destroyGameObject false because we do this in the OnShutdown callback on FusionLauncher
//             runner.Shutdown(false);
//         }
//
//         FusionLauncher launcher = FindObjectOfType<FusionLauncher>();
//         if (launcher == null)
//             launcher = new GameObject("Launcher").AddComponent<FusionLauncher>();
//
//         NetworkSceneManagerDefault lm = gameObject.AddComponent<NetworkSceneManagerDefault>();
//
//         launcher.Launch(_gameMode, _room.text, lm, OnConnectionStatusUpdate, OnSpawnWorld, OnSpawnPlayer,
//             OnDespawnPlayer);
//
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }
//
//     private void OnConnectionStatusUpdate(NetworkRunner runner, FusionLauncher.ConnectionStatus status, string reason)
//     {
//         if (!this)
//             return;
//
//         Debug.Log(status);
//     }
//
//     private void OnSpawnWorld(NetworkRunner runner)
//     {
//         Debug.Log("Spawning GameManager");
//         //runner.Spawn(_KnightPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
//
//         void InitNetworkState(NetworkRunner runner, NetworkObject world)
//         {
//             world.transform.parent = transform;
//         }
//     }
//
//     private void OnSpawnPlayer(NetworkRunner runner, PlayerRef playerref)
//     {
//         Debug.Log($"Spawning tank for player {playerref}");
//
//         Vector3 vPos = new Vector3(10, 2, 5);
//
//         //GameObject Menu = GameObject.Find("Canvas");
//         //Menu.SetActive(false);
//
//         if (playerref == 9)
//         {
//             runner.Spawn(_KnightPrefab, vPos, Quaternion.identity, playerref);
//         }
//         else
//         {
//             runner.Spawn(_WizardPrefab, vPos, Quaternion.identity, playerref);
//             //GameObject Menu = GameObject.Find("Canvas");
//             //Menu.SetActive(false);
//         }
//     }
//
//     private void OnDespawnPlayer(NetworkRunner runner, PlayerRef playerref)
//     {
//         Debug.Log($"Despawning Player {playerref}");
//     }
// }