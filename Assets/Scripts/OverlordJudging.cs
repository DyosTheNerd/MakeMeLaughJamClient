using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordJudging : MonoBehaviour
{
    public GameObject Playfield;
    public GameObject OverlordMoodPointer;
    float pointerMovementSpeed = 1.0f;

    [Header("LIKED TYPE OF CARDS")]
    public List<card> likedCardtypes = new List<card>();

    public int overlordMood = 51;
    public int cardInfluenceOnMood = 1;
    private float changeInMood;

    RectTransform rectTransform;

    public void OverlordJugdgeNow()
    {
        List<card> playedCardList = Playfield.GetComponent<PlayfieldControll>().PlayedCards;

        //JUDGE
        for (int i = 0; i < playedCardList.Count; i++)
        {
            card currentCard = playedCardList[i];

            foreach (card likedCard in likedCardtypes)
            {
                if (currentCard.typeOfCard == likedCard.typeOfCard)
                {
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