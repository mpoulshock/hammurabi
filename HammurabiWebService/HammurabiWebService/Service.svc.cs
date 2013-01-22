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
        public Response AssessTest()
        {
            // This is a mock request object for testing purposes...

            // Assert facts and combine them into a list
            Fact fact1 = new Fact(new Factlet("FedTaxFilingStatus","Jim"), new Tvar("Single"));
            List<Fact> facts = new List<Fact>(){fact1};

            // Define goals to be sought, and combine them into a list
            Goal goal1 = new Goal(new Factlet("ThresholdAmount","Jim"));
            Goal goal2 = new Goal(new Factlet("ApplicablePercentage","Jim"));
            List<Goal> goals = new List<Goal>(){goal1,goal2};

            // Assemble the Request object
            Request request = new Request(goals, facts);

            // Assess the request
            return Assess(request);
        }

        /// <summary>
        /// Takes a Request object and instantiates a Hammurabi session.
        /// </summary>
        public Response Assess(Request request)
        {
            // Start a fresh session
            Facts.Clear();

            // Assert facts into a Hammurabi session
            foreach(Fact f in request.Facts)
            {
                // Determine type and convert

                // Convert TempVal to Tvar
                Tstr tv = new Tstr(Convert.ToString(f.TemporalValue.TimeLine[0].Value));

                // Assert
                Thing t = Facts.AddThing(f.Factlet.Thing1);
                Facts.Assert(t,f.Factlet.Relationship,tv);
            }

            // Assemble goals into goal list and send list to Hammurabi engine
            List<GoalBlob> goalblobs = new List<GoalBlob>();
            foreach(Goal g in request.Goals)
            {
                Thing t1 = Facts.AddThing(g.Factlet.Thing1);
                Thing t2 = Facts.AddThing(g.Factlet.Thing2);
                Thing t3 = Facts.AddThing(g.Factlet.Thing3);

                GoalBlob gb = new GoalBlob(g.Factlet.Relationship, t1, t2, t3);
                goalblobs.Add(gb);
            }

            // Invoke Hammurabi's determination
            Engine.Response response = Engine.Investigate(goalblobs);

            // Convert engine response into web service response

            // Get values of goals
            List<RequestedGoal> requestedGoals = new List<RequestedGoal>();
            foreach (GoalBlob g in response.Goals)
            {
                RequestedGoal rg = new RequestedGoal(new Factlet(g.Relationship, g.Thing1.Id), new Tvar(g.Value().TestOutput), true);
                requestedGoals.Add(rg);
            }

            // Return the response object
            return new Response(requestedGoals, new List<NeededFact>(), response.PercentComplete);
        }
    }
}
