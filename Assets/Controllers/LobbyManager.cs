using UnityEngine;

namespace Controllers
{
    public class LobbyManager
    {
        public LobbyManager instance;
        
        public void awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
             Debug.LogError("Multiple LobbyManager instances detected.");
            }
        }
    }
}