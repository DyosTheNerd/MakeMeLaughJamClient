using System;

namespace Remote.ApiStructs
{
    [Serializable]
    public class JokeApiStruct
    {
        public JokeDocuments[] documents;
    }
    [Serializable]
    public class JokeDocuments
    {
        public JokeFields fields;
    }
    
    [Serializable]
    public class JokeFields
    {
        public StringApiField id;
        public StringApiField text;
    }
    
}