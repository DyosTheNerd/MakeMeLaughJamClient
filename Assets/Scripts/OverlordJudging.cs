using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    public GameObject PlayerManager;
    public GameObject CardManager;
    public GameObject OverlordMoodPointer;
    float pointerMovementSpeed = 1.0f;

    [Header("LIKED TYPE OF CARDS")]
    public List<card> likedCardTypes = new List<card>();

    public int overlordMood = 51;
    public int cardInfluenceOnMood = 1;
    private float changeInMood;

    RectTransform rectTransform;

    private void Start()
    {
        BuildlikedCardTypes();
    }

    private void BuildlikedCardTypes()
    {

    }

    public void OverlordJugdgeNow()
    {
        List<int> playedCardList = PlayerManager.GetComponent<PlayerManager>().PlayedCards();

        //JUDGE
        for (int i = 0; i < playedCardList.Count; i++)
        {
            int currentCardId = playedCardList[i];

            card currentCard = CardManager.GetComponent<CardManager>().GetCardInfo(currentCardId);

            foreach (card likedCard in likedCardTypes)
            {
                if (currentCard.typeOfCard == likedCard.typeOfCard)
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
            Debug.Log("End of Scale");
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