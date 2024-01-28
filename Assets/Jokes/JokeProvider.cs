using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeProvider : MonoBehaviour
{
    public static JokeProvider instance;
    
    void Awake()
    {
        instance = this;
    }

    public string GetJoke(int jokeId)
    {
        return $"What do you call a cow with no legs? Ground beef. - {jokeId}";
    }
}
