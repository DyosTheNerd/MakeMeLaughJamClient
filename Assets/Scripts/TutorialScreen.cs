using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour
{
    public void GoToWaitingRoom()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene("WaitingRoomScene");
    }
}
