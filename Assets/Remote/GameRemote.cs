using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine;

public class GameRemote : MonoBehaviour
{
    
    private static string _baseProjectUrl =
        "https://firestore.googleapis.com/v1/projects/makemelaughjam/databases/(default)/documents/games";

    // Start is called before the first frame update
    void Start()
    {
        GameStruct gameStruct = new GameStruct();
        gameStruct.id = "test";
        gameStruct.code = "123456";
        
        GameApiStruct gameApiStruct = gameStruct.ToGameApiStruct();
        
        
        //$"{_firebaseURL}scores.json"
        Debug.Log("Doing POST");
        RestClient.Post<GameApiStruct>($"{_baseProjectUrl}", gameApiStruct)
            .Then(response =>
            {
                Debug.Log($"code = {response.fields.code.stringValue}");
            }).Catch(reject => {Debug.Log(reject.Data);});
        
        
        RestClient.Put<GameApiStruct>($"{_baseProjectUrl}/list", gameApiStruct)
            .Then(response =>
            {
                Debug.Log($"code = {response.fields.code.stringValue}");
            }).Catch(reject => {Debug.Log(reject.Data);});
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
