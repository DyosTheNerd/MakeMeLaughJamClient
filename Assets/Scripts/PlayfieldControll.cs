using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldControll : MonoBehaviour
{
    public int totalRounds = 10;
    public int currentRound = 1;

    public List<GameObject> PlayedCards = new List<GameObject>();

    public GameObject overlord;
    public GameObject counter;

    private void StartNewRound() 
    {
        if(currentRound == totalRounds - 1) 
        {            
            Debug.Log("End of Game");
        }
        else
        {
            //Set Round
            currentRound = currentRound + 1;
            counter.GetComponent<TextMesh>().text = currentRound.ToString();

            //Set new Judging
            overlord.GetComponent<OverlordJudging>().BuildJudgeRules();
        }
    }
}