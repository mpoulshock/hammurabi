// Copyright (c) 2012 Hammura.bi LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Hammurabi;
using System;
using System.Collections.Generic;

/*
 * This component serves as a middle layer between functional legal rules and an
 * interactive interview.  Its purpose is to assess what facts are known and what 
 * facts still need to be obtained from the user, and to serve back objects that
 * embody the next screen that should be displayed.  Various types of presentation
 * layers can be built on top of this.
 * 
 * Inputs:
 *  - Goals
 *  - Assertions (via the FactBase)
 *  
 * Outputs:
 *  - Whether another question needs to be asked
 *  - What the next question is (this is a factlet: subj-rel-obj)
 *  - Progress percentage
 */

namespace Interactive
{
    /// <summary>
    /// Assess Hammurabi conclusions and determines what facts remain unknown.
    /// </summary>
    public partial class Engine
    {
        /// <summary>
        /// Response object (output from the Engine).
        /// </summary>
        public class Response
        {
            public bool InvestigationComplete;
            public Facts.Factlet NextFact;
            public int PercentComplete;
            public List<GoalBlob> Goals;

            /// <summary>
            /// General response constructor.
            /// </summary>
            public Response(bool done, Facts.Factlet next, int percent, List<GoalBlob> goals)
            {
                InvestigationComplete = done;
                NextFact = next;
                PercentComplete = percent;
                Goals = goals;
            }
        }

        /// <summary>
        /// Asks the next question or displays the interview results.
        /// </summary>  
        public static Response Investigate(List<GoalBlob> goals)
        {
            // Default outputs
            bool allDone = true;
            int percent = 0;
            Facts.Factlet theNextFact = new Facts.Factlet("", null, null, null);
            
            // Prepare to look for unknown facts
            Facts.GetUnknowns = true;
            Facts.Unknowns.Clear();
            
            // Iterate through each goal
            foreach (GoalBlob g in goals)
            {
                if (!g.Value().HasBeenDetermined)
                {
                    allDone = false;
                }
            }
            Facts.GetUnknowns = false;

            // Determine the next fact and the percent complete
            if (!allDone)
            {
                theNextFact = Facts.Unknowns[0];
                percent = ProgressPercentage(Facts.Count(), Facts.Unknowns.Count);
            }
            
            return new Engine.Response(allDone, theNextFact, percent, goals);
        }

        /// <summary>
        /// Calculates how much of the interview has been completed.
        /// </summary>
        private static int ProgressPercentage(double answered, double unknown)
        {
            double percent = (answered / (answered + unknown)) * 100;
            return Convert.ToInt32(Math.Round(percent, 0));
        }
    }

    /// <summary>
    /// Bundle of things that are passed into the engine, representing a goal.
    /// </summary>
    public class GoalBlob
    {
        public string Relationship;
        public Thing Thing1;
        public Thing Thing2;
        public Thing Thing3;

        public GoalBlob(string relationship, Thing thing1, Thing thing2, Thing thing3)
        {
            Relationship = relationship;
            Thing1 = thing1;
            Thing2 = thing2;
            Thing3 = thing3;
        }

        public GoalBlob(string relationship, string thing1, string thing2, string thing3)
        {
            Relationship = relationship;
            Thing1 = Facts.AddThing(thing1);
            Thing2 = Facts.AddThing(thing2);
            Thing3 = Facts.AddThing(thing3);
        }

        public Tvar Value()
        {
            // Consider implementing caching
            return this.GetFunction().Invoke();
        }

        public Func<Tvar> GetFunction()
        {
            // Set the goal's Things
            Engine.Thing1 = Facts.AddThing(Thing1);
            Engine.Thing2 = Facts.AddThing(Thing2);
            Engine.Thing3 = Facts.AddThing(Thing3);

            // Get the lambda function
            return Interactive.Templates.GetQ(Relationship).theFunc;
        }

        public string ValueAsString()
        {
            return this.QuestionText() + " " + Value().TestOutput;
        }

        public string QuestionText()
        {
            // Embed the names of the Things into the question
            string result = Interactive.Templates.GetQ(Relationship).questionText;

            result = result.Replace("{1}", Thing1.Id);
            result = result.Replace("{2}", Thing2.Id);
            result = result.Replace("{3}", Thing3.Id);
            
            return result;
        }
    }
}