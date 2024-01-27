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

    

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        //InteractionManager.instance.HandUpdated += AddHandIfNotExists;
        //hands = new Dictionary<string, GameObject>();
    }

    private void AddHandIfNotExists(string playerId, CardInteraction[] cards)
    {
        //if (!hands.ContainsKey(playerId))
        //{
        //    GameObject hand = Instantiate(handPrefab, canvas.transform);
        //    HandLocal handLocal = hand.GetComponent<HandLocal>();
        //    handLocal.playerId = playerId;
        //    handLocal.OnHandUpdated(playerId, cards);
        //    hands.Add(playerId, hand);
        //}
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
            return;
        }

        // Condition for card draw.
        // E.G. if(roundNumber <=5 or roundNumber % 2 == 1)
        //  for replenishing cards only until round 5 and then only on rounds 7 and 9
        //if( fun condition )
        PlayerDrawCards();

        //TODO hands changed hook
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
        //TODO player ready hook
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

        //TODO player reset hook
    }

    public bool ArePlayersReady()
    {
        return waitingForPlayers.Count == 0;
    }

    public void PlayerPlayCard(string playerId, int cardId)
    {
        playedCards.Add(cardId);
        Player player = GetPlayer(playerId);

        // this just removes one instance from the player hand.
        player.PlayCard(cardId);

        //TODO player hand changed hook
        //TODO player card played hook
    }
    

    //Only valid if players are ready.
    public IReadOnlyList<int> PlayedCards()
    {
        return playedCards.AsReadOnly();
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
