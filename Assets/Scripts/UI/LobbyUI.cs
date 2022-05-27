using Sapphire;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{

    private Player _wizardPlayer;
    private Player _knightPlayer;

    private void Start()
    {
        Player.PlayerJoined += AddPlayer;
    }

    public void AddPlayer(Player player)
    {
        if (player.GetType() == typeof(Wizard))
        {
            _wizardPlayer = player;
        }
        else
        {
            _knightPlayer = player;
        }
    }

    private void Update()
    {
        if (_wizardPlayer != null && _wizardPlayer.Object != null && _wizardPlayer.Object.IsValid)
        {
            UIManager.Instance.WizardText.text = $"Wizard: {_wizardPlayer.Username}";
            UIManager.Instance.WizardReady.text = _wizardPlayer.IsReady ? "Ready" : "Not ready";
        }
        
        if (_knightPlayer != null && _knightPlayer.Object != null && _knightPlayer.Object.IsValid)
        {
            UIManager.Instance.KnightText.text = $"Knight: {_knightPlayer.Username}";
            UIManager.Instance.KnightReady.text = _knightPlayer.IsReady ? "Ready" : "Not ready";
        }

        if (GameManager.playState == GameManager.PlayState.INGAME)
        {
            UIManager.Instance.HideLobbyUI();
        }
    }
}