using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private RectTransform _scrollContent;
    [SerializeField] private LobbyItem _lobbyItemPrefab;

    private void Start()
    {
        LobbyPlayer.PlayerJoined += AddPlayer;
    }

    private void AddPlayer(LobbyPlayer player)
    {
        var obj = Instantiate(_lobbyItemPrefab, _scrollContent);
        obj.SetPlayer(player);
    }
}
