using System.Collections;
using Controllers;
using UnityEngine;

public class GameRemoteSocket : MonoBehaviour
{
    public static GameRemoteSocket instance;

    private string gameId;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("error");
        }

        gameId = Lobby.getRoomNumber();
    }

    public void Start()
    {
        StartCoroutine(Connect());

    }
    
    private IEnumerator Connect()
    {
        yield return new WaitForSeconds(1);
        WebSocketBridge.instance.Connect(gameId);
    }
}