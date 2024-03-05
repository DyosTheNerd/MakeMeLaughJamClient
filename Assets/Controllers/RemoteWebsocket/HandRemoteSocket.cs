using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class HandRemoteSocket: MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void WebSocketSend (string playerAndHand );
    
      void Start()
    {
        InteractionManager.instance.HandUpdated += OnHandUpdated;

        WebSocketBridge.instance.messageReceivedEvent += WebSocketReceive;
        
    }

    public void OnHandUpdated(string playerId, CardInteraction[] cards, int roundNumber)
    {
        string message = handUpdatedAsJSON(playerId, cards, roundNumber);
        WebSocketBridge.instance.Send( "handUpdate-" + playerId, message);
    }

    public void WebSocketReceive(string topic, string message)
    {
        if (topic == "playCard")
        {
            PlayCard playCard = JsonUtility.FromJson<PlayCard>(message);
            
            _playCard(playCard.playerId, playCard.cardId, playCard.roundNumber);
        }
        
    }
    
    
    private void _playCard(string playerId, int cardId, int roundNumber )
    {
        Debug.Log($"Playing card {cardId} for player {playerId}");
        InteractionManager.instance.PlayCard(cardId, playerId, roundNumber);
    }
    
    
    [Serializable]
    struct HandUpdate 
    {
        public string playerId;
        public CardInteraction[] cards;
        public int roundNumber;
    }
    
    [Serializable]
    struct PlayCard 
    {
        public string playerId;
        public int cardId;
        public int roundNumber;
    }
    
    private string handUpdatedAsJSON(string playerId, CardInteraction[] cards, int roundNumber)
    {
        return  JsonUtility.ToJson(new HandUpdate {playerId = playerId, cards = cards, roundNumber = roundNumber});
    }
}