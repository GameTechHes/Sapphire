using Sapphire;
using TMPro;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text isReady;

    private Player _player;

    private void Start()
    {
        isReady.text = "Not Ready";
    }

    private void Update()
    {
        if (_player.Object != null && _player.Object.IsValid)
        {
            username.text = _player.Username;
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }
}