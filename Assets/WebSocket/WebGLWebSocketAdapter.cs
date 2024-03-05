using System.Runtime.InteropServices;
using UnityEngine;

/*
 * This class implements the IWebSocketAdapter for WebGL.
 */
public class WebGLWebSocketAdapter : MonoBehaviour ,IWebSocketAdapter
{
    [DllImport("__Internal")]
    private static extern void WebSocketInit (string gameId);

    
    [DllImport("__Internal")]
    private static extern void WebSocketSend (string message);


        
    
    public void Connect(string id)
    {
        Debug.Log("Unity GameRemoteSocket Start");
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
                WebSocketInit(id);
        #else
            Debug.Log("WEBGL Websocket Adapter - Connect: " + id);        
        #endif
        Debug.Log("Unity GameRemoteSocket End");
    }

    public void Send(string topic, string message)
    {
      
        
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            WebSocketSend(topic + "-:-" + message);
        #else
        Debug.Log("WEBGL Websocket Adapter - Send: " + topic + " " + message); 
        #endif
    }

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public void Receive(string topic, string message)
    {
        WebSocketBridge.instance.Receive(topic,message);
    }

    public void ReceiveWebsocketMessage(string msg)
    {
        WebSocketStruct wsStruct = JsonUtility.FromJson<WebSocketStruct>(msg);
        Receive(wsStruct.topic, msg);
    }
    
}

struct WebSocketStruct
{
    public string topic;
}