using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHands : MonoBehaviour
{

    CardManager cardManager;

    // Start is called before the first frame update
    void Start()
    {
        cardManager = FindAnyObjectByType<CardManager>();

        InteractionManager.instance.CardPlayed += (cardId, playerId, roundnr) =>
        {
            Debug.Log($"Card {cardId} played by {playerId} - {roundnr}");
        };
    }

    public void UpdateHand()
    {
        InteractionManager.instance.UpdatePlayerHand("a", new CardInteraction[2]{
            new CardInteraction()
            {
                id = 1,
                text = "test1",
                type = "type1"
            },
            new CardInteraction()
            {
                id = 2,
                text = "test2",
                type = "type2"
            }
        }, 0);
    }
    

}
