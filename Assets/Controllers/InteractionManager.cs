using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct representing a card
public struct CardInteraction
{
    public int id;
    public string text;
    public string type;
}


public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    
    // Event with two integer parameters
    public delegate void OnCardPlayed(int cardId, string playerId);
    public event OnCardPlayed CardPlayed;
    
    public delegate void OnUpdatePlayerHand(string playerId, CardInteraction[] cards);
    public event OnUpdatePlayerHand HandUpdated;
    
    void Awake()
    {
        if (instance == null)
        {
            InteractionManager.instance = this;    
        }
        else
        {
            Debug.LogError("Multiple InteractionManager instances detected.");
        }
        
    }

    public void PlayCard(int cardId, string playerId)
    {
        CardPlayed?.Invoke(cardId, playerId);
    }
    
    public void UpdatePlayerHand(string playerId, CardInteraction[] cards)
    {
        HandUpdated?.Invoke(playerId, cards);
    }
}
