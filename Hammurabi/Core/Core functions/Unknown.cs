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
        /// Determines whether any of the input objects are unstated legal entities.
        /// </summary>
        /// <remarks>
        /// Has to handle objects because some arguments might be things
        /// other than LegalEntities.  We only care aboue whether the
        /// LegalEntities are unknown.
        /// </remarks>
        public static bool EntityArgIsUnknown(params object[] list)
        {
            foreach (object e in list)
            {
                // I don't love any of this...actually I detest it.
                if (e == null)
                {
                    return true;
                }
                if (e.GetType() == new Person("").GetType())
                {
                    if (((Person)e).Id == "") return true;
                }
                if (e.GetType() == new Property("").GetType())
                {
                    if (((Property)e).Id == "") return true;
                }
            }

            return false;
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