using System;
using System.Collections.Generic;

namespace Remote.ApiStructs
{
    public class GameListApiStruct
    {
        public GameListFields fields;
    }
    public class GameListFields
    {
        private Dictionary<string, StringApiField> games;
    }
    
    
    public class GameStruct
    {
        public string id;
        public string code;


        public GameStruct FromGameApiStruct(GameApiStruct apiStruct)
        {
            return null;
        }
         
        public GameApiStruct ToGameApiStruct()
        {
            GameApiStruct apiStruct = new GameApiStruct()
                {
                    fields = new GameApiFields()
                    {
                        code = new StringApiField()
                        {
                            stringValue = code
                        }
                    }
                };
            
            return apiStruct;
        }
    }

    [Serializable]
    public class GameApiStruct
    {
        public GameApiFields fields;
              
    }
    
    [Serializable]
    public class GameApiFields
    {
        public StringApiField code;
    }

    [Serializable]
    public class StringApiField
    {
        public string stringValue;
    }
    
}