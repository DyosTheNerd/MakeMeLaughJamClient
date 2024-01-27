using System.Collections;
using System.Collections.Generic;
using Controllers.Remote;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine;

public class HandRemote : MonoBehaviour
{
    void Start()
    {
        InteractionManager.instance.HandUpdated += OnHandUpdated;        
    }
    
    public void OnHandUpdated(string playerId, CardInteraction[] cards)
    {   
        HandApiStruct handApiStruct = HandApiStruct.FromHand(playerId, cards);
        
        RestClient.Patch<HandApiStruct>($"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}/hands/hand_{playerId}", handApiStruct)
            .Then(response =>
            {
                Debug.Log($"code = {response}");
            }).Catch(reject =>
            {
                Debug.Log(reject.Message);
            });
    }

}
