using System.Collections;
using Controllers.Remote;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine;

public class HandRemote : MonoBehaviour
{
    void Start()
    {
        InteractionManager.instance.HandUpdated += OnHandUpdated;

        StartCoroutine(PollActions());
    }

    IEnumerator PollActions()
    {
        while (true)
        {
            RequestHelper helper = new RequestHelper();
            helper.EnableDebug = true;
            Debug.Log($"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}/actions/");
            RequestHelper request = new RequestHelper
            {
                Uri = $"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}/actions/",
                
                EnableDebug = true
            };

            RestClient.Get<ActionApiStruct>(request)
                .Then(response =>
                {
                    foreach (var doc in response.documents)
                    {
                        string playerId = doc.fields.playerId.stringValue;
                        int cardId = int.Parse(doc.fields.id.integerValue);
                        PlayCard(playerId, cardId);
                    }
                }).Catch(reject => { Debug.Log(reject.Message); });

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);
        }
    }

    public void OnHandUpdated(string playerId, CardInteraction[] cards)
    {
        HandApiStruct handApiStruct = HandApiStruct.FromHand(playerId, cards);

        RestClient.Patch<HandApiStruct>(
                $"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}/hands/hand_{playerId}",
                handApiStruct)
            .Then(response => { Debug.Log($"code = {response}"); }).Catch(reject => { Debug.Log(reject.Message); });

        RestClient.Delete(
                $"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}/actions/action_{playerId}")
            .Then(response => { Debug.Log($"code = {response}"); }).Catch(reject => { Debug.Log(reject.Message); });
    }

    // function to call when a card is played
    public void PlayCard(string playerId, int cardId)
    {
        Debug.Log($"Playing card {cardId} for player {playerId}");
        InteractionManager.instance.PlayCard(cardId, playerId);
    }
}