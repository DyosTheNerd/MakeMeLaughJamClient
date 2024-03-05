mergeInto(LibraryManager.library, {

  WebSocketInit: function(id){
    window.dispatchReactUnityEvent("WebSocketInit", UTF8ToString(id));
  },
  
  WebSocketSend: function(msg){
    const  message = UTF8ToString(msg);
    
    window.dispatchReactUnityEvent("WebSocketSend",message);
  },

  GetGameQrCode: function(id){
    window.dispatchReactUnityEvent("GetGameQrCode", UTF8ToString(id));
  }
  
});