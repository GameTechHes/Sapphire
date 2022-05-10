using System;
using System.Collections.Generic;
using Fusion;

public class LobbyPlayer : NetworkBehaviour
{
    [Networked] public string Username { get; set; }
    
    public static LobbyPlayer Local;
    public static readonly List<LobbyPlayer> Players = new List<LobbyPlayer>();
    
    public static Action<LobbyPlayer> PlayerJoined;
    public static Action<LobbyPlayer> PlayerLeft;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            RPC_SetPlayerStats(ClientInfo.Username);
        }

        Players.Add(this);
        PlayerJoined?.Invoke(this);
        
        DontDestroyOnLoad(gameObject);
    }
    
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
    private void RPC_SetPlayerStats(string username)
    {
        Username = username;
    }
}
