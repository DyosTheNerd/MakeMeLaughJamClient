using System;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Controllers
{
    [Serializable]
    public class LobbyPlayer
    {
        public string name;
        public string id;
    }
    
    public class Lobby
    {
        private static string roomNumber;
        
        public static List<LobbyPlayer> players = new List<LobbyPlayer>();
        
        
        public static string getRoomNumber()
        {
            if (roomNumber == null)
            {
                roomNumber = generateId();
            }
            return roomNumber;
        }
        
        
        private static string generateId()
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