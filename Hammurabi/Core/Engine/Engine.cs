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
            public Facts.Fact NextFact;
            public int PercentComplete;
            public List<GoalBlob> Goals;

            /// <summary>
            /// General response constructor.
            /// </summary>
            public Response(bool done, Facts.Fact next, int percent, List<GoalBlob> goals)
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
            Facts.Fact theNextFact = new Facts.Fact("", null, null, null);
            
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
        public static int ProgressPercentage(double answered, double unknown)
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
        public object Arg1;
        public object Arg2;
        public object Arg3;

        public GoalBlob(string relationship, object arg1, object arg2, object arg3)
        {
            Relationship = relationship;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }

        public Tvar Value()
        {
            // Consider implementing caching
            return this.GetFunction().Invoke();
        }

        public Func<Tvar> GetFunction()
        {
            // Get the template for the function, based on the relationship
            Question q = Interactive.Templates.GetQ(Relationship);

            // Set the function's arguments before invoking it

            // Convert first argument from string to proper type
            if (q.arg1Type == "Thing") Engine.Thing1 = Facts.AddThing(Convert.ToString(Arg1));
            else if (q.arg1Type == "Tbool") Engine.Tbool1 = Convert.ToBoolean(Arg1);
            else if (q.arg1Type == "Tnum")  Engine.Tnum1 = Convert.ToDecimal(Arg1);
            else if (q.arg1Type == "Tstr")  Engine.Tstr1 = Convert.ToString(Arg1);
            else if (q.arg1Type == "Tdate") Engine.Tdate1 = Convert.ToDateTime(Arg1);
            else if (q.arg1Type == "Tset")  Engine.Tset1 = (Tset)Arg1;   // ?

            // Second argument
            if (q.arg2Type == "Thing") Engine.Thing2 = Facts.AddThing(Convert.ToString(Arg2));
            else if (q.arg2Type == "Tbool") Engine.Tbool2 = Convert.ToBoolean(Arg2);
            else if (q.arg2Type == "Tnum")  Engine.Tnum2 = Convert.ToDecimal(Arg2);
            else if (q.arg2Type == "Tstr")  Engine.Tstr2 = Convert.ToString(Arg2);
            else if (q.arg2Type == "Tdate") Engine.Tdate2 = Convert.ToDateTime(Arg2);
            else if (q.arg2Type == "Tset")  Engine.Tset2 = (Tset)Arg2;

            // Third argument
            if (q.arg3Type == "Thing") Engine.Thing3 = Facts.AddThing(Convert.ToString(Arg3));
            else if (q.arg3Type == "Tbool") Engine.Tbool3 = Convert.ToBoolean(Arg3);
            else if (q.arg3Type == "Tnum")  Engine.Tnum3 = Convert.ToDecimal(Arg3);
            else if (q.arg3Type == "Tstr")  Engine.Tstr3 = Convert.ToString(Arg3);
            else if (q.arg3Type == "Tdate") Engine.Tdate3 = Convert.ToDateTime(Arg3);
            else if (q.arg3Type == "Tset")  Engine.Tset3 = (Tset)Arg3;

            // Return the lambda function
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

            return result.Replace("{1}", Convert.ToString(Arg1))
                         .Replace("{2}", Convert.ToString(Arg2))
                         .Replace("{3}", Convert.ToString(Arg3));
        }
    }
}