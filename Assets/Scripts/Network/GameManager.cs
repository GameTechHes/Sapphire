using Fusion;
using Sapphire;

public class GameManager : NetworkBehaviour
{
    private LevelManager _levelManager;
    public static GameManager instance { get; private set; }

    [Networked] private int _wizard { get; set; }
    [Networked] private int _knight { get; set; }

    public enum PlayState
    {
        LOBBY,
        INGAME,
    }

    [Networked] private PlayState networkedPlayState { get; set; }

    public static PlayState playState
    {
        get => (instance != null && instance.Object != null && instance.Object.IsValid)
            ? instance.networkedPlayState
            : PlayState.LOBBY;
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
        if (Object.HasStateAuthority)
        {
            _levelManager.LoadLevel(1);
        }
    }

    public PlayerType AssignRole(PlayerRef playerRef)
    {
        if (_wizard == 0)
        {
            _wizard = playerRef;
            return PlayerType.WIZARD;
        }

        _knight = playerRef;
        return PlayerType.KNIGHT;
    }
}