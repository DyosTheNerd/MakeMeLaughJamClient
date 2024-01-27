using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Remote
{
    public class RemoteConfig : MonoBehaviour
    {
            
        
        public string baseProjectUrl =
            "https://firestore.googleapis.com/v1/projects/makemelaughjam/databases/(default)/documents/games";

        public string gameId = "126875";
        
        public static RemoteConfig instance;
        
        private void Awake()
        {
            if (instance == null)
            {
                gameId = generateId();
                instance = this;
            }
        }
        private string generateId()
        {
            string id = "";
            for (int i = 0; i < 6; i++)
            {
                id += Random.Range(0, 10);
            }

            return id;
        }
    }
}