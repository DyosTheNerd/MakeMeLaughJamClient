using System;

namespace Remote.ApiStructs
{
    [Serializable]
    public class ActionApiStruct
    {
        public ApiDocumentStruct[] documents;
    }

    [Serializable]
    public class ApiDocumentStruct
    {
        public PlayedCardFields fields;

    }

    [Serializable]
    public class PlayedCardFields
    {
        public StringIntegerField id;
        public StringApiField playerId;
        public StringIntegerField roundNumber;
    }
}