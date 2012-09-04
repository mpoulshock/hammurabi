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

using System;
using System.Collections.Generic;

namespace Hammurabi
{
    /*
     * Akkadian rules let you specify when one fact assumes another. Examples:
     * 
     *   OnActiveDuty(p) assumes InArmedForces(p)
     *   IsPregnant(p) assumes Gender(p) = "Female"
     *   AreMarried(p,q) assumes IsMarried(p)
     * 
     * A statement like "<A> assumes <B>" allows two inferences to be drawn:
     * 
     *   1. If A, then B.
     *   2. If -B, then -A.
     * 
     * These inferences are distinct from ordinary Akkadian rules.  OnActiveDuty() and
     * InArmedForces() can have 'regular' rules with conditions proving them, and in 
     * addition have assumptions that cause short-circuit inferences, when appropriate.
     * 
     * The assumption relationships are stored in a tabular data structure.  Whenever a
     * new fact is asserted, Hammurabi checks the table to see if any short-circuit 
     * inferences can be made. 
     * 
     * The rules below implement this assumption-checking procedure.
     */

    /// <summary>
    /// Handles inferences generated when one fact is known to assume another fact.
    /// </summary>
    public partial class Assumptions
    {
        /// <summary>
        /// List of all assumptions that have been declared in the rules.
        /// </summary>
        public static List<AssumptionPair> AssumptionPairs;

        /// <summary>
        /// Pair of nodes composing an assumption.
        /// </summary>
        public class AssumptionPair
        {
            public AssumptionPoint LeftHandPoint {get; set;} 
            public AssumptionPoint RightHandPoint {get; set;} 

            public AssumptionPair(AssumptionPoint leftP, AssumptionPoint rightP)
            {
                LeftHandPoint = leftP;
                RightHandPoint = rightP;
            }
        }

        /// <summary>
        /// One node of an assumption.
        /// </summary>
        public class AssumptionPoint
        {
            public string Relationship;
            public int Arg1, Arg2, Arg3;
            public Tvar Value;

            public AssumptionPoint(string rel, int arg1, int arg2, int arg3, Tvar val)
            {
                Relationship = rel;
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
                Value = val;
            }
        }

        /// <summary>
        /// Scans the assumption table, looking for forward-chaining inferences.
        /// </summary>
        public static void TriggerInferences(string rel, Thing e1, Thing e2, Thing e3, Tvar val)
        {
            // First, look to see if fact (f1) is assumed by another true fact (f2)
            // Iterate through assumptions table for f2-f1 pair
//            foreach (AssumptionPair pair in AssumptionPairs)
//            {
//                if (pair.rightHandPoint.relationship == rel)
//                {
//                    // If f2 == true, return f1 == true
//                }
//            }



            // Else, look to see if some other fact (f3) assumes the main fact (f1)
            // Iterate through the assumptions table looking for f1-f3 pair
            // If f3 == false, return f1 == false

        }
    }
}
