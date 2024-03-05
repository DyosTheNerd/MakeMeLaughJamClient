using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

public class PlayerCountPresenter : MonoBehaviour
{
    void Start()
    {
        LobbyManagerSocket.instance.updatePlayersEvent += OnPlayerCountUpdated;
    }

    public void OnPlayerCountUpdated(List<LobbyPlayer> players)
    {
        Debug.Log("updating players: " + players.Count);
        GetComponent<TextMeshProUGUI>().text = $"{players.Count}";
    }
}
