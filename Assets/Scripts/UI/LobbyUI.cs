using Sapphire;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _knightText;
    [SerializeField] private TMP_Text _wizardText;
    [SerializeField] private TMP_Text _wizardReady;
    [SerializeField] private TMP_Text _knightReady;

    private Player _wizardPlayer;
    private Player _knightPlayer;

    private void Start()
    {
        Player.PlayerJoined += AddPlayer;
    }

    public void AddPlayer(Player player)
    {
        if (player.PlayerType == PlayerType.WIZARD)
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
            _wizardText.text = $"Wizard: {_wizardPlayer.Username}";
            _wizardReady.text = _wizardPlayer.IsReady ? "Ready" : "Not ready";
        }
        
        if (_knightPlayer != null && _knightPlayer.Object != null && _knightPlayer.Object.IsValid)
        {
            _knightText.text = $"Knight: {_knightPlayer.Username}";
            _knightReady.text = _knightPlayer.IsReady ? "Ready" : "Not ready";
        }
    }
}