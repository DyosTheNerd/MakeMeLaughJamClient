using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldControll : MonoBehaviour
{
    OverlordJudging overlordJudging;

    bool allPlayersReady = false;

    [Header("ROUNDS")]
    public int totalRounds = 10;
    public int currentRound = 0;

    [Header("PLAYERS")]
    PlayerManager players;

    [Header("ROUND CARDS")]
    public List<card> PlayedCards = new List<card>();

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
        //Request Status of player-input

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
            counter.GetComponent<TextMesh>().text = currentRound.ToString();

            // Refill player hands according to _some_ rule
            players.RestockHands(currentRound);

            //Set new Judging (SHOULD IT CHANGE EVERY ROUND?)
            //overlordJudging.BuildJudgeRules();
        }
    }
}