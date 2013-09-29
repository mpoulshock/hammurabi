// Copyright (c) 2012-2013 Hammura.bi LLC
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

using Akkadian;
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
            public List<Facts.Fact> Goals;

            /// <summary>
            /// General response constructor.
            /// </summary>
            public Response(bool done, Facts.Fact next, int percent, List<Facts.Fact> goals)
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
        public static Response Investigate(List<Facts.Fact> goals)
        {
            // Default outputs
            bool allDone = true;
            int percent = 0;
            Facts.Fact theNextFact = new Facts.Fact("", null, null, null);

            InitializeProofTree();

            // Pre-evaluate each goal in order to cache the results of 
            // each evaluted function in the FactBase.  This will make 
            // look-ahead short-circuiting work in the interview.
            // There will obviously be performance implications to
            // evaluating each goal twice.  However, a four-line fix 
            // to a vexing problem feels delightful.  And the caching
            // may end up making things tolerable after all.
            //
            // But note that the look-ahead short-circuiting issue is not
            // completely solved: it will still fail in large rules where
            // the intermediate conditions are not checked/pre-evaluated.
            foreach (Facts.Fact g in goals)
            {
                g.Value();
            }

//            Console.WriteLine(ShowProofTree());

            // Prepare to look for unknown facts
            Facts.GetUnknowns = true;
            Facts.Unknowns.Clear();
            
            // Iterate through each goal
            foreach (Facts.Fact g in goals)
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
}