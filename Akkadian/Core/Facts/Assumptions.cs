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

using System;

namespace Akkadian
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
     * 
     * This table-scanning concept will not scale and will someday have to be reinvented.
     */

    /// <summary>
    /// Handles inferences generated when one fact is known to assume another fact.
    /// </summary>
    public partial class Assumptions
    {
        /// <summary>
        /// Pair of nodes composing an assumption.
        /// </summary>
        public class Pair
        {
            public Point LeftHandPoint {get; set;} 
            public Point RightHandPoint {get; set;} 

            public Pair(Point leftP, Point rightP)
            {
                LeftHandPoint = leftP;
                RightHandPoint = rightP;
            }
        }

        /// <summary>
        /// One node of an assumption.
        /// </summary>
        public class Point
        {
            public string Relationship;
            public int Arg1, Arg2, Arg3;
            public Tvar Value;

            public Point(string rel, int arg1, int arg2, int arg3, Tvar val)
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
            foreach (Pair p in Pairs)
            {
                // If A, then B
                if (p.LeftHandPoint.Relationship == rel)
                {
                    // TODO: Currently only handles expressions that are eternally true
                    if (Tvar.EqualTo(p.LeftHandPoint.Value, val))
                    {
                        // For each rightPoint.Arg number, get the corresponding Thing
                        Thing[] args = new Thing[3]{e1,e2,e3};

                        int a1 = p.RightHandPoint.Arg1 - 1;  // -1 b/c array is base-zero
                        int a2 = p.RightHandPoint.Arg2 - 1;
                        int a3 = p.RightHandPoint.Arg3 - 1;

                        Thing t1 = a1 >= 0 ? args[a1] : null;
                        Thing t2 = a2 >= 0 ? args[a2] : null;
                        Thing t3 = a3 >= 0 ? args[a3] : null;

                        Facts.Assert(t1, p.RightHandPoint.Relationship, t2, t3, p.RightHandPoint.Value);
                    }
                }

                // If -B, then -A
                else if (p.RightHandPoint.Relationship == rel)
                {
                    // If right-hand expression is always false...
                    if (Tvar.EqualTo(p.RightHandPoint.Value, val).IsFalse)
                    {
                        // If the left-hand side is a boolean (non-booleans can't be negated)...
                        if (Tvar.EqualTo(p.LeftHandPoint.Value, new Tbool(true)) ||
                            Tvar.EqualTo(p.LeftHandPoint.Value, new Tbool(false)))
                        {
                            // For each leftPoint.Arg number, get the corresponding Thing
                            int r1 = p.RightHandPoint.Arg1;
                            int r2 = p.RightHandPoint.Arg2;
                            int r3 = p.RightHandPoint.Arg3;

                            // I hope no one sees how ugly this is
                            Thing t1, t2, t3;
                            if (r1 == 1)      t1 = e1;
                            else if (r2 == 1) t1 = e2;
                            else              t1 = e3;

                            if (r1 == 2)      t2 = e1;
                            else if (r2 == 2) t2 = e2;
                            else              t2 = e3;

                            if (r1 == 3)      t3 = e1;
                            else if (r2 == 3) t3 = e2;
                            else              t3 = e3;

                            // Assert -A
                            Tbool leftVal = (Tbool)p.LeftHandPoint.Value;
                            Facts.Assert(t1, p.LeftHandPoint.Relationship, t2, t3, !leftVal);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Scans the assumption table, looking for assumptions to add to the Facts.Unknowns list.
        /// </summary>
        public static void AskAssumedFacts(string rel, Thing e1, Thing e2, Thing e3)
        {
            foreach (Pair p in Pairs)
            {
                // If A, then B
                if (p.LeftHandPoint.Relationship == rel)
                {
                    // For each rightPoint.Arg number, get the corresponding Thing
                    Thing[] args = new Thing[3]{e1,e2,e3};
                    
                    int a1 = p.RightHandPoint.Arg1 - 1;  // -1 b/c array is base-zero
                    int a2 = p.RightHandPoint.Arg2 - 1;
                    int a3 = p.RightHandPoint.Arg3 - 1;
                    
                    Thing t1 = a1 >= 0 ? args[a1] : null;
                    Thing t2 = a2 >= 0 ? args[a2] : null;
                    Thing t3 = a3 >= 0 ? args[a3] : null;

                    // Investigates all assumptions
                    // TODO: Does not short circuit
                    Facts.Fact f = new Facts.Fact(p.RightHandPoint.Relationship, t1.Id, t2.Id, t3.Id);
                    f.Value();
                }
            }
        }
    }
}