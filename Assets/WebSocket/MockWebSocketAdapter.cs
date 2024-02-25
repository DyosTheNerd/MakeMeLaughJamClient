using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MockWebSocketAdapter : MonoBehaviour, IWebSocketAdapter
{
    private WebSocketBridge _bridge;

    [SerializeField] private TextMeshProUGUI topicInput;
    [SerializeField] private TextMeshProUGUI messageInput;
    
    

    public void Start()
    {
        _bridge = WebSocketBridge.instance;
        
        if (_bridge == null)
        {
            Debug.LogError("WebSocketBridge not found. Please add the WebSocketBridge prefab to the scene.");
        }
        

        
    }

    public void Connect(string id)
    {
        Debug.Log("Mock Connect " + id);
    }

    public void Send(string topic, string message)
    {
        Debug.Log($"Mock Send: {topic} {message}");
    }

    public void Close()
    {
        Debug.Log("Mock Close");
    }

    public void Receive(string topic, string message)
    {
        _bridge.Receive(topic, message);
    }

    public void FromInput()
    {
        // takes topicInput.text and formats it to a regular string
        string topic = topicInput.text.Substring(0, topicInput.text.Length - 1);
        string message = messageInput.text.Substring(0, messageInput.text.Length - 1);
        Receive(topic, message);
    }
}
