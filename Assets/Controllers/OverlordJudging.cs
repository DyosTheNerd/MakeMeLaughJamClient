using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    CardManager c_manager;
    PlayerManager p_manager;

    public GameObject PlayerManager;
    public GameObject CardManager;
    public GameObject OverlordMoodPointer;
    float pointerMovementSpeed = 1.0f;

    [Header("LIKED TYPE OF CARDS")]
    public List<string> likedCardTypes = new List<string>();
    public int overlordMood = 51;
    public int cardInfluenceOnMood = 1;
    public int numberOfLikedCardTypes = 2;
    private float changeInMood;
    public List<int> Numbers;
    RectTransform rectTransform;

    public delegate void OnOverlordSatisfied();
    public event OnOverlordSatisfied SatisfiedOverlord;

    public delegate void OnOverlordDissatisfied();
    public event OnOverlordDissatisfied DissatisfiedOverlord;


    private void Start()
    {
        c_manager = CardManager.GetComponent<CardManager>();
        p_manager = PlayerManager.GetComponent<PlayerManager>();
        BuildlikedCardTypes();
    }

    private void BuildlikedCardTypes()
    {
        List<string> cardTypes = CardManager.GetComponent<CardManager>().cardTypes;

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

        rectTransform = OverlordMoodPointer.GetComponent<RectTransform>();
        Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x + changeInMood, rectTransform.anchoredPosition.y);

        Vector2 currentPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        StartCoroutine(LerpFromTo(currentPosition, newPosition, 5f));

        //SHOW REACTION
        if (overlordMood < 0)
        {
            Debug.Log("ANGER!!!! - YOU ALL WILL DIE!");
            DissatisfiedOverlord?.Invoke();
        }
        else if (overlordMood > 100)
        {
            Debug.Log("LAUGHTER!!! -  YOU ALL WIN!!!");
            SatisfiedOverlord?.Invoke();
        }

        //Reset
        changeInMood = 0;
    }

    IEnumerator LerpFromTo(Vector2 currentPosition, Vector2 newPosition, float duration)
    {
        for (
            float t = 0f; t < duration; t += pointerMovementSpeed * Time.deltaTime
        )
        {
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, newPosition, t / duration);
            yield return 0;
        }
    }
}