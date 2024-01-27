using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayfieldControll : MonoBehaviour
{
    OverlordJudging overlordJudging;

    //public bool allPlayersReady = false;

    [Header("ROUNDS")]
    public int totalRounds = 10;
    public int currentRound = 0;

    [Header("PLAYERS")]
    PlayerManager players;

    [Header("STUFF")]
    public GameObject overlord;
    public GameObject counter;
    public GameObject playerManager;


    IEnumerator RoundsCoroutine(int rounds)
    {

        for (int i = 0; i < rounds; i++)
        {
            StartNewRound();

            while (players.ArePlayersReady() != true)
                yield return new WaitForSeconds(5);

            overlordJudging.OverlordJugdgeNow();
            
            //TODO Add overlord animation wait time thing;
            //yield return new WaitUntil( something something animation complete )
        }

    }

    private void Start()
    {
        overlordJudging = overlord.GetComponent<OverlordJudging>();
        players = playerManager.GetComponent<PlayerManager>();

        StartCoroutine(RoundsCoroutine(totalRounds));
    }

    private void StartNewRound()
    {
        if (currentRound == totalRounds - 1)
        {
            Debug.Log("End of Game");
        }
        else
        {
            //Set Round
            currentRound = currentRound + 1;
            counter.GetComponent<TextMeshProUGUI>().text = currentRound.ToString();

            // TODO update round UI here.

            // Refill player hands according to _some_ rule
            players.RestockHands(currentRound);
            // sets are players ready to false
            players.UnreadyPlayers();
        }
    }
}