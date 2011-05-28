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
    public partial class H
    {
        
        /// <summary>
        /// Returns true when a condition holds for at least one member of
        /// a given set.
        /// </summary>
        public static Tbool Exists(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            Tset subset = Filter(theSet, x => argumentFcn(x));
            return subset.Count > 0;
        }
        public static Tbool Exists(Tset theSet, Func<object,object,Tbool> argumentFcn, object fcnArg1, object fcnArg2)
        {
            Tset subset = Filter(theSet, (x,y) => argumentFcn(x,y), fcnArg1, fcnArg2);
            return subset.Count > 0;
        }
        
        /// <summary>
        /// Returns true when a condition holds for all members of
        /// a given set.
        /// </summary>
        public static Tbool ForAll(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            Tset subset = Filter(theSet, x => argumentFcn(x));
            return theSet.Count == subset.Count;
        }
        
        /// <summary>
        /// Returns a subset of a set, filtered by a given function with one input
        /// argument.  Set members that satisfy the given function are included in
        /// the subset.
        /// </summary>
        public static Tset Filter(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            if (theSet.IsUnknown) { return new Tset(); }
            
            Tset result = new Tset();
            
            Dictionary<object,Tvar> fcnValues = new Dictionary<object,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();
            
            // Get the temporal value of each distinct entity in the set
            foreach(LegalEntity le in theSet.DistinctEntities())
            {
                Tbool val = argumentFcn(le);
                
                if (val.IsUnknown) { return new Tset();    }    // TODO: Implement short-circuiting
                
                fcnValues.Add(le, val);
                listOfTvars.Add(val);
            }

            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                List<LegalEntity> membersOfOldSet = theSet.EntitiesAsOf(dt);
                List<LegalEntity> membersOfNewSet = new List<LegalEntity>();
                
                foreach(LegalEntity le in membersOfOldSet)
                {
                    Tbool funcVal = (Tbool)fcnValues[le];
                    
                    if (Convert.ToBoolean(funcVal.AsOf(dt).ToBool) == true)
                    {
                        membersOfNewSet.Add(le);
                    }
                }
                
                result.AddState(dt, membersOfNewSet);    
            }
            
            return result.LeanTvar<Tset>();
        }
        
        /// <summary>
        /// Returns a subset of a set, filtered by a given function with two input
        /// arguments.  Set members that satisfy the given function are included in
        /// the subset.
        /// The argument representing the members of the set should be input as null. 
        /// </summary>
        public static Tset Filter(Tset theSet, Func<object,object,Tbool> argumentFcn, object fcnArg1, object fcnArg2)
        {
            if (theSet.IsUnknown) { return new Tset(); }
            
            Tset result = new Tset();
            
            Dictionary<object,Tvar> fcnValues = new Dictionary<object,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();
            
            // Get the temporal value of each distinct entity in the set
            foreach(LegalEntity le in theSet.DistinctEntities())
            {
                Tbool val;
                
                if (fcnArg1 == null)
                {
                    val = argumentFcn(le,fcnArg2);
                }
                else
                {
                    val = argumentFcn(fcnArg1,le);
                }
                
                if (val.IsUnknown) { return new Tset();    }    // TODO: Implement short-circuiting
                
                fcnValues.Add(le, val);
                listOfTvars.Add(val);
            }

            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                List<LegalEntity> membersOfOldSet = theSet.EntitiesAsOf(dt);
                List<LegalEntity> membersOfNewSet = new List<LegalEntity>();
                
                foreach(LegalEntity le in membersOfOldSet)
                {
                    Tbool funcVal = (Tbool)fcnValues[le];
                    
                    if (Convert.ToBoolean(funcVal.AsOf(dt).ToBool) == true)
                    {
                        membersOfNewSet.Add(le);
                    }
                }
                
                result.AddState(dt, membersOfNewSet);    
            }
            
            return result.LeanTvar<Tset>();
        }
        
        /// <summary>
        /// Totals the values of a given numeric property of the members of
        /// a set.
        /// </summary>
        public static Tnum Sum(Tset theSet, Func<LegalEntity,Tnum> func)
        {
            return ApplyFcnToTset(theSet, x => func(x), y => Tnum.Sum(y));
        }
        
        /// <summary>
        /// Returns the minimum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public static Tnum Min(Tset theSet, Func<LegalEntity,Tnum> func)
        {
            return ApplyFcnToTset(theSet, x => func(x), y => Auxiliary.Minimum(y));
        }
        
        /// <summary>
        /// Returns the maximum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public static Tnum Max(Tset theSet, Func<LegalEntity,Tnum> func)
        {
            return ApplyFcnToTset(theSet, x => func(x), y => Auxiliary.Maximum(y));
        }
        
        /// <summary>
        /// A private method that applies a higher-order function to a set.
        /// </summary>
        private static Tnum ApplyFcnToTset(Tset theSet, 
                                           Func<LegalEntity,Tnum> argumentFcn, 
                                           Func<List<object>,object> aggregationFcn)
        {
            bool needToReturnUnknown = false;
            
            if (theSet.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            Dictionary<LegalEntity,Tvar> fcnValues = new Dictionary<LegalEntity,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();
            
            // Get the temporal value of each distinct entity in the set
            foreach(LegalEntity le in theSet.DistinctEntities())
            {
                Tvar val = argumentFcn(le);
                
                if (val.IsUnknown)
                {
                    needToReturnUnknown = true;
                }
                else
                {
                    fcnValues.Add(le, val);
                    listOfTvars.Add(val);
                }
            } 

            if (needToReturnUnknown) { return new Tnum(); }  // TODO: Implement short-circuiting
            
            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                List<LegalEntity> membersOfSet = theSet.EntitiesAsOf(dt);
                List<object> values = new List<object>();
                
                foreach(LegalEntity le in membersOfSet)
                {
                    Tvar funcVal = (Tvar)fcnValues[le];    
                    object funcValAt = funcVal.ObjectAsOf(dt);
                    values.Add(funcValAt);
                }
                
                object val = aggregationFcn(values);
                
                result.AddState(dt, val);    
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Private method that aggregates all time points among a Tset and one
        /// or more Tvars
        /// </summary>
        private static List<DateTime> AggregatedTimePoints(Tset theSet, List<Tvar> listOfTvars)
        {
            // Find all breakpoints in the Tvars
            List<DateTime> allTimePoints = TimePoints(listOfTvars);
            
            // Add breakpoints from theSet
            foreach(DateTime dt in theSet.TimePoints())
            {
                if (!allTimePoints.Contains(dt))
                {
                    allTimePoints.Add(dt);
                }
            }
            
            return allTimePoints;
        }
        
    }
}