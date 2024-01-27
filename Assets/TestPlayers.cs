using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayers : MonoBehaviour
{
    PlayerManager players;

    string[] playerNames = { "Apple", "Banana", "Orange", "Mango", "Grape", "Pineapple", "Strawberry", "Watermelon", "Kiwi", "Blueberry" };
    int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        players = FindAnyObjectByType<PlayerManager>();
    }

    // Update is called once per frame
    public void AddPlayer()
    {
        players.AddPlayer(playerNames[playerCount], playerNames[playerCount]);
        playerCount++;
    }

    public void ReadyPlayer()
    {
        players.ReadyPlayer(playerNames[Random.Range(0, 9)]);
    }

    public void UnreadyPlayers()
    {
        players.UnreadyPlayers();
    }

    public void PlayRandomCard()
    {
        string chosenPlayers = players.waitingForPlayers[Random.Range(0, players.waitingForPlayers.Count)];


    }

}
