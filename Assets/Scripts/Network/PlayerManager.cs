using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static List<Network.Player> _allPlayers = new List<Network.Player>();
    public static List<Network.Player> allPlayers => _allPlayers;
    private static Queue<Network.Player> _playerQueue = new Queue<Network.Player>();

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

    public static void AddPlayer(Network.Player player)
    {
        Debug.Log("Player Added");

        int insertIndex = _allPlayers.Count;
        // Sort the player list when adding players
        for (int i = 0; i < _allPlayers.Count; i++)
        {
            if (_allPlayers[i].playerID > player.playerID)
            {
                insertIndex = i;
                break;
            }
        }

        _allPlayers.Insert(insertIndex, player);
        _playerQueue.Enqueue(player);
    }

    public static void RemovePlayer(Network.Player player)
    {
        if (player == null || !_allPlayers.Contains(player))
            return;

        Debug.Log("Player Removed " + player.playerID);

        _allPlayers.Remove(player);
    }
}