using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hammurabi;
using Interactive;

namespace Akkadian.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class WebService : IWebService
    {
        // Equivalent of "hello, world"
        public Packet AssessTest()
        {
            // Assert facts and combine them into a list
            TemporalValue v1 = new TemporalValue("Single");
            Factoid fact1 = new Factoid("Tstr","USC.Tit26.Sec2.FedTaxFilingStatus","Jim","","",new List<TemporalValue>(){v1});
            List<Factoid> facts = new List<Factoid>(){fact1};

            // Define goals to be sought, and combine them into a list
            Factoid goal1 = new Factoid("Tnum", "IRS.Pub501.StandardDeduction", "Jim", "", "");
//            Factoid goal1 = new Factoid("Tnum","USC.Tit26.Sec151.ThresholdAmount","Jim","","");
//            Factoid goal2 = new Factoid("Tnum","USC.Tit26.Sec151.ApplicablePercentage","Jim","","");
//            Factoid goal3 = new Factoid("Tbool","USC.Tit26.Sec152.IsDependentOf","Kid","Dad","");  // For some reason, this is very slow...
            List<Factoid> goals = new List<Factoid>(){goal1};

            // Assemble a mock request to the web service
            Packet mockPacket = new Packet(goals, facts);

            // Assess the request
            return Assess(mockPacket);
        }

        /// <summary>
        /// Takes an income packet and instantiates a Hammurabi session.
        /// </summary>
        public Packet Assess(Packet request)
        {
            // Start timer 
            DateTime startTime = DateTime.Now;

            // Pre-evaluate each goal to enable look-ahead short circuiting.
            // See Hammurabi | Core | Engine.cs, line ~81, for an explanation.
            foreach (Factoid g in request.Goals)
            {
                Facts.Fact gb = new Facts.Fact(g.Relationship, g.Arg1, g.Arg2, g.Arg3);
                gb.Value();
            }

            // Start a fresh session
//            Facts.Clear();
            Facts.GetUnknowns = true;
            Facts.Unknowns.Clear();
            bool allDone = true;
            request.PercentageComplete = 100;
            
            // Assert facts into a Hammurabi session
            foreach(Factoid f in request.AssertedFacts)
            {
                AssertFact(f); 
            }

            // Iterate through each goal
            foreach(Factoid g in request.Goals)
            {
                Facts.Fact gb = new Facts.Fact(g.Relationship, g.Arg1, g.Arg2, g.Arg3);

                // Assign to a variable so it's only evaluated once
                Tvar gbVal = gb.Value();

                // Convert Tvar to timeline object
                g.Timeline = TvarToTimeline(gbVal);

                // All goals resolved?
                if (!gbVal.HasBeenDetermined)
                {
                    allDone = false;
                }
            }

            // Stop looking for unknown facts
            Facts.GetUnknowns = false;

            // Determine the next fact and the percent complete
            if (!allDone)
            {
//                Factoid neededFact = new Factoid("Tnum","USC.Tit26.Sec151.ThresholdAmount","Jim","","");
//                Facts.Fact f = new Facts.Fact("USC.Tit26.Sec151.ThresholdAmount", null, null, null);
//                Factoid neededFact = new Factoid(f);

                Factoid neededFact = new Factoid(Facts.Unknowns[0]);
                request.NeededFacts = new List<Factoid>(){neededFact}; 

                request.PercentageComplete = Interactive.Engine.ProgressPercentage(Facts.Count(), Facts.Unknowns.Count);
            }

            // Add elapsed time to response object
            request.ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);

            return request;
        }
         
        /// <summary>
        /// Converts Tvar object to Timeline object.
        /// </summary>
        private static List<TemporalValue> TvarToTimeline(Hammurabi.Tvar tv)
        {
            List<TemporalValue> result = new List<TemporalValue>();
            
            for (int i=0; i < tv.IntervalValues.Count; i++ ) 
            {
                // TODO: This only handles strings right now...(b/c Unstated is an unrecognized object)
                result.Add(new TemporalValue(tv.IntervalValues.Keys[i], tv.IntervalValues.Values[i].Val.ToString()));
            }
            return result;
        }

        /// <summary>
        /// Asserts a given fact (of the proper Tvar type)
        /// </summary>
        private static void AssertFact(Factoid f)
        {
            // Instantiate relevant Things
            Thing t1 = f.Arg1.ToString() != "" ? Facts.AddThing(f.Arg1.ToString()) : null;
            Thing t2 = f.Arg2.ToString() != "" ? Facts.AddThing(f.Arg2.ToString()) : null;
            Thing t3 = f.Arg3.ToString() != "" ? Facts.AddThing(f.Arg3.ToString()) : null;

            // Sometimes I have my doubts about static typing...
            if (f.FactType == "Tbool")
            {
                Tbool val = new Tbool();  
                foreach (TemporalValue v in f.Timeline)
                {
                    val.AddState(v.Date, new Hval(v.Value));
                }
                Facts.Assert(t1, f.Relationship, t2, t3, val);
            } 
            else if (f.FactType == "Tnum")
            {
                Tnum val = new Tnum();  
                foreach (TemporalValue v in f.Timeline)
                {
                    val.AddState(v.Date, new Hval(v.Value));
                }
                Facts.Assert(t1, f.Relationship, t2, t3, val);
            }
            else if (f.FactType == "Tstr")
            {
                Tstr val = new Tstr();  
                foreach (TemporalValue v in f.Timeline)
                {
                    val.AddState(v.Date, new Hval(v.Value));
                }
                Facts.Assert(t1, f.Relationship, t2, t3, val);
            }
            else if (f.FactType == "Tdate")
            {
                Tdate val = new Tdate();  
                foreach (TemporalValue v in f.Timeline)
                {
                    val.AddState(v.Date, new Hval(v.Value));
                }
                Facts.Assert(t1, f.Relationship, t2, t3, val);
            }
            else if (f.FactType == "Tset")
            {
                Tset val = new Tset();  
                foreach (TemporalValue v in f.Timeline)
                {
                    val.AddState(v.Date, new Hval(v.Value));
                }
                Facts.Assert(t1, f.Relationship, t2, t3, val);
            }
        }
    }
}