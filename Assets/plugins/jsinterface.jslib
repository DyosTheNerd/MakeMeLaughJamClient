mergeInto(LibraryManager.library, {

  WebSocketInit: function(id){
    window.dispatchReactUnityEvent("WebSocketInit", UTF8ToString(id));
  },
  
  WebSocketSend: function(topic, message){
    window.dispatchReactUnityEvent("WebSocketSend", UTF8ToString(topic), UTF8ToString(message));
  },

  GetGameQrCode: function(id){
    window.dispatchReactUnityEvent("GetGameQrCode", UTF8ToString(id));
  }
  
});