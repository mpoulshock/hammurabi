using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HammurabiWebService
{
    [DataContract]
    public class Goal
    {
        [DataMember]
        public Factlet Factlet;

        [DataMember]
        public DateTime AsOfDate;
    }

    [DataContract]
    public class Fact
    {
        [DataMember]
        public Factlet Factlet;

        [DataMember]
        public Tvar TemporalValue;
    }

    [DataContract]
    public class Factlet
    {
        [DataMember]
        public string Relationship;

        [DataMember]
        public string Thing1;

        [DataMember]
        public string Thing2;

        [DataMember]
        public string Thing3;
    }

    [DataContract]
    public class Tvar
    {
        [DataMember]
        public SortedList<DateTime?, string> TimeLine;  // SortedList?
    }
}
