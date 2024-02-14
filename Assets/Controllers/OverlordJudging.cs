using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    public static OverlordJudging instance;
    CardManager c_manager;
    PlayerManager p_manager;

    [Header("LIKED TYPE OF CARDS")]
    public List<string> likedCardTypes = new List<string>();
    public int overlordMood = 51;
    public int oldOverlordMood = 51;
    public int cardInfluenceOnMood = 1;
    public int numberOfLikedCardTypes = 2;

    public List<int> Numbers;

    public delegate void OnOverlordSatisfied();
    public event OnOverlordSatisfied SatisfiedOverlord;

    public delegate void OnOverlordDissatisfied();
    public event OnOverlordDissatisfied DissatisfiedOverlord;


    private void Awake()
    {
        if (instance == null)
        {
            OverlordJudging.instance = this;
        }
        else
        {
            Debug.LogError("Multiple OverlordJudging instances detected.");
        }
    }

    private void Start()
    {
        c_manager = CardManager.instance;
        p_manager = PlayerManager.instance;
        BuildlikedCardTypes();
    }

    private void BuildlikedCardTypes()
    {
        List<string> cardTypes = c_manager.cardTypes;

        //be save with each number only once
        for (int random = 0; random < cardTypes.Count; random++)
        {
            Numbers.Add(random);
        }

        for (int i = 0; i < numberOfLikedCardTypes; i++)
        {
            int random = Random.Range(0, Numbers.Count);
            int randomNumber = Numbers[random];

            for (int cartCount = 0; cartCount < cardTypes.Count; cartCount++)
            {
                if (randomNumber == cartCount)
                {
                    likedCardTypes.Add(cardTypes[cartCount]);
                }
            }
            Numbers.Remove(randomNumber);
        }
    }

    public void OverlordJugdgeNow()
    {

        float changeInMood = 0;
        List<int> playedCardList = p_manager.PlayedCards();

        //JUDGE
        for (int i = 0; i < playedCardList.Count; i++)
        {
            int currentCardId = playedCardList[i];

            card currentCard = c_manager.GetCardInfo(currentCardId);

            if (likedCardTypes.Contains(currentCard.typeOfCard))
            {
                    //Positive CHANGE IN MOOD        
                    changeInMood += cardInfluenceOnMood  * currentCard.intensity;
            }
            else
            {   
                //Negative CHANGE IN MOOD        
                changeInMood -= cardInfluenceOnMood * currentCard.intensity;
            }

                
            
        }

        oldOverlordMood = overlordMood;
        overlordMood += (int)changeInMood;
    }

    public void EvaluateMood()
    {        
        if (overlordMood <= 0)
        {
            Debug.Log("ANGER!!!! - YOU ALL WILL DIE!");
            DissatisfiedOverlord?.Invoke();
        }
        else if (overlordMood >= 100)
        {
            Debug.Log("LAUGHTER!!! -  YOU ALL WIN!!!");
            SatisfiedOverlord?.Invoke();
        }
    }

}