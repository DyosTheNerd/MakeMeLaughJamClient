using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalHandManager : MonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> hands;
    
    [SerializeField] private GameObject handPrefab; 
    
    [SerializeField] private GameObject canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.instance.HandUpdated += AddHandIfNotExists;
        hands = new Dictionary<string, GameObject>();
    }

    private void AddHandIfNotExists(string playerId, CardInteraction[] cards)
    {
        if (!hands.ContainsKey(playerId))
        {
            if (canvas == null)
            {
                Debug.LogError($"Canvas is null {transform.name}");
                
            }
            else
            {
                GameObject hand = Instantiate(handPrefab, canvas.transform);
                HandLocal handLocal = hand.GetComponent<HandLocal>();
                handLocal.playerId = playerId;
                handLocal.OnHandUpdated(playerId, cards);
                hands.Add(playerId, hand);    
            }
            
        }
    }
    
}
