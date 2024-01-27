using System;

namespace Remote.ApiStructs
{
    [Serializable]
    public class LobbyApiStruct
    {
        public LobbyApiDocumentStruct[] documents;
    }
    
        [Serializable]
        public class LobbyApiDocumentStruct
        {
            public LobbyPlayerFields fields;
    
        }
    
        [Serializable]
        public class LobbyPlayerFields
        {
            public StringApiField name;
            public StringApiField playerId;
        }
}