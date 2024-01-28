using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfieldControll : MonoBehaviour
{
    OverlordJudging overlordJudging;
    Cardholder cardHolder;

    //public bool allPlayersReady = false;

    [Header("ROUNDS")]
    public int totalRounds = 10;
    public int currentRound = 0;

    [Header("PLAYERS")]
    PlayerManager players;

    [Header("STUFF")]
    public GameObject overlord;
    public GameObject cards;
    public GameObject counter;
    public GameObject playerManager;


    IEnumerator RoundsCoroutine(int rounds)
    {

        for (int i = 0; i < rounds; i++)
        {
            StartNewRound();

            while (players.ArePlayersReady() != true)
                yield return new WaitForSeconds(3);

            overlordJudging.OverlordJugdgeNow();

            //TODO Add overlord animation wait time thing;
            yield return new WaitForSeconds(5);
                //new WaitUntil( something something animation complete )
        }

    }

    private void Start()
    {
        overlordJudging = overlord.GetComponent<OverlordJudging>();
        cardHolder = cards.GetComponent<Cardholder>();
        players = playerManager.GetComponent<PlayerManager>();

        overlordJudging.SatisfiedOverlord += WinEnding;
        overlordJudging.DissatisfiedOverlord += LoseEnding;

        StartCoroutine(RoundsCoroutine(totalRounds));
    }

    private void StartNewRound()
    {
        if (currentRound == totalRounds - 1)
        {
            Debug.Log("End of Game");
            // Here the Game Ends on an impasse
            OutOfRoundsEnding();
        }
        else
        {
            //Set Round
            currentRound = currentRound + 1;
            counter.GetComponent<TextMeshProUGUI>().text = currentRound.ToString();

            // TODO update round UI here.
            cardHolder.SetCardUI();

            // Refill player hands according to _some_ rule
            players.RestockHands(currentRound);
            // sets are players ready to false
            //players.UnreadyPlayers();
        }
    }
    private void OutOfRoundsEnding()
    {
        int overlordMood = overlord.GetComponent<OverlordJudging>().overlordMood;

        if (overlordMood < 25)
        {
            Debug.Log("That was not good! Not at All!");
        }
        else if (overlordMood > 25 && overlordMood < 75)
        {
            Debug.Log("There will be another year for humanity, perhaps.");
        }
        else
        {
            Debug.Log("That was entertaining. I think I'll let the earth live....for now.");
        }
        // TODO add a coroutine to delay the scene transition
        SceneManager.LoadScene("WinSomeLoseSomeScreen");
    }

    private void WinEnding()
    {
        // TODO add a coroutine to delay the scene transition
        SceneManager.LoadScene("WinScreen");
    }

    private void LoseEnding()
    {
        // TODO add a coroutine to delay the scene transition
        SceneManager.LoadScene("LoseScreen");
    }

}