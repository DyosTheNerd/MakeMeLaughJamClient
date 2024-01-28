using System.Collections;
using System.Collections.Generic;
using Controllers.Remote;
using Proyecto26;
using Remote.ApiStructs;
using UnityEngine;

public class JokeProvider : MonoBehaviour
{
    public static JokeProvider instance;

    private static Dictionary<string, string> jokeDB = new Dictionary<string, string>();
    
    private static bool initialized = false;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!initialized)
        {
            _initializeJokes();
           
        }
    }

    public string GetJoke(int jokeId)
    {
        
        if (CardManager.instance == null)
        {
            return $"What do you call a cow with no legs? Ground beef. - missing card Manager";
        }
        card card = CardManager.instance.GetCardInfo(jokeId);
        if (jokeDB.TryGetValue($"{card.typeOfCard}_{card.intensity}", out string joke))
        {
            return joke;
        }
        
        return $"What do you call a cow with no legs? Ground beef. - missing text for jokeId {jokeId} {card.typeOfCard} {card.intensity}";
    }

    public string GetJokeByCard(card c)
    {
        if (jokeDB.TryGetValue($"{c.typeOfCard}_{c.intensity}", out string joke))
        {
            return joke;
        }
        
        return $"What do you call a cow with no legs? Ground beef. - missing text for card {c.id} with {c.typeOfCard} {c.intensity}";
    }
    

    private void _initializeJokes()
    {
        RequestHelper request = new RequestHelper
        {
            Uri =$"{RemoteConfig.sJokeDbUrl}",
            EnableDebug = true
        };
        
        RestClient.Get<JokeApiStruct>(
                request)
            .Then(response =>
            {
                foreach (var doc in response.documents)
                {
                    jokeDB.Add(doc.fields.id.stringValue, doc.fields.text.stringValue);
                }
                Debug.Log(jokeDB.Count);
                initialized = true;
            })
            .Catch(reject => { Debug.Log(reject.Message); });
    }
    
}
