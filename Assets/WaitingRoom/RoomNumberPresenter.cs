using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

public class RoomNumberPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = Lobby.getRoomNumber();
    }
}
