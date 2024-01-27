using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

public class PlayerCountPresenter : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        LobbyManager.instance.updatePlayersEvent += OnPlayerCountUpdated;
    }

    public void OnPlayerCountUpdated(List<LobbyPlayer> players)
    {
        Debug.Log("PlayerCountPresenter.OnPlayerCountUpdated");
        this.GetComponent<TextMeshProUGUI>().text = $"{players.Count}";
        
    }
}
