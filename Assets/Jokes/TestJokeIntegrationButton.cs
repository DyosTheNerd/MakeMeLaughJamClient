using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TestJokeIntegrationButton : MonoBehaviour
{
    
    Random random = new Random();

    private void Start()
    {
        random.InitState();
    }

    public void OnClick()
    {
        Debug.Log("TestJokeIntegrationButton clicked");
        
        
        
        JokeRollManager.instance.OnCardPlayed(random.NextInt(5), "player1",random.NextInt(5));
    }
}
