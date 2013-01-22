//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//
//namespace HammurabiWebService
//{
//    [DataContract]
//    public class Goal
//    {
//        [DataMember]
//        public Factlet Factlet;
//
//        [DataMember]
//        public DateTime AsOfDate;
//
//        // Constructor (no as-of date)
//        public Goal(Factlet factlet)
//        {
//            Factlet = factlet;
//            AsOfDate = new DateTime(1800,1,1);
//        }
//
//        // Constructor
//        public Goal(Factlet factlet, DateTime asOfDate)
//        {
//            Factlet = factlet;
//            AsOfDate = asOfDate;
//        }
//    }
//
//    [DataContract]
//    public class Fact
//    {
//        [DataMember]
//        public Factlet Factlet;
//
//        [DataMember]
//        public Tvar TemporalValue;
//
//        // Constructor
//        public Fact(Factlet factlet, Tvar tvar)
//        {
//            Factlet = factlet;
//            TemporalValue = tvar;
//        }
//    }
//
//    [DataContract]
//    public class Factlet
//    {
//        [DataMember]
//        public string Relationship;
//
//        [DataMember]
//        public string Thing1;
//
//        [DataMember]
//        public string Thing2;
//
//        [DataMember]
//        public string Thing3;
//
//        // Constructor
//        public Factlet(string relationship, string thing1)
//        {
//            Relationship = relationship;
//            Thing1 = thing1;
//        }
//
//        // Constructor
//        public Factlet(string relationship, string thing1, string thing2)
//        {
//            Relationship = relationship;
//            Thing1 = thing1;
//            Thing2 = thing2;
//        }
//
//        // Constructor
//        public Factlet(string relationship, string thing1, string thing2, string thing3)
//        {
//            Relationship = relationship;
//            Thing1 = thing1;
//            Thing2 = thing2;
//            Thing3 = thing3;
//        }
//    }
//
//    [DataContract]
//    public class Tvar
//    {
//        [DataMember]
//        public List<DateValuePair> TimeLine;
//
//        // Constructor
//        public Tvar()
//        {
//            TimeLine = new List<DateValuePair>();
//        }
//
//        // Constructor (eternal value)
//        public Tvar(string s)
//        {
//            TimeLine = new List<DateValuePair>();
//            TimeLine.Add(new DateValuePair(s));
//        }
//    }
//
//    [DataContract]
//    public class DateValuePair
//    {
//        [DataMember]
//        public DateTime Date;
//
//        [DataMember]
//        public string Value2;
//
//        [DataMember]
//        public object Value;   // ???
//
//        // Constructor (eternal value)
//        public DateValuePair(string value)
//        {
//            Date = new DateTime(1800,1,1);
//            Value = value;
//        }
//
//        // Constructor
//        public DateValuePair(DateTime date, string value)
//        {
//            Date = date;
//            Value = value;
//        }
//    }
//}
