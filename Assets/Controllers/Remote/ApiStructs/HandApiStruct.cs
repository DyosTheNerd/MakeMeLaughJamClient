using System;
using System.Collections.Generic;
using Proyecto26;

namespace Remote.ApiStructs
{
    [Serializable]
    public class HandApiStruct
    {

        public HandWrapperStruct fields = new HandWrapperStruct();
        
        public  static HandApiStruct FromHand(string playerId, CardInteraction[] cards)
        {
            HandApiStruct handApiStruct = new HandApiStruct();

            handApiStruct.fields = new HandWrapperStruct()
            {
                playerId = new StringApiField()
                {
                    stringValue = playerId
                },
                cards = new CardApiStruct()
                {
                    arrayValue = new CardArray()
                    {
                        values = new CardArrayValues[cards.Length]
                    }
                }
            };
            for (int i = 0; i < cards.Length; i++)
            {
                CardInteraction card = cards[i];
                CardMapApiStruct cardMapApiStruct = new CardMapApiStruct()
                {
                    fields = new CardApiFields()
                    {
                        id = new StringIntegerField()
                        {
                            integerValue = $"{card.id}"
                        },
                        type = new StringApiField()
                        {
                            stringValue = card.type
                        }
                    }
                };
                CardArrayValues cardArrayValues = new CardArrayValues()
                {
                    mapValue = cardMapApiStruct
                };
                handApiStruct.fields.cards.arrayValue.values[i] = cardArrayValues;
            }
            
            
            return handApiStruct;
        }
    }
    
    [Serializable]
    public class HandWrapperStruct
    {
        public StringApiField playerId;
        public CardApiStruct cards;
    }
    
    [Serializable]
    public class CardApiStruct
    {
        public CardArray arrayValue;
    }
    
    [Serializable]
    public class CardArray
    {
     public   CardArrayValues[] values;
    }
    
    [Serializable]
    public class CardArrayValues
    {
       public  CardMapApiStruct mapValue;
    }
    
    [Serializable]
    public class CardMapApiStruct
    {
        public CardApiFields fields;
    }
    
    [Serializable]
    public class CardApiFields
    {
        public StringIntegerField id;
        public StringApiField type;
    }
}