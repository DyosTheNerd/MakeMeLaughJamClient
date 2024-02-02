using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameLoopMessaging : MonoBehaviour
{

    [Header("Parameters")]
    public TextMeshProUGUI cardText;


    private bool playersVoting = false;

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

}
