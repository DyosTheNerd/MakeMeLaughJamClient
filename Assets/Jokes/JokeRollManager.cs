using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeRollManager : MonoBehaviour
{
    public static JokeRollManager instance;
    
    public GameObject jokeDialogPrefab;

    public Sprite[] portraits;
    
    private Dictionary<string, string> playedJokes = new Dictionary<string, string>();
    
    private GameObject currentJokeDialog;
    
    void Start()
    {
        instance = this;
        if (InteractionManager.instance)
        {
            InteractionManager.instance.CardPlayed += OnCardPlayed;
        }
        
    }

    public void RollJoke(int cardId, string playerId)
    {
        if (currentJokeDialog != null)
        {
            Destroy(currentJokeDialog);
        }
        
        
        
        string joke = JokeProvider.instance.GetJoke(cardId);
        
        Sprite portrait = getPortrait(playerId);
        
        GameObject newInstance = Instantiate(jokeDialogPrefab);
        JokeDialog dialog = newInstance.GetComponent<JokeDialog>(); 
        dialog.SetJoke(joke);
        dialog.transform.SetParent(transform);
        dialog.transform.localPosition = Vector3.zero;
        dialog.SetPortrait(portrait);

        dialog.SetName(getPlayerNameAndStatsText(playerId, cardId));
        dialog.FireDialog();
        
        currentJokeDialog = newInstance;

    }
    
    string getPlayerNameAndStatsText(string playerId, int cardId)
    {
        string playerName = getPlayerName(playerId);
        card theCard = getCard(cardId);
        
        return $"{playerName} makes a {theCard.typeOfCard} joke with intensity {theCard.intensity}!";
    }

    card getCard(int cardId)
    {
        
        if (CardManager.instance == null) return new card()
        {
            id = 0,
            intensity = 0,
            typeOfCard = "NoType",
        };
        
        return CardManager.instance.GetCardInfo(cardId);
    }
    
    string getPlayerName(string playerId)
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager == null) return "NoName";
        return playerManager.GetPlayer(playerId).name;
    }
    
    public void OnCardPlayed(int cardId, string playerId, int roundNumber)
    {
        Debug.Log("Card played: " + cardId + " by " + playerId + " in round " + roundNumber);
        if (playedJokes.ContainsKey($"{playerId}-{roundNumber}")) return;
        
        RollJoke(cardId, playerId);
        playedJokes.Add($"{playerId}-{roundNumber}", "joke");
        
    }

    Sprite getPortrait(string playerId)
    {
        
        if (portraits.Length == 0)
        {
            return null;
        } 
        
        int hash = playerIdToHashInteger(playerId);
        
        int index = hash % portraits.Length;
        
        return portraits[index];
    }

    int playerIdToHashInteger(string playerId)
    {
        int hash = 0;
        foreach (var character in playerId)
        {
            hash += character;
        }

        return hash;
    }
    
    
}
