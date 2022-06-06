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
            Player.Local._uiManager.WizardText.text = $"Wizard: {_wizardPlayer.Username}";
            Player.Local._uiManager.WizardReady.text = _wizardPlayer.IsReady ? "Ready" : "Not ready";
        }
        
        if (_knightPlayer != null && _knightPlayer.Object != null && _knightPlayer.Object.IsValid)
        {
            Player.Local._uiManager.KnightText.text = $"Knight: {_knightPlayer.Username}";
            Player.Local._uiManager.KnightReady.text = _knightPlayer.IsReady ? "Ready" : "Not ready";
        }
    }
}