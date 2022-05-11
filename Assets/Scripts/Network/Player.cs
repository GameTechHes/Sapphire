using System;
using System.Collections.Generic;
using Fusion;
using Sapphire;
using UnityEngine;
using UserInterface;

namespace Sapphire
{
    public enum PlayerType
    {
        KNIGHT,
        WIZARD
    }

    public class Player : NetworkBehaviour
    {
        public const byte MAX_HEALTH = 100;

        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GameObject[] objectsToDisable;

        [Networked] private int Health { get; set; }
        [Networked] public string Username { get; set; }
        [Networked] public PlayerType PlayerType { get; set; }

        private ThirdPersonController _controller;
        private float _respawnInSeconds = -1;

        public static Action<Player> PlayerJoined;
        public static Action<Player> PlayerLeft;

        public int playerID { get; private set; }

        public static readonly List<Player> Players = new List<Player>();
        public static Player Local;

        public void InitNetworkState(PlayerType type)
        {
            Health = MAX_HEALTH;
            PlayerType = type;
        }

        public override void Spawned()
        {
            healthBar.SetMaxHealth(MAX_HEALTH);
            healthBar.SetProgress(Health);

            playerID = Object.InputAuthority;
            if (Object.HasInputAuthority)
            {
                Local = this;
                RPC_SetPlayerStats(ClientInfo.Username);
            }

            _controller = GetComponentInChildren<ThirdPersonController>();

            Players.Add(this);
            PlayerJoined?.Invoke(this);

            if (!Object.HasInputAuthority)
            {
                foreach (var obj in objectsToDisable)
                {
                    if (obj.TryGetComponent<Camera>(out var camera))
                    {
                        camera.enabled = false;
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                // Cursor.lockState = CursorLockMode.Locked;
            }

            DontDestroyOnLoad(gameObject);

            Debug.Log("Spawned [" + this + "] type=" + (PlayerType == PlayerType.KNIGHT
                          ? "Knight"
                          : "Wizard") + " IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer +
                  " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
        private void RPC_SetPlayerStats(string username)
        {
            Username = username;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
            {
                if (_respawnInSeconds >= 0)
                    CheckRespawn();
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            PlayerManager.RemovePlayer(this);
        }

        public void CheckRespawn()
        {
            if (_respawnInSeconds > 0)
                _respawnInSeconds -= Runner.DeltaTime;

            if (_respawnInSeconds <= 0)
            {
                Debug.Log("Player respawned");
                _controller.GetComponent<NetworkTransform>().transform.position = new Vector3(0.0f, 100.0f, 0.0f);
                _respawnInSeconds = -1;
            }
        }

        public void Respawn(float inSeconds)
        {
            _respawnInSeconds = inSeconds;
        }

        public void TriggerDespawn()
        {
            PlayerManager.RemovePlayer(this);

            if (Object == null)
            {
                return;
            }

            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}