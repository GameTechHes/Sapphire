using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private LevelManager _levelManager;
    public static GameManager instance { get; private set; }
    
    public enum PlayState
    {
        LOBBY,
        LEVEL,
        TRANSITION
    }
    
    [Networked]
    private PlayState networkedPlayState { get; set; }
    
    public static PlayState playState
    {
        get => (instance != null && instance.Object != null && instance.Object.IsValid) ?  instance.networkedPlayState : PlayState.LOBBY;
        set
        {
            if (instance != null && instance.Object != null && instance.Object.IsValid)
                instance.networkedPlayState = value;
        }
    }

    public override void Spawned()
    {
        instance = this;
        _levelManager = FindObjectOfType<LevelManager>(true);
    }
}
