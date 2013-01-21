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
            List<Func<Hammurabi.Tvar>> goals = new List<Func<Hammurabi.Tvar>>();
            foreach(Goal g in request.Goals)
            {
                // Set the goal's Things
                Engine.Thing1 = Facts.AddThing(g.Factlet.Thing1);
                Engine.Thing2 = Facts.AddThing(g.Factlet.Thing2);
                Engine.Thing3 = Facts.AddThing(g.Factlet.Thing3);

                // Get the goal's template function and load it onto the goals list
                Func<Hammurabi.Tvar> seedGoal = Interactive.Templates.GetQ(g.Factlet.Relationship).theFunc;
                goals.Add(seedGoal);
            }

            // Invoke Hammurabi's determination
            Engine.Response response = Engine.Investigate(goals);

            // Convert engine response into web service response

            // Get values of goals
            List<RequestedGoal> requestedGoals = new List<RequestedGoal>();
            foreach (Func<Hammurabi.Tvar> g in goals)
            {
                RequestedGoal rg = new RequestedGoal(new Factlet("ThresholdAmount","Jim"), new Tvar(g.Invoke().TestOutput),true);
                requestedGoals.Add(rg);
            }

            // Return the response object
            return new Response(requestedGoals, new List<NeededFact>(), response.PercentComplete);
        }
    }
}
