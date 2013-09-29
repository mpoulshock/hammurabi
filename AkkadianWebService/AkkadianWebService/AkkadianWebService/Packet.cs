using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Hammurabi;

namespace Akkadian.Service
{
    // Akkadian web service request object

    [DataContract]
    public class Packet
    {
        // Web service response time (in milliseconds)
        [DataMember (Order=1)]
        public decimal ResponseTimeInMs;
        
        // Interview progress
        [DataMember (Order=2)]
        public int PercentageComplete;

        // Goals, or answers, sought by the user
        [DataMember (Order=3)]
        public List<Factoid> Goals;

        // Facts still needed in order to solve goals
        [DataMember (Order=4)]
        public List<Factoid> NeededFacts;

        // Facts asserted by the user
        [DataMember (Order=5)]
        public List<Factoid> AssertedFacts;

        // When true, the web service parrots back the facts in the response
        [DataMember (Order=6)]
        public bool Echo = false;

        // Level of detail, in the web service response, about the unknown facts (Concise, Top, Screen, All)
        [DataMember (Order=7)]
        public string UnknownFactsLevelOfDetail = "Concise";

        // Constructor
        public Packet(List<Factoid> goals, List<Factoid> assertedFacts)
        {
            Goals = goals;
            AssertedFacts = assertedFacts;
        }
    }

    [DataContract]
    public class Factoid
    {
        [DataMember (Order=1)]
        public string Relationship;

        [DataMember (Order=2)]
        public object Arg1;

        [DataMember (Order=3)]
        public object Arg2;

        [DataMember (Order=4)]
        public object Arg3;

        [DataMember (Order=5)]
        public List<TemporalValue> Timeline;

        [DataMember (Order=6)]
        public DateTime AsOfDate;

        [DataMember (Order=7)]
        public bool IsDetermined;

        [DataMember (Order=8)]
        public string FactType;
        
        [DataMember (Order=9)]
        public string QuestionText;
        
        [DataMember (Order=10)]
        public string HelpText;

        public Factoid()
        {
        }

        public Factoid(string factType, string relationship, object arg1, object arg2, object arg3)
        {
            FactType = factType;
            Relationship = relationship;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }

        public Factoid(string factType, string relationship, object arg1, object arg2, object arg3, List<TemporalValue> timeline)
        {
            FactType = factType;
            Relationship = relationship;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Timeline = timeline;
        }

        public Factoid(Facts.Fact f)
        {
            FactType = f.GetTvarType();
            Relationship = f.Relationship;
            Arg1 = Util.ArgToString(f.Arg1);
            Arg2 = Util.ArgToString(f.Arg2);
            Arg3 = Util.ArgToString(f.Arg3);
            QuestionText = f.QuestionText();
        }

    }

    [DataContract]
    public class TemporalValue
    {
        [DataMember (Order=1)]
        public DateTime Date;

        [DataMember (Order=2)]
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