using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HService
{
    // Hammurabi web service response object

    [DataContract]
    public class HammurabiResponse
    {
        // Calculated value of the goals sought by the user
        [DataMember]
        public List<RequestedGoal> RequestedGoals;

        // Unknown facts relevant to the goals
        [DataMember]
        public List<NeededFact> NeededFacts;

        // Percentage complete
        [DataMember]
        public int PercentageComplete;


        // Constructs the web service response
        public HammurabiResponse(int result)
        {
            PercentageComplete = result;
        }
    }

    [DataContract]
    public class RequestedGoal
    {
        [DataMember]
        public Factlet Factlet;

        [DataMember]
        public Tvar TemporalValue;

        [DataMember]
        public DateTime? AsOf;

        [DataMember]
        public bool IsDetermined;
    }

    [DataContract]
    public class NeededFact
    {
        [DataMember]
        public Factlet Factlet;

        [DataMember]
        public string QuestionType;

        [DataMember]
        public string QuestionText;

        [DataMember]
        public string HelpText;

        // Data validation constraints?
    }
}
