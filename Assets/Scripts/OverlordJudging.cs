using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    public GameObject Playfield;

    public void BuildJudgeRules()
    {
        //Build Taste

        //SaveTasteForRound

    }  

    public void OverlordJugdgeNow()
    {
        List<card> playedCardList = Playfield.GetComponent<PlayfieldControll>().PlayedCards;

        for(int i = 0; i < playedCardList.Count; i++) 
        {            
            card currentCard = playedCardList[i];
        }
    }
}