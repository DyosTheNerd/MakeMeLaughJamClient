using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
}
[System.Serializable]
public class card
{
    public string typeOfCard;

    public int intensity;
    public card(string typeOfCard, int intensity)
    {
        this.typeOfCard = typeOfCard;
        this.intensity = intensity;
    }
}