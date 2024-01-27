using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHands : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.instance.CardPlayed += (cardId, playerId) =>
        {
            Debug.Log($"Card {cardId} played by {playerId}");
        };
        
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
        });
    }


}
