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
    public partial class H
    {
        /// <summary>
        /// Structure that conveys information about whether a rule should short-circuit (see below).
        /// </summary>
        public class RulePreCheckResponse
        {
            public Tvar val;
            public bool shouldShortCircuit;

            public RulePreCheckResponse(Tvar theTvar, bool shortCircuit)
            {
                val = theTvar;
                shouldShortCircuit = shortCircuit;
            }
        }

        /// <summary>
        /// Determines whether the rule should short-circuit before firing the actual rule logic.
        /// </summary>
        public static RulePreCheckResponse ShortCircuitValue<T>(string rel, string symIndicator, string questionIndicator, params object[] argList) where T : Tvar
        {
            // First, determine whether any of the Things are unknown.
            Hstate h = EntityArgIsUnknown(argList);
            if (h != Hstate.Known) 
            {
                T scv = (T)Auxiliary.ReturnProperTvar<T>(new Hval(null, h));
                return new RulePreCheckResponse(scv, true);
            }

            // Extract argument parameters from argList
            object param1 = argList[0];
            object param2 = argList.Length > 1 ? argList[1] : null;
            object param3 = argList.Length > 2 ? argList[2] : null;

            // Handle symmetrical facts
            if (symIndicator == "Sym")
            {
                // If the fact has not been asserted, add it to the list of unknown facts, so it gets asked.
                if (!Facts.HasBeenAssertedSym(param1, rel, param2))
                {
                    // EXPERIMENTAL: First, ask any assumed facts 
                    if (Facts.GetUnknowns)
                    {
//                        if (Facts.Sym(param1, rel, param2).IsEternallyUncertain)
//                        {
//                            Assumptions.AskAssumedFacts(rel, param1, param2, param3);
//                            Assumptions.AskAssumedFacts(rel, param2, param1, param3);
//                        }

                        // "?" indicates that this node is a question to be asked of the user
                        if (questionIndicator == "?")
                        {
                            Facts.AddUnknown(rel, param1, param2, null);
                        }
                    }
                }

                // If the fact has been asserted and is not "uncertain", return the asserted value.
                else
                {
                    Tbool f = Facts.Sym(param1, rel, param2);
                    if (!f.IsEternallyUncertain)
                    {
                        return new RulePreCheckResponse(f, true);
                    }
                }
            }

            // Handle non-symmetrical facts (same code pattern as above)
            else
            {
                if (!Facts.HasBeenAsserted(rel, param1, param2, param3))
                {
                     
                    if (Facts.GetUnknowns)
                    {
                        // EXPERIMENTAL: First, ask any assumed facts
//                        Assumptions.AskAssumedFacts(rel, param1, param2, param3, false);
                    
                        if (questionIndicator == "?")
                        {
                            Facts.AddUnknown(rel, param1, param2, param3);
                        }
                    }
                    
                }
                else
                {
                    T f = Facts.QueryTvar<T>(rel, param1, param2, param3);
                    if (!f.IsEternallyUncertain)
                    {
                        return new RulePreCheckResponse(f, true);
                    }
                }
            }

            // Otherwise, proceed to examine the rule conditions and sub-rules.
            return new RulePreCheckResponse(null, false);
        }

        /// <summary>
        /// Determines whether any of the input objects are unknown Things.
        /// </summary>
        /// <remarks>
        /// Has to handle objects because some arguments might be things
        /// other than Things.  We only care aboue whether the Things are 
        /// unknown.
        /// </remarks>
        public static Hstate EntityArgIsUnknown(params object[] list)
        {
            bool hasUnstated = false;
            bool hasUncertain = false;
            bool hasStub = false;

            foreach (object e in list)
            {
                if (e.GetType() == new Thing("").GetType())
                {
                    string id = ((Thing)e).Id;
                    if (id == "#Unstated#" || id == "") hasUnstated = true;
                    else if (id == "#Uncertain#") hasUncertain = true;
                    else if (id == "#Stub#") hasStub = true;
                }
            }

            if (hasUnstated) return Hstate.Unstated;
            else if (hasStub) return Hstate.Stub;
            else if (hasUncertain) return Hstate.Uncertain;
            return Hstate.Known;
        }

        /// <summary>
        /// Given a group of boolean Hvals, returns Hstate that trumps the others.
        /// This order is different than that of the function below because with ANDs
        /// and ORs we always want to try to prove (OR) or falsify (AND) the consequent.
        /// </summary>       
        public static Hstate PrecedingStateForLogic(List<Hval> inputs) 
        {
            Hval[] list = Auxiliary.ListToArray<Hval>(inputs);

            // If one fact is unstated, we want to continue trying to prove (OR) or 
            // falsify (AND) the conclusion of the rule.
            if (AnyHvalsAre(Hstate.Unstated, list)) return Hstate.Unstated;

            // Uncertain trumps Stub because if the user were able to answer the question,
            // Hammurabi could possibly provide a determination.
            if (AnyHvalsAre(Hstate.Uncertain, list)) return Hstate.Uncertain;

            // Else, stub...
            if (AnyHvalsAre(Hstate.Stub, list)) return Hstate.Stub;

            return Hstate.Known;
        }

        /// <summary>
        /// Given a group of Hvals, returns Hstate that trumps the others.
        /// </summary>       
        public static Hstate PrecedingState(List<Hval> list) 
        {
            return PrecedingState(Auxiliary.ListToArray<Hval>(list));
        }
        protected static Hstate PrecedingState(params Hval[] list) 
        {
            // Where there is a stub, there's no need to consider uncertain or unstated facts
            if (AnyHvalsAre(Hstate.Stub, list)) return Hstate.Stub;

            // Where a fact is uncertain, there's no need to query for more information
            if (AnyHvalsAre(Hstate.Uncertain, list)) return Hstate.Uncertain;

            // If one fact is known and the other unstated, the conclusion can't be reached 
            // and is unstated
            if (AnyHvalsAre(Hstate.Unstated, list)) return Hstate.Unstated;
            
            return Hstate.Known;
        }

        /// <summary>
        /// Returns true if any of the input Hvals are unstated.
        /// </summary>       
        protected static bool AnyHvalsAre(Hstate state, params Hval[] list) 
        {
            foreach (Hval h in list)
            {
                if (h.State == state) return true;
            }
            
            return false;
        }
    }
}