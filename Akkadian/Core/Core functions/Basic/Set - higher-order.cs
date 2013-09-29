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
using System.Linq;

namespace Akkadian
{
    /// <summary>
    /// Provides the interface for 
    /// </summary>
    public partial class Tset
    {
        /// <summary>
        /// Returns true when a condition holds for at least one member of
        /// a given set.
        /// </summary>
        public Tbool Exists(Func<Thing,Tbool> argumentFcn)
        {
            // Analyze set for existence of the condition
            Tset subset = this.Filter(x => argumentFcn(x));
            return subset.Count > 0;
        }
        
        /// <summary>
        /// Returns true when a condition holds for all members of
        /// a given set.
        /// </summary>
        public Tbool ForAll(Func<Thing,Tbool> argumentFcn)
        {
            // By convention, the universal quantification of an empty set is always true
            // http://en.wikipedia.org/wiki/Universal_quantification#The_empty_set
            if (this.Count == 0) return new Tbool(true);

            // Analyze set for universality of the condition
            Tset subset = this.Filter(x => argumentFcn(x));
            return this.Count == subset.Count;
        }
        
        /// <summary>
        /// Filter function - for various types of legal entities
        /// </summary>
        public Tset Filter(Func<Thing,Tbool> argumentFcn)
        {
            return ApplyFcnToTset<Tset>(this, x => argumentFcn((Thing)x), y => CoreFilter(y));
        }
        private static Hval CoreFilter(List<Tuple<Thing,Hval>> list)
        {
            List<Thing> result = new List<Thing>();
            foreach (Tuple<Thing,Hval> tu in list)
            {
                if (tu.Item2.IsTrue)
                {
                    result.Add(tu.Item1);
                }
            }
            return new Hval(result);
        }

        /// <summary>
        /// Totals the values of a given numeric property of the members of
        /// a set.
        /// </summary>
        public Tnum Sum(Func<Thing,Tnum> func)
        {
            return ApplyFcnToTset<Tnum>(this, x => func((Thing)x), y => CoreSum(y));
        }
        private static Hval CoreSum(List<Tuple<Thing,Hval>> list)
        {
            return SliceHvalsFromCube(list).Sum(item => Convert.ToDecimal(item.Val));
        }

        /// <summary>
        /// Returns the minimum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tnum Min(Func<Thing,Tnum> func)
        {
            return ApplyFcnToTset<Tnum>(this, x => func((Thing)x), y => CoreMin(y));
        }
        private static Hval CoreMin(List<Tuple<Thing,Hval>> list)
        {
            return Util.Minimum(SliceHvalsFromCube(list));
        }

        /// <summary>
        /// Returns the maximum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tnum Max(Func<Thing,Tnum> func)
        {
            return ApplyFcnToTset<Tnum>(this, x => func((Thing)x), y => CoreMax(y));
        }
        private static Hval CoreMax(List<Tuple<Thing,Hval>> list)
        {
            return Util.Maximum(SliceHvalsFromCube(list));
        }

        /// <summary>
        /// Sorts the members of a Tset based on a Tnum function.  Members with lower function values
        /// come first in the sorted list.
        /// </summary>
        public Tset OrderBy(Func<Thing,Tnum> func)
        {
            return ApplyFcnToTset<Tset>(this, x => func((Thing)x), y => CoreOrder(y)).Lean;
        }
        private static Hval CoreOrder(List<Tuple<Thing,Hval>> list)
        {
            List<Thing> result = new List<Thing>();

            IEnumerable<Tuple<Thing,Hval>> query = list.OrderBy(pair => pair.Item2.Val);

            foreach (Tuple<Thing,Hval> pair in query)
            {
                result.Add(pair.Item1);
            }

            return new Hval(result);
        }

        /// <summary>
        /// Applies an aggregation function to a Tset and an argument function.
        /// </summary>
        private static T ApplyFcnToTset<T>(Tset theSet, 
                                           Func<Thing,Tvar> argumentFcn, 
                                           Func<List<Tuple<Thing,Hval>>,Hval> aggregationFcn) where T : Tvar
        {
            Dictionary<Thing,Tvar> fcnValues = new Dictionary<Thing,Tvar>();
            List<Tvar> listOfTvars = new List<Tvar>();

            // Get the temporal value of each distinct entity in the set
            foreach(Thing le in DistinctEntities(theSet))
            {
                Tvar val = argumentFcn(le);
                fcnValues.Add(le, val);
                listOfTvars.Add(val);
            } 

            // At each breakpoint, for each member of the set,
            // aggregate and analyze the values of the functions
            T result = (T)Util.ReturnProperTvar<T>();
            foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
            {
                Hval membersOfSet = theSet.ObjectAsOf(dt);

                // If theSet is unknown...
                if (!membersOfSet.IsKnown)
                {
                    result.AddState(dt, membersOfSet);
                }
                else
                {
                    // Cube that gets sent to the aggregation function
                    List<Tuple<Thing,Hval>> thingValPairs = new List<Tuple<Thing,Hval>>();

                    // Values to check for uncertainty
                    List<Hval> values = new List<Hval>();

                    foreach(Thing le in (List<Thing>)membersOfSet.Val)
                    {
                        Tvar funcVal = (Tvar)fcnValues[le];    
                        Hval funcValAt = funcVal.ObjectAsOf(dt);
                        values.Add(funcValAt);
                        thingValPairs.Add(new Tuple<Thing,Hval>(le,funcValAt));
                    }

                    Hstate top = PrecedingState(values);
                    if (top != Hstate.Known)
                    {
                        result.AddState(dt, new Hval(null, top));
                    }
                    else
                    {
                        result.AddState(dt, aggregationFcn(thingValPairs));
                    } 
                }
            }

            return result.LeanTvar<T>();
        }

        /// <summary>
        /// Private method that aggregates all time points among a Tset and one
        /// or more Tvars
        /// </summary>
        private static List<DateTime> AggregatedTimePoints(Tset theSet, List<Tvar> listOfTvars)
        {
            listOfTvars.Add(theSet);
            return TimePoints(listOfTvars);
        }

        /// <summary>
        /// Returns the column of Hvals from the cube of Thing-Hval pairs.
        /// </summary>
        private static List<Hval> SliceHvalsFromCube(List<Tuple<Thing,Hval>> list)
        {
            List<Hval> slice = new List<Hval>();
            foreach (Tuple<Thing,Hval> t in list)
            {
                slice.Add(t.Item2);
            }
            return slice;
        }

        /// <summary>
        /// Returns a list of all legal entities that were ever members of the 
        /// set. 
        /// </summary>
        private static List<Thing> DistinctEntities(Tset theSet)
        {
            List<Thing> result = new List<Thing>();

            foreach(KeyValuePair<DateTime,Hval> de in theSet.TimeLine)
            {
                if (de.Value.IsKnown)
                {
                    foreach(Thing le in (List<Thing>)de.Value.Val)
                    {
                        if (!result.Contains(le))
                        {
                            result.Add(le);    
                        }
                    }
                }
            }

            return result;
        }
    }
}