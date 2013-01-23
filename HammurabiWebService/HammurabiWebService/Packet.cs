using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HammurabiWebService
{
    // Hammurabi web service request object

    [DataContract]
    public class Packet
    {
        // Web service response time (in milliseconds)
        [DataMember]
        public decimal ResponseTimeInMs;

        // Goals, or answers, sought by the user
        [DataMember]
        public List<Fact> Goals;

        // Facts asserted by the user
        [DataMember]
        public List<Fact> AssertedFacts;

        // Facts still needed in order to solve goals
        [DataMember]
        public List<Fact> NeededFacts;

        // When true, the web service parrots back the facts in the response
        [DataMember]
        public bool Echo = false;

        // Level of detail, in the web service response, about the unknown facts (Concise, Top, Screen, All)
        [DataMember]
        public string UnknownFactsLevelOfDetail = "Concise";

        // Interview progress
        [DataMember]
        public int PercentageComplete;

        public Packet()
        {
        }

        // Constructor
        public Packet(List<Fact> goals, List<Fact> assertedFacts)
        {
            Goals = goals;
            AssertedFacts = assertedFacts;
        }
    }

    [DataContract]
    public class Fact
    {
        [DataMember]
        public string Relationship;
        
        [DataMember]
        public string Thing1;
        
        [DataMember]
        public string Thing2;
        
        [DataMember]
        public string Thing3;

        [DataMember]
        public List<TemporalValue> Timeline;

        [DataMember]
        public DateTime AsOfDate;

        [DataMember]
        public bool IsDetermined;

        [DataMember]
        public string FactType;
        
        [DataMember]
        public string QuestionText;
        
        [DataMember]
        public string HelpText;

        public Fact()
        {
        }

        public Fact(string relationship, string thing1, string thing2, string thing3)
        {
            Relationship = relationship;
            Thing1 = thing1;
            Thing2 = thing2;
            Thing3 = thing3;
        }

        public Fact(string relationship, string thing1, string thing2, string thing3, List<TemporalValue> timeline)
        {
            Relationship = relationship;
            Thing1 = thing1;
            Thing2 = thing2;
            Thing3 = thing3;
            Timeline = timeline;
        }
    }

    [DataContract]
    public class TemporalValue
    {
        [DataMember]
        public DateTime Date;

        [DataMember]
        public object Value;
        
        // Constructor (eternal value)
        public TemporalValue(string value)
        {
            Date = new DateTime(1800,1,1);
            Value = value;
        }
        
        // Constructor
        public TemporalValue(DateTime date, object value)
        {
            Date = date;
            Value = value;
        }
    }
}