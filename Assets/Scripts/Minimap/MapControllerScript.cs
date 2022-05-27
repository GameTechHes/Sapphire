using System.Collections;
using Sapphire;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minimap
{
    public class MapControllerScript : MonoBehaviour
    {
        private StarterAssetsInputs _starterAssetsInputs;
        private PlayerInput _playerInput;
        private AudioManager _audioManager;

        private bool _displayMap;
        private bool _canToggle = true;
        private const float ToggleTime = 0.2f;
        private const float MinSize = 15;
        private const float MaxSize = 200;

        private void Awake()
        {
            _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
            _playerInput = FindObjectOfType<PlayerInput>();
            _audioManager = FindObjectOfType<AudioManager>();
        }

        void Start()
        {
            UIManager.Instance.MinimapDisplay(false);
            ResumeGame();
        }

        void Update()
        {
            if ((_starterAssetsInputs.resumeGame || _starterAssetsInputs.displayMap) && _canToggle && Player.Local.GetType() == typeof(Wizard))
            {
                _starterAssetsInputs.displayMap = false;
                StartCoroutine(ToggleMiniMap());
            }
        }

        private void ResumeGame()
        {
            if (_starterAssetsInputs.resumeGame)
            {
                _starterAssetsInputs.resumeGame = false;
                _playerInput.actions.FindActionMap("Player").Enable();
                _playerInput.actions.FindActionMap("UI").Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private IEnumerator ToggleMiniMap()
        {
            _displayMap = !_displayMap;
            UIManager.Instance.MinimapDisplay(_displayMap); 

            
            if (_displayMap)
            {
                _audioManager.Play("Opening_map");
                _starterAssetsInputs.StopPlayerMovement();
                _playerInput.actions.FindActionMap("Player").Disable();
                _playerInput.actions.FindActionMap("UI").Enable();
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