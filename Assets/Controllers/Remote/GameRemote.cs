using System.Collections;
using System.Collections.Generic;
using Controllers.Remote;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine;

public class GameRemote : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameStruct gameStruct = new GameStruct();
        gameStruct.id = RemoteConfig.instance.gameId;
        gameStruct.code = RemoteConfig.instance.gameId;
        
        GameApiStruct gameApiStruct = gameStruct.ToGameApiStruct();
        
        RestClient.Patch<GameApiStruct>($"{RemoteConfig.instance.baseProjectUrl}/{RemoteConfig.instance.gameId}", gameApiStruct)
            .Then(response =>
            {
                Debug.Log($"code = {response.fields.code.stringValue}");
            }).Catch(reject => {Debug.Log(reject.Data);});
        
    }
}
