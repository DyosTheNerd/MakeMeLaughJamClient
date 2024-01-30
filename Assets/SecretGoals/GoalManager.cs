using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [Header("PARAMETERS")]
    Vector2Int intensityRange = new Vector2Int(4, 6);
    Vector2Int quantityRange = new Vector2Int(4, 6);


    CardManager cardManager;

    public void Start()
    {
        cardManager = FindAnyObjectByType<CardManager>();
    }

    public Goal GetGoal(List<string> preferedTypes)
    {

        int goalType = Random.Range(0, 2);
        string cardType = cardManager.cardTypes[Random.Range(0, cardManager.cardTypes.Count)];

        //// jank
        switch (goalType)
        {
            // Case for card Intensity
            case 0:
                if (preferedTypes.Contains(cardType))
                {
                    return new Goal();
                }
                else
                {

                }
                break;
            // Case for card type
            case 1:
                break;
        }

        return new Goal();
    }


}

public enum GoalOperation
{
    INTENSITY_GREATER,
    INTENSITY_LESSER,
    CARD_TYPE
}

[System.Serializable]
public struct Goal
{
    //public string cardType;
    public int quantity;
    public int counter;
    public string cardType;
    public int intensity;
    public GoalOperation operation;

    public Goal(GoalOperation operation, int quantity, int intensity, string cardType)
    {
        this.intensity = intensity;
        this.quantity = quantity;
        this.operation = operation;
        this.cardType = cardType;

        counter = 0;
    }

    public bool Match(card card)
    {
        return false;
    }
}
