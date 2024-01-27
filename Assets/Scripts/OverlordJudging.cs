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
    private float changeInMood;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = OverlordMoodPointer.GetComponent<RectTransform>();
    }

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
                    changeInMood = changeInMood + 1;

                    break;
                }
            }
        }

        //SHOW REACTION  
        if (overlordMood > 0 || overlordMood < 100)
        {

            Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x,rectTransform.anchoredPosition.y + 100.0f);

            Vector2 currentPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

            StartCoroutine(LerpFromTo(currentPosition, newPosition, 1.5f));
        }
        else
        {
            Debug.Log("End of Scale");
        }

        //Reset
        changeInMood = 0;
    }


    IEnumerator LerpFromTo(Vector2 pos1, Vector2 pos2, float duration)
    {
        for (
            float t = 0f;
            t < duration;
            t += pointerMovementSpeed * Time.deltaTime
        )
        {
            transform.position = Vector2.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
    }
}