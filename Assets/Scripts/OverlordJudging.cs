using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    public GameObject Playfield;

    //Properties of Overlord
    [Header("Properties")]
    public List<property> propertyList = new List<property>();

    [System.Serializable]
    public class property
    {
        public string typeOfProperty;

        public string valueOfProperty;

        public property(string typeOfProperty, string valueOfProperty)
        {
            this.typeOfProperty = typeOfProperty;
            this.valueOfProperty = valueOfProperty;
        }
    }

    public void BuildJudgeRules()
    {
        //Build Taste

        //SaveTasteForRound

    }  

    void OverlordJugdgeNow()
    {
        List<GameObject> playedCardList = Playfield.GetComponent<PlayfieldControll>().PlayedCards;

        for(int i = 0; i < playedCardList.Count; i++) 
        {            
            GameObject currentCard = playedCardList[i];
        }
    }
}