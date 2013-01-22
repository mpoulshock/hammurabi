//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//
//namespace HammurabiWebService
//{
//    // Hammurabi web service request object
//
//    [DataContract]
//    public class Request
//    {
//        // Goals, or answers, sought by the user
//        [DataMember]
//        public List<Goal> Goals;
//
//        // Facts asserted by the user
//        [DataMember]
//        public List<Fact> Facts;
//
//        // When true, the web service parrots back the facts in the response
//        [DataMember]
//        public bool Echo = false;
//
//        // Level of detail, in the web service response, about the unknown facts (Concise, Top, Screen, All)
//        [DataMember]
//        public string UnknownFactsLevelOfDetail = "Concise";
//
//        // Constructor
//        public Request(List<Goal> goals, List<Fact> facts)
//        {
//            Goals = goals;
//            Facts = facts;
//        }
//    }
//}
