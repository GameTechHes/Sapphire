using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Player;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : NetworkBehaviour, INetworkRunnerCallbacks
    {
        [Header("Character Input Values")] public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;
        public bool aim;
        public bool resumeGame;

        [Header("UI Input Values")] public bool displayMap;

        [Header("Movement Settings")] public bool analogMovement;

        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private NetworkInputData _frameworkInput;
        private ThirdPersonController _tpc;

        public override void Spawned()
        {
            _tpc = GetComponent<ThirdPersonController>();
            if (Object.HasInputAuthority)
            {
                Runner.AddCallbacks(this);
            }

            Debug.Log("Spawned [" + this + "] IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer +
                      " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (_tpc != null)
            {
                _frameworkInput.move = move;
                _frameworkInput.look = look;
                if (jump)
                {
                    _frameworkInput.jump = jump;
                    jump = false;
                }

                if (shoot)
                {
                    _frameworkInput.shoot = shoot;
                    shoot = false;
                }

                _frameworkInput.sprint = sprint;
                _frameworkInput.aim = aim;
                input.Set(_frameworkInput);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                _tpc.JumpAndGravity(input);
                _tpc.GroundedCheck();
                _tpc.Move(input);
            }
            else
            {
                _tpc.JumpAndGravity();
                _tpc.GroundedCheck();
                _tpc.Move();
            }
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnResumeGame(InputValue value)
        {
            ResumeGameInput(value.isPressed);
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnDisplayMap(InputValue value)
        {
            DisplayMapInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            aim = value.isPressed;
        }

        public void OnEscape(InputValue value)
        {
            if (value.isPressed)
            {
                SetCursorState(false);
            }
        }

        public void StopPlayerMovement()
        {
            move = Vector2.zero;
            look = Vector2.zero;
            jump = false;
            shoot = false;
            sprint = false;
        }

        public void DisplayMapInput(bool newDisplayMapState)
        {
            displayMap = newDisplayMapState;
        }

        public void ResumeGameInput(bool newResumeGameState)
        {
            resumeGame = newResumeGameState;
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void ShootInput(bool newShootInput)
        {
            shoot = newShootInput;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
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

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
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

    public struct NetworkInputData : INetworkInput
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;
        public bool aim;
    }
}