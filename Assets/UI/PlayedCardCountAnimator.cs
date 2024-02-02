using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayedCardCountAnimator : MonoBehaviour
{

    [Header("Parameters")]
    public int defaultFontSize = 11;
    public float maximumFontIncreaseRatio = 2.0f;
    public float decreaseRatio = 0.1f;
    public List<TextMeshProUGUI> cardCountTexts;

    [Header("")]
    public List<float> sizeRatios = new List<float>();


    private bool animationEnabled = false;
    public bool AnimationEnabled
    { 
        get => animationEnabled;
        set {

            animationEnabled = value;
            if(value == false)
            {
                for (int i = 0; i < sizeRatios.Count; i++)
                {
                    sizeRatios[i] = 1.0f;
                }
            }
        } }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cardCountTexts.Count; i++)
        {
            sizeRatios.Add(1.0f);
        }

        InteractionManager.instance.CardPlayed += PlayerPlayedCard;
    }

    void PlayerPlayedCard(int cardId, string playerId, int roundNumber)
    {
        if (AnimationEnabled)
        {
            string cardType = CardManager.instance.GetCardInfo(cardId).typeOfCard;
            int cardTypeId = CardManager.instance.GetCardTypeID(cardType);

            sizeRatios[cardTypeId] = maximumFontIncreaseRatio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimationEnabled)
        {
            for (int i = 0; i < cardCountTexts.Count; i++)
            {
                // reduces the difference by decreaseRatio% per second.
                float ratio = (sizeRatios[i] - 1.0f) * decreaseRatio * Time.deltaTime;
                sizeRatios[i] = sizeRatios[i] - ratio;

                cardCountTexts[i].fontSize = sizeRatios[i] * defaultFontSize;
            }
        }
    }
}
