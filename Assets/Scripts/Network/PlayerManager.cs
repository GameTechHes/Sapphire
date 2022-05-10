using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static List<Network.Player> _allPlayers = new List<Network.Player>();
    public static List<Network.Player> allPlayers => _allPlayers;

    public static Network.Player Get(PlayerRef playerRef)
    {
        for (int i = _allPlayers.Count - 1; i >= 0; i--)
        {
            if (_allPlayers[i] == null || _allPlayers[i].Object == null)
            {
                _allPlayers.RemoveAt(i);
                Debug.Log("Removing null player");
            }
            else if (_allPlayers[i].Object.InputAuthority == playerRef)
                return _allPlayers[i];
        }

        return null;
    }

    public static void RemovePlayer(Network.Player player)
    {
        if (player == null || !_allPlayers.Contains(player))
            return;

        Debug.Log("Player Removed " + player.playerID);

        _allPlayers.Remove(player);
    }
}