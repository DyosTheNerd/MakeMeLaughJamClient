using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerSystem : MonoBehaviour
{

    public static DummyPlayerSystem instance;


    public class DummyPlayer
    {
        public enum PlayStrategy
        {
            RANDOM,
        };

        public DummyPlayer(string playerID, PlayStrategy strategy)
        {
            this.playerID = playerID;
            this.playerCards = new List<int>();
            this.strategy = strategy;
        }

        public string playerID;
        public List<int> playerCards;
        //Coroutine current_coroutine;
        public PlayStrategy strategy;

        public IEnumerator PlayACard(float delay, int round)
        {

            yield return new WaitForSeconds(delay);

            int card = playerCards[Random.Range(0, playerCards.Count)];

            InteractionManager.instance.PlayCard(card, playerID, round);
        }


        public void UpdateHand(CardInteraction[] cards)
        {
            playerCards.Clear();
            for (int i = 0; i < cards.Length; i++)
            {
                playerCards.Add(cards[i].id);
            }
        }
    }

    void Start()
    {
        InteractionManager.instance.HandUpdated += OnHandUpdated;
    }


    [Header("Parameters")]
    // How many players will be simulated
    public int playerCount = 10;
    //Delay range for simulated players to play
    public Vector2 playDelayRange = new Vector2(10.0f, 45.0f);

    [Header("Data")]
    Dictionary<string, DummyPlayer> dummyPlayers;

    private void Awake()
    {
        if (instance == null)
        {
            DummyPlayerSystem.instance = this;
        }
        else
        {
            Debug.LogError("Multiple DummyPlayerSystem instances detected.");
        }
        dummyPlayers = new Dictionary<string, DummyPlayer>();
    }


    public void InitializeDummyPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            string idname = i.ToString();
            PlayerManager.instance.AddPlayer(idname, idname);
        }
    }

    public void OnHandUpdated(string playerId, CardInteraction[] cards, int roundNumber)
    {

        if (GameLoopController.instance.currentRound != roundNumber) return;

        if (!dummyPlayers.ContainsKey(playerId))
            dummyPlayers.Add(playerId, new DummyPlayer(playerId, DummyPlayer.PlayStrategy.RANDOM));

        dummyPlayers[playerId].UpdateHand(cards);

        StartCoroutine(dummyPlayers[playerId].PlayACard
            (
                Random.Range(playDelayRange.x,playDelayRange.y),
                roundNumber
            ));
    }
}
