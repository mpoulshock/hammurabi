//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//
//namespace HammurabiWebService
//{
//    // Hammurabi web service response object
//
//    [DataContract]
//    public class Response
//    {
//        // Calculated value of the goals sought by the user
//        [DataMember]
//        public List<RequestedGoal> RequestedGoals;
//
//        // Unknown facts relevant to the goals
//        [DataMember]
//        public List<NeededFact> NeededFacts;
//
//        // Percentage complete
//        [DataMember]
//        public int PercentageComplete;
//
//        // Constructor
//        public Response(List<RequestedGoal> requestedGoals, List<NeededFact> neededFacts, int percentageComplete)
//        {
//            RequestedGoals = requestedGoals;
//            NeededFacts = neededFacts;
//            PercentageComplete = percentageComplete;
//        }
//    }
//
//    [DataContract]
//    public class RequestedGoal
//    {
//        [DataMember]
//        public Factlet Factlet;
//
//        [DataMember]
//        public Tvar TemporalValue;
//
//        [DataMember]
//        public DateTime? AsOf;
//
//        [DataMember]
//        public bool IsDetermined;
//
//        // Constructor
//        public RequestedGoal(Factlet factlet, Tvar temporalValue, bool isDetermined)
//        {
//            Factlet = factlet;
//            TemporalValue = temporalValue;
//            IsDetermined = isDetermined;
//        }
//    }
//
//    [DataContract]
//    public class NeededFact
//    {
//        [DataMember]
//        public Factlet Factlet;
//
//        [DataMember]
//        public string QuestionType;
//
//        [DataMember]
//        public string QuestionText;
//
//        [DataMember]
//        public string HelpText;
//
//        // Data validation constraints?
//
//        // Constructor
//        public NeededFact(Factlet factlet, string questionType, string questionText, string helpText)
//        {
//            Factlet = factlet;
//            QuestionType = questionType;
//            QuestionText = questionText;
//            HelpText = helpText;
//        }
//    }
//}
