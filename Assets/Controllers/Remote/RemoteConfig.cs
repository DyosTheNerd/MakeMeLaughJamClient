using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Remote
{
    public class RemoteConfig : MonoBehaviour
    {
        
        public static string SbaseProjectUrl =
            "https://firestore.googleapis.com/v1/projects/makemelaughjam/databases/(default)/documents/games";

        
        public  string baseProjectUrl =
           SbaseProjectUrl;

        public string gameId = "126875";
        
        public static RemoteConfig instance;
        
        private void Awake()
        {
            if (instance == null)
            {
                gameId = Lobby.getRoomNumber();
                instance = this;
            }
        }
        
    }
}