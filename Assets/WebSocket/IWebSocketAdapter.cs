
    public interface IWebSocketAdapter
    {
        public void Send(string topic, string message);
        
        public void Receive(string topic, string message);
        
        public void Connect(string id);
        
        public void Close();
    }
