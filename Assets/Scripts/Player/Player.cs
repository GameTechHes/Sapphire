using System;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sapphire
{
    public abstract class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject[] objectsToDisable;
        [SerializeField] private Camera minimapCamera;

        [Networked] public string Username { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked(OnChanged = nameof(OnHealthChange))]
        public int Health { get; set; }

        private const byte MAX_HEALTH = 100;

        private float _respawnInSeconds = -1;

        public UIManager _uiManager;

        public Transform cameraRoot;
        private CinemachineVirtualCamera _followCamera;
        protected Animator _controller;
        protected float _cameraDistance = 4.0f;
        protected Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;

        public static Action<Player> PlayerJoined;

        public static readonly List<Player> Players = new List<Player>();
        public static Player Local;

        public override void Spawned()
        {
            _uiManager = FindObjectOfType<UIManager>();
            Health = MAX_HEALTH;

            _controller = GetComponent<Animator>();

            Players.Add(this);
            PlayerJoined?.Invoke(this);

            if (Object.HasInputAuthority)
            {
                Local = this;
                RPC_SetPlayerStats(ClientInfo.Username);
                if (_uiManager != null)
                {
                    _uiManager.SetHealth((int) MAX_HEALTH, (int) MAX_HEALTH);
                    _uiManager.SetUIPosition();
                }

                _followCamera = FindObjectOfType<CinemachineVirtualCamera>();
                _followCamera.Follow = cameraRoot;
                _cinemachine3RdPersonFollow = _followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            }

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
                Cursor.lockState = CursorLockMode.Locked;
            }

            Debug.Log("Spawned [" + this + "] IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer +
                      " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
        private void RPC_SetPlayerStats(string username)
        {
            Username = username;
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
        private void RPC_PlayerReady(NetworkBool isReady)
        {
            IsReady = isReady;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_AddHealth(int health)
        {
            Health += health;
            if (Health > MAX_HEALTH)
            {
                Health = MAX_HEALTH;
            }

            if (Health < 0)
            {
                Health = 0;
            }
        }

        public static void OnHealthChange(Changed<Player> changed)
        {
            changed.Behaviour.OnHealthChange();
        }

        private void OnHealthChange()
        {
            if (Object.HasInputAuthority)
            {
                _uiManager.SetHealth(Health, MAX_HEALTH);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
            {
                if (_respawnInSeconds >= 0)
                    CheckRespawn();
            }
        }

        public override void Render()
        {
            if (Object.HasInputAuthority && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RPC_PlayerReady(!IsReady);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Players.Remove(this);
        }

        public void Respawn(float inSeconds)
        {
            _respawnInSeconds = inSeconds;
        }

        public void CheckRespawn()
        {
            if (_respawnInSeconds > 0)
                _respawnInSeconds -= Runner.DeltaTime;
        }

        public void TriggerDespawn()
        {
            Players.Remove(this);

            if (Object == null)
            {
                return;
            }

            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }

        public static Player Get(PlayerRef playerref)
        {
            foreach (var player in Players)
            {
                if (player.Object.InputAuthority == playerref)
                {
                    return player;
                }
            }

            return null;
        }

        public Camera GetMinimapCamera()
        {
            return minimapCamera;
        }
    }
}