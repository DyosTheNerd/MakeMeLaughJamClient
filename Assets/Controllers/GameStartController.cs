using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finishes initialization of all the interconnected Controllers and Managers
/// E.G. Moves players from Lobby to Game.
///      Initializes the GameLoop
/// </summary>
public class GameStartController : MonoBehaviour
{
    public static GameStartController instance;
    public enum PlayerMode
    {
        REMOTE,
        DUMMY_SYSTEM
    };

    [Header("PARAMETERS")]
    public PlayerMode mode = PlayerMode.REMOTE;


    void Awake()
    {
        if (instance == null)
        {
            GameStartController.instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameStartController instances detected.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Starts the remote or simulated players
        if(mode == PlayerMode.REMOTE)
        {
            foreach (var player in Controllers.Lobby.players)
            {
                PlayerManager.instance.AddPlayer(player.id, player.name);
            }
        }
        else
        {
            DummyPlayerSystem.instance.InitializeDummyPlayers();
        }


        GameLoopController.instance.StartGame();
    }
}
