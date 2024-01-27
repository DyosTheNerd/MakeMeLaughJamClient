using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [Header("PLAYER PARAMETERS")]
    public int HandSize = 5;

    [Header("GAME VALUES")]
    // easier access list for methods.
    public List<Player> players;
    // dictionary to interface with ids
    private Dictionary<string, Player> id2Player = new Dictionary<string, Player>();
    public List<string> waitingForPlayers = new List<string>();
    public List<int> playedCards = new List<int>();

    [Header("MANAGERS")]
    CardManager cardManager;
    InteractionManager interactionManager;



    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        interactionManager = FindObjectOfType<InteractionManager>();

        interactionManager.CardPlayed += PlayerPlayCard;
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
        // for the first 
        if(roundNumber == 1)
        {
            FillHands();
            for (int i = 0; i < players.Count; i++)
            {
                interactionManager.UpdatePlayerHand(players[i].id, cardManager.ConvertToInteraction(players[i].ShowHand()).ToArray());
            }
            UnreadyPlayers();
            return;
        }

        // Condition for card draw.
        // E.G. if(roundNumber <=5 or roundNumber % 2 == 1)
        //  for replenishing cards only until round 5 and then only on rounds 7 and 9
        //if( fun condition )
        PlayerDrawCards();

        for (int i = 0; i < players.Count; i++)
        {
            interactionManager.UpdatePlayerHand(players[i].id, cardManager.ConvertToInteraction(players[i].ShowHand()).ToArray());
        }
        UnreadyPlayers();
    }

    public void FillHands()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].AddCards(cardManager.DrawCards(HandSize));
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
        for (int i = 0; i < players.Count; i++)
        {
            waitingForPlayers.Add(players[i].id);
        }
    }

    public bool ArePlayersReady()
    {
        return waitingForPlayers.Count == 0;
    }

    public void PlayerPlayCard(int cardId, string playerId)
    {
        // if the program is not waiting for this player anymore
        // we can discard this event
        if (!waitingForPlayers.Contains(playerId))
            return;

        playedCards.Add(cardId);
        Player player = GetPlayer(playerId);

        // this just removes one instance from the player hand.
        player.PlayCard(cardId);
        ReadyPlayer(playerId);
    }
    

    //Only valid if players are ready.
    public List<int> PlayedCards()
    {
        return playedCards;
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
