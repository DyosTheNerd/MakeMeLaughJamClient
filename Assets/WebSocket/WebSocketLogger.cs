using UnityEngine;

public class WebSocketLogger : MonoBehaviour
{
    private WebSocketBridge webSocketBridge;
    void Start()
    {
        webSocketBridge = WebSocketBridge.instance;
        if(webSocketBridge == null)
        {
            Debug.LogError("WebSocketBridge not found. Please add the WebSocketBridge prefab to the scene.");
        }
        webSocketBridge.messageReceivedEvent += LogMessage;
    }

    private void LogMessage(string topic, string message)
    {
        Debug.Log($"Received message: {topic} {message}");
    }
}
