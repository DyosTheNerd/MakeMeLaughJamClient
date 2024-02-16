using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameLoopMessaging : MonoBehaviour
{

    [Header("Parameters")]
    public TextMeshProUGUI cardText;


    private bool playersVoting = false;
    private bool overlordJudging = false;


    public void ClearMessageCard()
    {
        cardText.text = "";
    }

    IEnumerator UpdatePlayersVotingText()
    {
        while (playersVoting)
        {
            cardText.text = $"Players are Joking \n{PlayerManager.instance.NumberOfVotedPlayers()} / {PlayerManager.instance.NumberOfTotalPlayers()}\n" +
                            $"Time remaining: {GameLoopController.instance.currentTimeout}";
            yield return null;

        }
    }

    public void EnablePlayersVotingMessage()
    {
        playersVoting = true;
        StartCoroutine(UpdatePlayersVotingText());
    }

    public void DisablePlayersVotingMessage()
    {
        playersVoting = false;
        cardText.text = "";
    }



    IEnumerator UpdateOverlordJudgingMessage(float frequency, System.Func<bool> stopCondition)
    {
        
        int count = 1;
        while(!stopCondition())
        {
            // TODO Jank probably garbage collected a lot, but its there.
            cardText.text = $"The Overlord is Judging your Standup Routine" + new string('.', count++ % 3);
            yield return new WaitForSeconds(frequency);
            
        }

        cardText.text = $"";
    }

    public void EnableOverlordJudgingMessage(System.Func<bool> stopCondition)
    {
        StartCoroutine(UpdateOverlordJudgingMessage(0.5f, stopCondition));
    }




    public void SetPleasedMessage()
    {
        // Select from a random assortment of messagers or whatever
        cardText.text = "The Alien Overlord is amused by your earthling humor.";
    }
    public void SetUnpleasedMessage()
    {
        // Select from a random assortment of messagers or whatever
        cardText.text = "The Alien Overlord hates what earthlings consider funny.";
    }
    IEnumerator DisplayOverlordMessageRoutine(bool pleased, float duration)
    {
        if (pleased)
        {
            SetPleasedMessage();
        }
        else
        {
            SetUnpleasedMessage();
        }

        yield return new WaitForSeconds(duration);
        cardText.text = "";
    }
    public void DisplayOverlordMessage(bool pleased, float duration)
    {
            StartCoroutine(DisplayOverlordMessageRoutine(pleased, duration));
    }


    public void EnablePrepareNewRoundMessage()
    {
        cardText.text = $"Round {GameLoopController.instance.currentRound + 1} is starting next! \n Prepare to Select a new Joke!";
    }

}
