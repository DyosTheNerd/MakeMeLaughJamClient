using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cardholder : MonoBehaviour
{
    public GameObject PlayerManager;
    public GameObject CardManager;

    private Transform cardUI;

    public List<int> numberOfType;
    public List<string> typeOfCard;
    public List<int> countOfIntesity;
    public List<int> ListOfIntesity;

    private void Start()
    {
        foreach (string type in typeOfCard)
        {
            //Default number on zero
            numberOfType.Add(0);
        }

        HandleStatus();
    }

    void HandleStatus()
    {
        for (int i = 0; i < 8; i++)
        {
            cardUI = this.gameObject.transform.GetChild(i);

            //Change Count on Card
            cardUI.GetChild(0).GetComponent<TextMeshProUGUI>().text = numberOfType[i].ToString();

            //Change Lights on Card by Bools
            for (int countOfLights = 0; countOfLights < 10; countOfLights++)
            {
                cardUI.GetChild(1).GetChild(countOfLights).GetComponent<Image>().enabled = false;
            }
        }
    }

    public void SetCardUI()
    {
        List<int> playedCardList = PlayerManager.GetComponent<PlayerManager>().PlayedCards();

        //Gettings Infos for playedCards and Store
        for (int i = 0; i < playedCardList.Count; i++)
        {
            //CurrentCard
            int currentCardId = playedCardList[i];
            card currentCard = CardManager.GetComponent<CardManager>().GetCardInfo(currentCardId);

            //Increase COUNT OF CARDS
            numberOfType[i] = numberOfType[i] + 1;

            for (int placeInList = 0; i < 7; i++)
            {
                if (currentCard.typeOfCard == typeOfCard[placeInList])
                {
                    //Ad Intensity tot Intensity of Card (total in between)
                    ListOfIntesity[placeInList] = ListOfIntesity[placeInList] + currentCard.intensity;
                    countOfIntesity[placeInList] = countOfIntesity[placeInList] + 1;
                    break;
                }
            }
        }

        //Average of card intensity
        for (int intensity = 0; intensity < ListOfIntesity.Count; intensity++)
        {
            ListOfIntesity[intensity] = ListOfIntesity[intensity] / countOfIntesity[intensity];
        }

        //HANDLING STATUS WHEN INFORMATIONS ARE Catched
        for (int i = 0; i < 7; i++)
        {
            cardUI = this.gameObject.transform.GetChild(i);

            //Change Count on Card
            cardUI.GetChild(0).GetComponent<TextMeshProUGUI>().text = numberOfType[i].ToString();

            //Change Lights on Card
            for (int countOfLights = 0; countOfLights < 10; countOfLights++)
            {
                if (ListOfIntesity.Count != 0)
                {

                    if (ListOfIntesity[i] < countOfLights)
                    {
                        cardUI.GetChild(1).GetChild(countOfLights).GetComponent<Image>().enabled = true;
                    }
                }
            }
        }

        //Clean-up numberOfType-list
        typeOfCard.Clear();
        ListOfIntesity.Clear();
        countOfIntesity.Clear();

        foreach (string type in typeOfCard)
        {
            numberOfType.Add(0);
        }
    }

}