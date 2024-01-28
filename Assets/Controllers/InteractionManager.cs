using UnityEngine;

// struct representing a card
public struct CardInteraction
{
    public int id;
    public string text;
    public int intensity;
    public string type;
}

/// <summary>
/// Manager for interactions between the game and the player.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    
    // Event with two integer parameters
    public delegate void OnCardPlayed(int cardId, string playerId, int roundNumber);
    public event OnCardPlayed CardPlayed;
    
    public delegate void OnUpdatePlayerHand(string playerId, CardInteraction[] cards, int roundNumber);
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

    public void PlayCard(int cardId, string playerId, int roundNumber)
    {
        CardPlayed?.Invoke(cardId, playerId,  roundNumber);
    }
    
    public void UpdatePlayerHand(string playerId, CardInteraction[] cards, int roundNumber)
    {
        HandUpdated?.Invoke(playerId, cards, roundNumber);
    }
}
