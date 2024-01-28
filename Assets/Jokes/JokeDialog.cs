using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JokeDialog : MonoBehaviour
{
    public Sprite portrait;

    
    public void SetJoke(string theJoke)
    {
        // get child by name
        transform.Find("Joke").GetComponent<TextMeshProUGUI>().text = theJoke;
    }

    public void SetName(string theName)
    {
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = theName;
    }
    
    public void SetPortrait(Sprite thePortrait)
    {
        transform.Find("Portrait").GetComponent<Image>().sprite = thePortrait;
    }

    public void FireDialog()
    {
        Debug.Log("Starting Joke ");
        StartCoroutine(DestroyDialog());
    }
    
    // coroutine to destroy the dialog after a few seconds
    IEnumerator DestroyDialog()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    

}
