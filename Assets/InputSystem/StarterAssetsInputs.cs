using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
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

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
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
    }
}