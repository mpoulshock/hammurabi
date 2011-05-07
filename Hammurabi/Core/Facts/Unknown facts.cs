// Copyright (c) 2011 The Hammurabi Project
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
    public static partial class Facts
    {
        /// <summary>
        /// List of facts that are unknown and needed to resolve a goal (method)
        /// that has been called.
        /// </summary>
        public static List<Factlet> Unknowns = new List<Factlet>();
        
        /// <summary>
        /// When true, allows facts to be added to UnknownFacts.
        /// Needs to be false by default or UnknownFacts would devour all memory.
        /// </summary>
        public static bool GetUnknowns = false;

        /// <summary>
        /// Turns on the mode in which unknowns are collected into the
        /// Facts.Unknowns list.
        /// </summary>
        //  TODO: Delete?
        public static void UnknownModeOn()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
        }
        
        /// <summary>
        /// Add a (two-entity) factlet to UnknownFacts.
        /// </summary>
        public static void AddUnknown(LegalEntity e1, string rel, LegalEntity e2)
        {
            // Keep list from devouring the entire universe
            if (Unknowns.Count < 500)
            {
                // Ignore duplicates
                if (!IsUnknown(e1,rel,e2))
                { 
                    Unknowns.Add(new Factlet(e1,rel,e2));
                }
            }
        }
        
        /// <summary>
        /// Add a (one-entity) factlet to Facts.Unknowns.
        /// </summary>
        public static void AddUnknown(LegalEntity e1, string rel)
        {
            AddUnknown(e1, rel, null);
        }
        
        /// <summary>
        /// Indicates whether Facts.Unknowns contains a given factlet.
        /// </summary>
        /// <remarks>
        /// Note that this is distinct from whether a fact HasBeenAsserted.
        /// </remarks>
        public static bool IsUnknown(LegalEntity e1, string rel, LegalEntity e2)
        {
            foreach (Factlet t in Unknowns)
            {
                if (t.subject == e1 &&
                    t.relationship == rel &&
                    t.directObject == e2)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Returns a string showing all factlets in Facts.Unknowns.
        /// </summary>
        public static string ShowUnknowns()
        {
            string result = "";
            
            foreach (Facts.Factlet f in Facts.Unknowns)
            {
                if (f.directObject != null)
                {
                    result += f.subject.Id + " " + f.relationship + " " + f.directObject.Id + "\n";
                }
                else
                {
                    result += f.subject.Id + " " + f.relationship + "\n";
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Returns a string showing all relationships in Facts.Unknowns.
        /// Used to test the order in which factlets are added to that list.
        /// </summary>
        public static string ShowUnknownTest()
        {
            string result = "";
            
            foreach (Facts.Factlet f in Facts.Unknowns)
            {
                result += f.relationship + " ";
            }
            
            return result.Trim();
        }
        
        /// <summary>
        /// A Factlet object represents a fact that has no value because it
        /// is needed (unknown).
        /// </summary>
        public class Factlet
        {
            public LegalEntity subject;
            public string relationship;
            public LegalEntity directObject;
            
            /// <summary>
            /// Factlet that relates to one legal entity.
            /// </summary>
            public Factlet(LegalEntity subj, string rel)
            {
                subject = subj;
                relationship = rel;
            }
            
            /// <summary>
            /// Factlet that relates to two legal entities.  
            /// </summary>
            public Factlet(LegalEntity subj, string rel, LegalEntity obj)
            {
                subject = subj;
                relationship = rel;
                directObject = obj;
            }
        }
    }
}
        

