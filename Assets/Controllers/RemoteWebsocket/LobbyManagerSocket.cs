using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManagerSocket : MonoBehaviour
{
  public static LobbyManagerSocket instance;

    // event to notify that players are updated
    public delegate void UpdatePlayers(List<LobbyPlayer> players);
    
    public event UpdatePlayers updatePlayersEvent;

    public string id;
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            id = Lobby.getRoomNumber();
        }
        else
        {
            Debug.LogError("Multiple LobbyManager instances detected.");
        }
    }

    public void Start()
    {
        WebSocketBridge.instance.messageReceivedEvent += OnMessageReceived;        
    }


    public void OnMessageReceived(string topic, string message)
    {
        if ("playerJoined".Equals(topic))
        {
            PlayerJoinedStruct playersJoined = JsonUtility.FromJson<PlayerJoinedStruct>(message);
            
            List<LobbyPlayer> players = new List<LobbyPlayer>(playersJoined.players);
            _updatePlayers(players);
        }
    }
    
    [Serializable]
    struct PlayerJoinedStruct
    {
        public LobbyPlayer[] players;
    }
    
    private void _updatePlayers(List<LobbyPlayer> players)
    {
        Lobby.players = players;

        Debug.Log("updating players:" + players);

        updatePlayersEvent?.Invoke(players);
    }
}
