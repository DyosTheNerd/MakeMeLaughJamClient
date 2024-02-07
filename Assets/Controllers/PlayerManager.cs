using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("PLAYER PARAMETERS")]
    public int HandSize = 5;

    [Header("GAME VALUES")]
    // easier access list for methods.
    public List<Player> players;
    // dictionary to interface with ids
    private Dictionary<string, Player> id2Player = new Dictionary<string, Player>();
    public List<string> waitingForPlayers = new List<string>();
    public List<int> playedCards = new List<int>();
    public Dictionary<string, int> playedCardOfType = new Dictionary<string, int>();
    List<float> playedCardsRatio = new List<float>();


    [Header("MANAGERS")]
    CardManager cardManager;
    InteractionManager interactionManager;
    GameLoopController flowControl;


    private void Awake()
    {
        if (instance == null)
        {
            PlayerManager.instance = this;
        }
        else
        {
            Debug.LogError("Multiple PlayerManager instances detected.");
        }
    }

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        interactionManager = InteractionManager.instance;
        flowControl = GameLoopController.instance;

        interactionManager.CardPlayed += PlayerPlayCard;

        foreach (var player in Lobby.players)
        {
            AddPlayer(player.id, player.name);
        }

        foreach (var type in CardManager.instance.cardTypes)
        {
            playedCardOfType[type] = 0;
            playedCardsRatio.Add(0.0f);
        }
    }



    private void ComputePlayedCardRatios(bool normalized)
    {
        int types = CardManager.instance.cardTypes.Count;
        playedCardsRatio.Clear();
        float total = 0;
        float max = 0;
        for (int i = 0; i < types; i++)
        {
            playedCardsRatio.Add(playedCardOfType[CardManager.instance.cardTypes[i]]);
            total += playedCardsRatio[i];
            max = playedCardsRatio[i] > max ? playedCardsRatio[i] : max;
        }

        for (int i = 0; i < types; i++)
        {
            if (normalized)
                playedCardsRatio[i] /= max;
            else
                playedCardsRatio[i] /= total;
        }
    }

    public List<float> PlayedCardRatios()
    {
        return playedCardsRatio;
    }

    public int NumberOfVotedPlayers()
    {
        return players.Count - waitingForPlayers.Count;
    }
    public int NumberOfTotalPlayers()
    {
        return players.Count;
    }

    public Player GetPlayer(string id)
    {
        Player p;
        bool success = id2Player.TryGetValue(id, out p);
        if (success) return p;
        return new Player("null", "null");

    }

    public void RestockHands(int roundNumber)
    {
        //// for the first 
        //if(roundNumber == 1)
        //{
        FillHands();
        for (int i = 0; i < players.Count; i++)
        {
            interactionManager.UpdatePlayerHand(players[i].id, cardManager.ConvertToInteraction(players[i].ShowHand()).ToArray(), roundNumber);
        }
        UnreadyPlayers();
        //    return;
        //}

        //// Condition for card draw.
        //// E.G. if(roundNumber <=5 or roundNumber % 2 == 1)
        ////  for replenishing cards only until round 5 and then only on rounds 7 and 9
        ////if( fun condition )
        //PlayerDrawCards();

        //for (int i = 0; i < players.Count; i++)
        //{

        //    interactionManager.UpdatePlayerHand(players[i].id, cardManager.ConvertToInteraction(players[i].ShowHand()).ToArray(), roundNumber);
        //}
        //UnreadyPlayers();
    }

    public void FillHands()
    {
        Debug.Log("Filling hands");
        for (int i = 0; i < players.Count; i++)
        {
            List<int> dealtCards = cardManager.DrawCards(HandSize - players[i].cardsInHand.Count);

            Debug.Log("Player " + players[i].name + " was dealt cards " + string.Join("-", dealtCards));
            players[i].AddCards(dealtCards);
        }
    }

    public void PlayerDrawCards()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].AddCard(cardManager.DrawCard());
        }
    }

    public void AddPlayer(string id, string name)
    {
        Player player = new Player(id, name);
        players.Add(player);
        id2Player.Add(id, player);
        waitingForPlayers.Add(id);
    }

    public void ReadyPlayer(string id)
    {
        waitingForPlayers.Remove(id);
    }

    // Resets players and played cards for a new round.
    public void UnreadyPlayers()
    {
        waitingForPlayers.Clear();
        playedCards.Clear();

        foreach (var type in CardManager.instance.cardTypes)
            playedCardOfType[type] = 0;


        for (int i = 0; i < players.Count; i++)
        {
            waitingForPlayers.Add(players[i].id);
        }
    }

    public bool ArePlayersReady()
    {
        return waitingForPlayers.Count == 0;
    }

    public void PlayerPlayCard(int cardId, string playerId, int forRoundNumber)
    {
        // @andre
        if (forRoundNumber != flowControl.currentRound)
        {
            return;
        }
        // if the program is not waiting for this player anymore
        // we can discard this event
        if (!waitingForPlayers.Contains(playerId))
            return;
        ReadyPlayer(playerId);
        Player player = GetPlayer(playerId);
        player.PlayCard(cardId);

        playedCards.Add(cardId);

        string cardType = CardManager.instance.GetCardInfo(cardId).typeOfCard;
        playedCardOfType[cardType]++;

        Debug.Log("Player " + player.name + " played card " + cardId + " " + cardManager.GetCardInfo(cardId).typeOfCard);
        ComputePlayedCardRatios(true);
    }


    //Only valid if players are ready.
    public List<int> PlayedCards()
    {
        return new List<int>(playedCards);
    }

}


[System.Serializable]
public class Player
{
    public string id;
    public string name;

    public List<int> cardsInHand;
    //public Goal goal;

    public Player(string id, string name)
    {
        this.id = id;
        this.name = name;
        cardsInHand = new List<int>();
    }

    public bool IsValid()
    {
        return !id.Equals("null");
    }

    public void AddCard(int id)
    {
        cardsInHand.Add(id);
    }

    public void AddCards(List<int> ids)
    {
        cardsInHand.AddRange(ids);
    }

    public void PlayCard(int id)
    {
        cardsInHand.Remove(id);
    }

    public void Empty()
    {
        cardsInHand.Clear();
    }

    public IReadOnlyList<int> ShowHand()
    {
        return cardsInHand.AsReadOnly();
    }



}
