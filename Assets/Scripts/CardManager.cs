using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;


    [Header("Parameters")]
    public List<string> cardTypes;
    // How many cards to populate the card dictionary with.
    // Currently using one for each type, intensity value inside the interval
    // (ratingInterval.y - ratingInterval.x) * cardTypes.Count() = different card templates.
    //public int cardVariety;

    // y must be always bigger or equal to x
    public Vector2Int ratingInterval;

    [Header("Internal Things")]
    public List<card> cardDictionary;


    // This logic can be adjusted to be non uniform.
    public void GenerateCardDictionary()
    {
        for (int i = 0; i < cardTypes.Count; i++)
        {
            for (int j = 0; j <= ratingInterval.y - ratingInterval.x; j++)
            {
                cardDictionary.Add(new card(cardDictionary.Count, cardTypes[i], ratingInterval.x + j));
            }
        }
    }

    public card GetCardInfo(int id)
    {
        return cardDictionary[id];
    }

    public List<int> DrawCards(int n)
    {
        List<int> cards = new List<int>();
        for (int i = 0; i < n; i++)
        {
            cards.Add(DrawCard());
        }
        return new List<int>();
    }

    public int DrawCard()
    {
        return Random.Range(0, cardDictionary.Count);
    }

    private void Start()
    {
        GenerateCardDictionary();
        instance = this;
    }


    public List<CardInteraction> ConvertToInteraction(IReadOnlyList<int> cardsIds)
    {
        List<CardInteraction> cardInteractions = new List<CardInteraction>();

        foreach (var cardId in cardsIds)
        {
            cardInteractions.Add(GetCardInfo(cardId).AsInteraction());
        }

        return cardInteractions;
    }

}


[System.Serializable]
public struct card
{
    public int id;
    public string typeOfCard;
    public int intensity;
    public card(int id, string typeOfCard, int intensity)
    {
        this.id = id;
        this.typeOfCard = typeOfCard;
        this.intensity = intensity;
    }

    public CardInteraction AsInteraction()
    {
        return new CardInteraction
        {
            id = id,
            text = "",
            intensity = intensity,
            type = typeOfCard
        };
    }
}