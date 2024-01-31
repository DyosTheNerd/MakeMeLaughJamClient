using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoopController : MonoBehaviour
{
    public static GameLoopController instance;

    // References
    OverlordJudging overlordJudging;
    Cardholder cardHolder;

    //public bool allPlayersReady = false;

    [Header("ROUNDS")]
    public int totalRounds = 10;
    public int currentRound = 0;
    public float timeOut = 30.0f;
    public bool timedOut = false;

    [Header("PLAYERS")]
    PlayerManager players;

    [Header("STUFF")]
    public GameObject overlord;
    public GameObject cards;
    public GameObject counter;
    public GameObject playerManager;



    void Awake()
    {
        if (instance == null)
        {
            GameLoopController.instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameLoopController instances detected.");
        }
    }

    IEnumerator TimeOut(float seconds)
    {
        float waitTime = seconds;
        bool playersRedied = false;
        while(waitTime > 0)
        {
            //Debug.Log(waitTime);
            playersRedied = players.ArePlayersReady();
            if (playersRedied) break;
            waitTime -= 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        if(!playersRedied)
            timedOut = true;
    }

    IEnumerator RoundsCoroutine(int rounds)
    {

        for (int i = 0; i < rounds; i++)
        {
            StartNewRound();

            timedOut = false;
            StartCoroutine(TimeOut(timeOut));
            while (players.ArePlayersReady() != true && !timedOut)
                yield return new WaitForSeconds(3);

            cardHolder.SetCardUI();

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
        Debug.Log(cardHolder);
        players = playerManager.GetComponent<PlayerManager>();

        overlordJudging.SatisfiedOverlord += WinEnding;
        overlordJudging.DissatisfiedOverlord += LoseEnding;

    }

    public void StartGame()
    {
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
            //cardHolder.SetCardUI();

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