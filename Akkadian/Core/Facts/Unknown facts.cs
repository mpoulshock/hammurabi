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
using System.Collections.Generic;

namespace Akkadian
{
    public partial class Facts
    {
        /// <summary>
        /// List of facts that are unknown and needed to resolve a goal (method)
        /// that has been called.
        /// </summary>
        public static List<Fact> Unknowns = new List<Fact>();
        
        /// <summary>
        /// When true, allows facts to be added to UnknownFacts.
        /// Needs to be false by default or UnknownFacts would devour all memory.
        /// </summary>
        public static bool GetUnknowns = false;

        /// <summary>
        /// Add a factlet to UnknownFacts.
        /// </summary>
        public static void AddUnknown(string rel, object e1, object e2, object e3)
        {
            // Keep list from devouring the entire universe
            if (Unknowns.Count < 500)
            {
                // Ignore duplicates
                if (!IsUnknown(rel, e1, e2, e3))
                { 
                    Unknowns.Add(new Fact(rel, e1, e2, e3));
                }
            }
        }

        /// <summary>
        /// Indicates whether Facts.Unknowns contains a given fact.
        /// </summary>
        /// <remarks>
        /// Note that this is distinct from whether a fact HasBeenAsserted.
        /// </remarks>
        public static bool IsUnknown(string rel, object e1, object e2, object e3)
        {
            foreach (Fact t in Unknowns)
            {
                if (t.Relationship == rel && t.Arg1 == e1 && t.Arg2 == e2 && t.Arg3 == e3)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Returns a string showing all facts in Facts.Unknowns.
        /// </summary>
        public static string ShowUnknowns()
        {
            string result = "";
            
            foreach (Facts.Fact f in Facts.Unknowns)
            {
                result += f.Relationship + " " + f.Arg1 + " " + f.Arg2 + " " + f.Arg3;
            }
            
            return result;
        }
    }
}