using UnityEngine;


/*
 * This class is a bridge between the Unity game and the WebSocket connection.
 */
public class WebSocketBridge : MonoBehaviour
{
    public static WebSocketBridge instance;

    [SerializeField]
    private  GameObject[] webSocketAdapters;
    
    public delegate void OnMessageReceived(string topic, string message);
    
    public event OnMessageReceived messageReceivedEvent;
    
    private IWebSocketAdapter[] _webSocketAdapters;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple WebSocketBridge instances detected.");
        }
    }

    void Start()
    {
        if (webSocketAdapters.Length == 0)
        {
            Debug.LogError("WebSocketAdapters not found. Please add the WebSocketAdapter prefab to the scene.");
        }
        
        _webSocketAdapters = new IWebSocketAdapter[webSocketAdapters.Length];
        for (int i = 0; i < webSocketAdapters.Length; i++)
        {
            _webSocketAdapters[i] = webSocketAdapters[i].GetComponent<IWebSocketAdapter>();
        }
    }
    
    public void Connect(string id)
    {
        foreach (var webSocketAdapter in _webSocketAdapters)
        {
            webSocketAdapter.Connect(id);
        }
    }

    public void Send(string topic, string message)
    {
        foreach (var webSocketAdapter in _webSocketAdapters)
        {
            webSocketAdapter.Send(topic, message);
        }
    }

    public void Close()
    {
        foreach (var webSocketAdapter in _webSocketAdapters)
        {
            webSocketAdapter.Close();
        }
    }

    public void Receive(string topic, string message)
    {
        messageReceivedEvent?.Invoke(topic, message);
    }
}
