using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Remote;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine.SceneManagement;


public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    // event to notify that players are updated
    public delegate void UpdatePlayers(List<LobbyPlayer> players);
    
    public event UpdatePlayers updatePlayersEvent;
    
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple LobbyManager instances detected.");
        }
    }

    public void Start()
    {
        InitializeRemoteGame();
        StartCoroutine(PollLobby());
    }

    IEnumerator PollLobby()
    {
        while (true)
        {
            try
            {
                RestClient.Get<LobbyApiStruct>(
                        $"{RemoteConfig.SbaseProjectUrl}/{Lobby.getRoomNumber()}/players")
                    .Then(response =>
                    {

                        Debug.Log($"code = {response.documents.Length}");

                        List<LobbyPlayer> players = new List<LobbyPlayer>();
                        
                        foreach (var doc in response.documents)
                        {
                            LobbyPlayer player = new LobbyPlayer()
                            {
                                id =doc.fields.playerId.stringValue,
                                name=  doc.fields.name.stringValue
                            };
                            
                            players.Add(player);
                        }

                        _updatePlayers(players);
                    })
                    .Catch(reject => { Debug.Log(reject.Message); });
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            yield return new WaitForSeconds(5);
        }   
    }

    // calls the remote API to initialize the game
    public void InitializeRemoteGame()
    {
        string gameId = Lobby.getRoomNumber();
        GameStruct gameStruct = new GameStruct();
        gameStruct.id = gameId;
        gameStruct.code = gameId;

        GameApiStruct gameApiStruct = gameStruct.ToGameApiStruct();

        RestClient.Patch<GameApiStruct>(
                $"{RemoteConfig.SbaseProjectUrl}/{gameId}",
                gameApiStruct)
            .Then(response => { Debug.Log($"code = {response}"); }).Catch(reject => { Debug.Log(reject.Message); });   
    }
    
    private void _updatePlayers(List<LobbyPlayer> players)
    {
        Lobby.players = players;

        Debug.Log("updating players");

        updatePlayersEvent?.Invoke(players);
    }
}