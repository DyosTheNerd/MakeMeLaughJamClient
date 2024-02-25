using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnClick()
    {
        if (Lobby.players.Count > 0)
        {
            SceneManager.LoadScene("Level_1");
        }
    }
}
