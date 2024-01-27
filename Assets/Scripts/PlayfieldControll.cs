using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayfieldControll : MonoBehaviour
{
    OverlordJudging overlordJudging;

    public bool allPlayersReady = false;

    [Header("ROUNDS")]
    public int totalRounds = 10;
    public int currentRound = 0;

    [Header("PLAYERS")]
    PlayerManager players;

    [Header("STUFF")]
    public GameObject overlord;
    public GameObject counter;
    public GameObject playerManager;

    private void Start()
    {
        overlordJudging = overlord.GetComponent<OverlordJudging>();
        players = playerManager.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        //Start new round on screen when all player-input is received
        if (allPlayersReady == true)
        {
            //Block further input

            HandleRound();
        }
    }

    private void HandleRound()
    {
        overlordJudging.OverlordJugdgeNow();
        StartNewRound();
    }

    private void StartNewRound()
    {
        if (currentRound == totalRounds - 1)
        {
            Debug.Log("End of Game");
        }
        else
        {
            allPlayersReady = false;

            //Set Round
            currentRound = currentRound + 1;
            counter.GetComponent<TextMeshProUGUI>().text = currentRound.ToString();

            // Refill player hands according to _some_ rule
            players.RestockHands(currentRound);
        }
    }
}