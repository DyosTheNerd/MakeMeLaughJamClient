using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLocal : MonoBehaviour
{
    public string playerId;
    
    private CardInteraction[]   _cards;
    
    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.instance.HandUpdated += OnHandUpdated;        
    }
    
    public void OnHandUpdated(string playerId, CardInteraction[] cards)
    {
        if (playerId == this.playerId)
        {
            _setCards(cards);
        }
    }

    void _setCards(CardInteraction[] cards)
    {
        Debug.Log($"Hand updated for player {playerId}");
        foreach (CardInteraction card in cards)
        {
            Debug.Log($"Card {card.id} : {card.text}");
        }
        _cards = cards;
    }
    
    public void PlayCard(int cardId)
    {
        Debug.Log($"Playing card {cardId} for player {playerId}");
        
        InteractionManager.instance.PlayCard(cardId, playerId);
    }
    
    
}
