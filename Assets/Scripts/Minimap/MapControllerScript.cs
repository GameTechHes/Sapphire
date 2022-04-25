using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minimap
{
    public class MapControllerScript : MonoBehaviour
    {
        public Camera minicam;
        private StarterAssetsInputs starterAssetsInputs;
        public PlayerInput playerInput;
        public GameObject minicamUI;

        private bool _displayMap = false;
        private bool _canToggle = true;
        private const float ToggleTime = 0.2f;
        private const float MinSize = 15;
        private const float MaxSize = 200;

        void Start()
        {
            minicamUI.SetActive(_displayMap);
            starterAssetsInputs = GameObject.Find("InputManager").GetComponent<StarterAssetsInputs>();
            ResumeGame();
        }

        void Update()
        {
            if ((starterAssetsInputs.resumeGame || starterAssetsInputs.displayMap) && _canToggle)
            {
                starterAssetsInputs.displayMap = false;
                StartCoroutine(ToggleMiniMap());
            }
        }

        private void ResumeGame()
        {
            if (starterAssetsInputs.resumeGame)
            {
                starterAssetsInputs.resumeGame = false;
                playerInput.actions.FindActionMap("Player").Enable();
                playerInput.actions.FindActionMap("UI").Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private IEnumerator ToggleMiniMap()
        {
            _displayMap = !_displayMap;
            minicamUI.SetActive(_displayMap);
            if (_displayMap)
            {
                starterAssetsInputs.StopPlayerMovement();
                playerInput.actions.FindActionMap("Player").Disable();
                playerInput.actions.FindActionMap("UI").Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                ResumeGame();
            }

            _canToggle = false;
            yield return new WaitForSeconds(ToggleTime);
            _canToggle = true;
        }
    }
}