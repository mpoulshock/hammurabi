using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hammurabi;
using Interactive;

namespace HammurabiWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class WebService : IWebService
    {
        // Equivalent of "hello, world"
        public Packet AssessTest()
        {
            // Assert facts and combine them into a list
            TemporalValue v1 = new TemporalValue("Single");
            Fact fact1 = new Fact("FedTaxFilingStatus","Jim","","",new List<TemporalValue>(){v1});
            List<Fact> facts = new List<Fact>(){fact1};

            // Define goals to be sought, and combine them into a list
            Fact goal1 = new Fact("ThresholdAmount","Jim","","");
            Fact goal2 = new Fact("ApplicablePercentage","Jim","","");
            Fact goal3 = new Fact("IsDependentOf","Kid","Dad","");
            List<Fact> goals = new List<Fact>(){goal1,goal2,goal3};

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

            // Start a fresh session
            Facts.Clear();
            
            // Assert facts into a Hammurabi session
            foreach(Fact f in request.AssertedFacts)
            {
                // Determine type and convert
                
                // Convert TempVal to Tvar
                Tstr tv = new Tstr(Convert.ToString(f.Timeline[0].Value));
                
                // Assert
                Thing t = Facts.AddThing(f.Thing1);
                Facts.Assert(t, f.Relationship, tv);
            }
            
            // Assemble goals into goal list and send list to Hammurabi engine
            List<GoalBlob> goalblobs = new List<GoalBlob>();
            foreach(Fact g in request.Goals)
            {
                Thing t1 = Facts.AddThing(g.Thing1);
                Thing t2 = Facts.AddThing(g.Thing2);
                Thing t3 = Facts.AddThing(g.Thing3);
                
                GoalBlob gb = new GoalBlob(g.Relationship, t1, t2, t3);
                goalblobs.Add(gb);
            }
            
            // Invoke Hammurabi's determination
            // TODO: Just pass engine Facts
            Engine.Response response = Engine.Investigate(goalblobs);
            
            // Convert engine response into web service response
            
            // Get values of goals
            foreach (GoalBlob g in response.Goals)
            {
                foreach (Fact f in request.Goals)
                {
//                    Tnum tn = new Tnum(8);
//                    tn.AddState(DateTime.Now,12);
//                    f.Timeline = ToTimeline(tn);

                    if (true) //AreEqual(g,f))
                    {
                        f.Timeline = ToTimeline(g.Value());  
                        break;
                    }
                }
            }


            request.PercentageComplete = response.PercentComplete;

            // Add elapsed time to response object
            request.ResponseTimeInMs = Convert.ToDecimal((DateTime.Now - startTime).TotalMilliseconds);

            return request;
        }



        private static bool AreEqual(GoalBlob b, Fact f)
        {
            return b.Relationship == f.Relationship;  // TODO: Enhance
        }

        private static List<TemporalValue> ToTimeline(Hammurabi.Tvar tv)
        {
            List<TemporalValue> result = new List<TemporalValue>();
            for (int i=0; i < tv.IntervalValues.Count; i++ ) 
            {
                result.Add(new TemporalValue(tv.IntervalValues.Keys[i], tv.IntervalValues.Values[i].Val));
            }
            return result;
        }
    }
}
