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
    /// <summary>
    /// Provides the interface for 
    /// </summary>
    public partial class Tset
    {
        /// <summary>
        /// Exists function - for various types of legal entities
        /// </summary>
        public Tbool Exists(Func<Person,Tbool> argumentFcn)
        {
            return ExistsCore(this, x => argumentFcn((Person)x));
        }
        public Tbool Exists(Func<Property,Tbool> argumentFcn)
        {
            return ExistsCore(this, x => argumentFcn((Property)x));
        }

        /// <summary>
        /// Returns true when a condition holds for at least one member of
        /// a given set.
        /// </summary>
        private static Tbool ExistsCore(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            Tset subset = Tset.FilterCore(theSet, x => argumentFcn(x));
            return subset.Count > 0;
        }
        
        /// <summary>
        /// ForAll function - for various types of legal entities
        /// </summary>
        public Tbool ForAll(Func<Person,Tbool> argumentFcn)
        {
            return ForAllCore(this, x => argumentFcn((Person)x));
        }
        public Tbool ForAll(Func<Property,Tbool> argumentFcn)
        {
            return ForAllCore(this, x => argumentFcn((Property)x));
        }
        
        /// <summary>
        /// Returns true when a condition holds for all members of
        /// a given set.
        /// </summary>
        private static Tbool ForAllCore(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            if (theSet.Count == 0) return false;  // Is this correct?
            
            Tset subset = Tset.FilterCore(theSet, x => argumentFcn(x));
            return theSet.Count == subset.Count;
        }
        
        
        /// <summary>
        /// Filter function - for various types of legal entities
        /// </summary>
        public Tset Filter(Func<Person,Tbool> argumentFcn)
        {
            return FilterCore(this, x => argumentFcn((Person)x));
        }
        
        public Tset Filter(Func<Property,Tbool> argumentFcn)
        {
            return FilterCore(this, x => argumentFcn((Property)x));
        }
        
        /// <summary>
        /// Returns a subset of a set, filtered by a given function with one input
        /// argument.  Set members that satisfy the given function are included in
        /// the subset.
        /// </summary>
        private static Tset FilterCore(Tset theSet, Func<object,Tbool> argumentFcn)
        {
            Dictionary<object,Tvar> fcnValues = new Dictionary<object,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();
            
            // Get the temporal value of each distinct entity in the set
            foreach(LegalEntity le in theSet.DistinctEntities())
            {
                Tbool val = argumentFcn(le);
                fcnValues.Add(le, val);
                listOfTvars.Add(val);
            }

            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            Tset result = new Tset();
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                Hval membersOfOldSet = theSet.EntitiesAsOf(dt);

                // If theSet is unknown...
                if (!membersOfOldSet.IsKnown)
                {
                    result.AddState(dt, membersOfOldSet);
                }
                else
                {
                    List<LegalEntity> membersOfNewSet = new List<LegalEntity>();
                    List<Hval> unknownValues = new List<Hval>();
                    
                    foreach(LegalEntity le in (List<LegalEntity>)membersOfOldSet.Val)
                    {
                        Tbool funcVal = (Tbool)fcnValues[le];
                        Hval funcHval = funcVal.ObjectAsOf(dt);

                        // If any of the entity functions are unknown...
                        if (!funcHval.IsKnown)
                        {
                            unknownValues.Add(funcHval);
                        }
                        else if (Convert.ToBoolean(funcHval.Val))
                        {
                            membersOfNewSet.Add(le);
                        }
                    }

                    // Aggregate the results for this point in time
                    Hstate top = PrecedingState(unknownValues);
                    if (unknownValues.Count > 0 && top != Hstate.Known) 
                    {
                        result.AddState(dt, top);
                    }
                    else
                    {
                        result.AddState(dt, new Hval(membersOfNewSet));   
                    }
                }  
            }
            
            return result.LeanTvar<Tset>();
        }

        /// <summary>
        /// Totals the values of a given numeric property of the members of
        /// a set.
        /// </summary>
        public Tnum Sum(Func<Property,Tnum> func)
        {
            return ApplyFcnToTset(this, x => func((Property)x), y => Tnum.Sum(y));
        }
        public Tnum Sum(Func<Person,Tnum> func)
        {
            return ApplyFcnToTset(this, x => func((Person)x), y => Tnum.Sum(y));
        }
  
        /// <summary>
        /// Returns the minimum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tnum Min(Func<LegalEntity,Tnum> func)
        {
            return ApplyFcnToTset(this, x => func(x), y => Auxiliary.Minimum(y));
        }
        
        /// <summary>
        /// Returns the maximum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tnum Max(Func<LegalEntity,Tnum> func)
        {
            return ApplyFcnToTset(this, x => func(x), y => Auxiliary.Maximum(y));
        }
        
        /// <summary>
        /// A private method that applies a higher-order function to a set.
        /// </summary>
        private static Tnum ApplyFcnToTset(Tset theSet, 
                                           Func<LegalEntity,Tnum> argumentFcn, 
                                           Func<List<Hval>,Hval> aggregationFcn)
        {
            Dictionary<LegalEntity,Tvar> fcnValues = new Dictionary<LegalEntity,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();
            
            // Get the temporal value of each distinct entity in the set
            foreach(LegalEntity le in theSet.DistinctEntities())
            {
                Tvar val = argumentFcn(le);
                fcnValues.Add(le, val);
                listOfTvars.Add(val);
            } 

            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            Tnum result = new Tnum();
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                Hval membersOfSet = theSet.EntitiesAsOf(dt);

                // If theSet is unknown...
                if (!membersOfSet.IsKnown)
                {
                    result.AddState(dt, membersOfSet);
                }
                else
                {
                    List<Hval> values = new List<Hval>();
                    
                    foreach(LegalEntity le in (List<LegalEntity>)membersOfSet.Val)
                    {
                        Tvar funcVal = (Tvar)fcnValues[le];    
                        Hval funcValAt = funcVal.ObjectAsOf(dt);
                        values.Add(funcValAt);
                    }
                    
                    Hval val = aggregationFcn(values);
                    
                    result.AddState(dt, val);   
                }
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