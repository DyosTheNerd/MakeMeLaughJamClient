using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("StartGameButton.OnClick");
         LobbyManager.instance.startGame();
    }
}
