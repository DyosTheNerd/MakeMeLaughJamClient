using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{


    [Header("LIKED TYPE OF CARDS")]
    public int numberOfLikedCardTypes = 2;
    public List<string> likedCardTypes = new List<string>();

    [Header("MOOD")]
    public int overlordMood = 51;
    public int cardInfluenceOnMood = 1;
    private float changeInMood;

    //GameObjects
    public GameObject PlayerManager;
    public GameObject CardManager;
    public GameObject OverlordMoodPointer;
    
    //STUFF
    public List<int> Numbers;
    RectTransform rectTransform;

    float pointerMovementSpeed = 1.0f;

    private void Start()
    {
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
        List<int> playedCardList = PlayerManager.GetComponent<PlayerManager>().PlayedCards();

        //JUDGE
        for (int i = 0; i < playedCardList.Count; i++)
        {
            int currentCardId = playedCardList[i];

            card currentCard = CardManager.GetComponent<CardManager>().GetCardInfo(currentCardId);

            foreach (string likedCard in likedCardTypes)
            {
                if (currentCard.typeOfCard == likedCard)
                {
                    //CHANGE IN MOOD        
                    changeInMood = changeInMood + cardInfluenceOnMood;

                    break;
                }
            }
        }

        //SHOW REACTION  
        if (overlordMood > 0 || overlordMood < 100)
        {
            rectTransform = OverlordMoodPointer.GetComponent<RectTransform>();
            Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x + changeInMood, rectTransform.anchoredPosition.y);

            Vector2 currentPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

            StartCoroutine(LerpFromTo(currentPosition, newPosition, 5f));
        }
        else
        {
            if (overlordMood < 0)
            {
                Debug.Log("ANGER!!!! - YOU ALL WILL DIE!");
            }
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