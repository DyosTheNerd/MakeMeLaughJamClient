using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJokeIntegrationButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("TestJokeIntegrationButton.OnClick");
        Debug.Log(JokeProvider.instance.GetJoke(1));
    }
}
