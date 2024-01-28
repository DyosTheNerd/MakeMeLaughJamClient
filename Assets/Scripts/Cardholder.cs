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
    public List<bool> ListOfIntesity;
    public List<bool> BoolsOfIntesity;

    private void Start()
    {
        foreach (string type in typeOfCard)
        {
            //Default number on zero
            numberOfType.Add(0);
        }

        for (int i = 0; i < 9; i++)
        {
            //BoolsOfIntesity for Lights at cards
            BoolsOfIntesity[i] = false;
        }

        HandleStatus();
    }

    void HandleStatus()
    {
        for (int i = 0; i < 8; i++)
        {
            cardUI = this.gameObject.transform.GetChild(i);

            Debug.Log(numberOfType[i]);

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

            //Catch Intensity
            //ListOfIntesity.Add(currentCard.intensity);

        }

        //HANDLING STATUS WHEN INFORMATIONS ARE Catched
        for (int i = 0; i < 7; i++)
        {
            cardUI = this.gameObject.transform.GetChild(i);

            //Change Count on Card
            cardUI.GetChild(0).GetComponent<TextMeshProUGUI>().text = numberOfType[i].ToString();

            //Change Lights on Card by Bools


            for (int countOfLights = 0; countOfLights < 10; i++)
            {
                cardUI.GetChild(1).GetChild(countOfLights).GetComponent<Image>().enabled = true;
            }
        }

        //Clean-up numberOfType-list
        typeOfCard.Clear();
        foreach (string type in typeOfCard)
        {
            numberOfType.Add(0);
        }
    }

}