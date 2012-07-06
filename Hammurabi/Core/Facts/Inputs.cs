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

namespace Hammurabi
{
    public class Input : Facts
    {
        //*********************************************************************
        // Standard inputs
        //*********************************************************************
        
        /// <summary>
        /// Looks for a particular Tbool input.
        /// </summary>
        public static Tbool Tbool(LegalEntity subj, string rel, LegalEntity directObj, Tbool defaultValue)    
        {    
            return (Tbool)QueryTvar<Tbool>(subj, rel, directObj, defaultValue); // experimental
        }
        public static Tbool Tbool(LegalEntity subj, string rel, LegalEntity directObj)    
        {    
            return (Tbool)QueryTvar<Tbool>(subj, rel, directObj);
        }
        public static Tbool Tbool(LegalEntity subj, string rel)
        {    
            return (Tbool)QueryTvar<Tbool>(subj, rel);
        }
        
        /// <summary>
        /// Looks for a particular Tnum input.
        /// </summary>
        public static Tnum Tnum(LegalEntity subj, string rel, LegalEntity directObj)        
        {    
            return (Tnum)QueryTvar<Tnum>(subj, rel, directObj);
        }
        public static Tnum Tnum(LegalEntity subj, string rel)        
        {    
            return (Tnum)QueryTvar<Tnum>(subj, rel);
        }
        
        /// <summary>
        /// Looks for a particular Tset input.
        /// </summary>
        public static Tset Tset(LegalEntity subj, string rel, LegalEntity directObj)        
        {    
            return (Tset)QueryTvar<Tset>(subj, rel, directObj);
        }
        public static Tset Tset(LegalEntity subj, string rel)        
        {    
            return (Tset)QueryTvar<Tset>(subj, rel);
        }
        
        /// <summary>
        /// Looks for a particular Tstr input.
        /// </summary>
        public static Tstr Tstr(LegalEntity subj, string rel, LegalEntity directObj)            
        {    
            return (Tstr)QueryTvar<Tstr>(subj, rel, directObj);
        }
        public static Tstr Tstr(LegalEntity subj, string rel)        
        {    
            return (Tstr)QueryTvar<Tstr>(subj, rel);
        }
        
        /// <summary>
        /// Looks for a particular Tdate input.
        /// </summary>
        public static Tdate Tdate(LegalEntity subj, string rel, LegalEntity directObj)           
        {   
            return (Tdate)QueryTvar<Tdate>(subj, rel, directObj);
        }
        public static Tdate Tdate(LegalEntity subj, string rel)           
        {   
            return (Tdate)QueryTvar<Tdate>(subj, rel);
        }

        /// <summary>
        /// Looks for a particular Person input.
        /// </summary>
        public static Person Person(LegalEntity subj, string rel)           
        {   
            return QueryPerson(subj, rel);
        }
    }
    
    public partial class Facts
    {
        //*********************************************************************
        // Symmetrical inputs
        //*********************************************************************
        
        /// <summary>
        /// Returns a symmetrical input boolean fact.  
        /// For example, A is married to B if B is married to A.
        /// </summary>
        /// <remarks>
        /// See unit tests for the truth table, which ensures that the existence
        /// of one false causes a false to be returned.
        /// Note that the Sym() functions are also designed to add facts to the
        /// Facts.Unknowns list in the proper order and with proper short-
        /// circuiting.
        /// </remarks>
        public static Tbool Sym(LegalEntity subj, string rel, LegalEntity directObj)
        {
            if (Facts.HasBeenAsserted(subj, rel, directObj))
            {
                return Input.Tbool(subj, rel, directObj);
            }
            
            return Input.Tbool(directObj, rel, subj);
        }
        
        /// <summary>
        /// Returns a symmetrical input string fact.
        /// </summary>
        public static Tbool Sym(LegalEntity subj, string rel, LegalEntity directObj, string val)
        {
            if (Facts.HasBeenAsserted(subj, rel, directObj))
            {
                return Input.Tstr(subj, rel, directObj) == val;
            }
            
            return Input.Tstr(directObj, rel, subj) == val;
        }
        
        /// <summary>
        /// Returns a symmetrical input string fact where the symmetrical relation
        /// has a different name in each direction (such as grandchild-grandparent).
        /// </summary>
        public static Tbool Sym(LegalEntity subj, string rel, LegalEntity obj, string text, string reverseText)
        {
            bool fwd = Facts.HasBeenAsserted(subj, rel, obj);
            bool bwd = Facts.HasBeenAsserted(obj, rel, subj);
            
            if (fwd) { return Input.Tstr(subj, rel, obj) == text; }
            
            else if (bwd) { return Input.Tstr(obj, rel, subj) == reverseText; }
            
            return Facts.Either(Input.Tstr(subj, rel, obj) == text, 
                                Input.Tstr(obj, rel, subj) == reverseText);
        }
        
        /// <summary>
        /// Returns either of the two Tbools.
        /// </summary>
        /// <remarks>
        /// This function is needed because if either A or B is false, the
        /// funtion should return false.  If, instead, A || B were used to
        /// analyze input facts, and if A were false and B were unknown, the 
        /// function would erroneously return unknown.
        /// </remarks>
        public static Tbool Either(Tbool A, Tbool B)
        {
            if (!A.IsEternallyUnknown)
            {
                return A;
            }
            
            return B;
        }   
        
        //*********************************************************************
        // Queries 
        //*********************************************************************
        
        // TODO: Refactor these to reduce repetition
        
        /// <summary>
        /// Query a temporal relationship (fact) of two legal entities.
        /// </summary>
        protected static T QueryTvar<T>(LegalEntity e1, string rel, LegalEntity e2) where T : Tvar
        {
            T defaultVal = (T)Auxiliary.ReturnProperTvar<T>(Hstate.Unstated);
            return QueryTvar<T>(e1, rel, e2, defaultVal);
        }

        protected static T QueryTvar<T>(LegalEntity e1, string rel, LegalEntity e2, T defaultValue) where T : Tvar
        {
            // Look up fact in table of facts
            foreach (Fact f in FactBase)
            {
                if (f.subject == e1 && f.relationship == rel && f.directObject == e2)
                {
                    return (T)f.v;
                }
            }

            // Add the fact to the list of unknown facts
            if (GetUnknowns)
            {
                AddUnknown(e1,rel,e2);
            }

            // If fact is not found, return a default value (usually "unstated")
            return defaultValue;
        }
        
        /// <summary>
        /// Query a temporal property (fact) of a single legal entity.
        /// </summary>
        protected static T QueryTvar<T>(LegalEntity e1, string rel) where T : Tvar
        {
            foreach (Fact f in FactBase)
            {
                if (f.subject == e1 && f.relationship == rel)
                {
                    return (T)f.v;
                }
            }
            
            if (GetUnknowns)
            {
                AddUnknown(e1, rel, null);
            }
            
            return (T)Auxiliary.ReturnProperTvar<T>(Hstate.Unstated);
        }

        /// <summary>
        /// Query that returns a person.
        /// </summary> 
        protected static Person QueryPerson(LegalEntity e1, string rel)
        {
            foreach (Fact f in FactBase)
            {
                if (f.subject == e1 && f.relationship == rel)
                {
                    return (Person)f.directObject;
                }
            }
            
            if (GetUnknowns)
            {
                AddUnknown(e1, rel, null);
            }
            
            return new Person("");
        }
    }
}