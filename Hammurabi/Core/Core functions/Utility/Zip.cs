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
    public abstract partial class H
    {
        /// <summary>
        /// Gets all time points and assocated values from the input Tvar objects, by merging them
        /// together like a zipper.
        /// </summary>
        /// <remarks>
        /// This method is ~590 times faster than TimePointValuesForMultipleTvars in a worst-case
        /// scenario like computing Time.TheDay > 12. For Tvars with very few time points, it's
        /// about 10-20% faster.
        /// </remarks>
        public static SortedList<DateTime,List<Hval>> TimePointValues(params Tvar[] list)
        {
            // Handle cases with more than two input Tvars, and BoolCount with one input argument
            if (list.Length != 2) return TimePointValuesForMultipleTvars(list);

            SortedList<DateTime,List<Hval>> result = new SortedList<DateTime,List<Hval>>();
            SortedList<DateTime,Hval> t1 = list[0].IntervalValues;
            SortedList<DateTime,Hval> t2 = list[1].IntervalValues;

            int idx1 = 0;
            int idx2 = 0;
            Hval val1 = t1.Values[0];
            Hval val2 = t2.Values[0];
            int t1Count = t1.Count;
            int t2Count = t2.Count;

            // Add the first state - assumes all Tvars start at Time.DawnOf
            result.Add(Time.DawnOf, new List<Hval>(){val1,val2});

            // Walk along the two Tvars, index by index, until you get to the end of both
            while (!(idx1+1 >= t1Count && idx2+1 >= t2Count))
            {
                // Don't exceed the length of an array
                int nextIdx1 = Math.Min(idx1+1, t1Count-1);
                int nextIdx2 = Math.Min(idx2+1, t2Count-1);

                DateTime nextDate1 = t1.Keys[nextIdx1];
                DateTime nextDate2 = t2.Keys[nextIdx2];

                // If the next dates on T1 and T2 are the same, advance along both Tvars
                if (nextDate1 == nextDate2)
                {
                    idx1 = nextIdx1;
                    val1 = t1.Values[idx1];

                    idx2 = nextIdx2;
                    val2 = t2.Values[idx2];

                    result.Add(nextDate1, new List<Hval>(){val1,val2});
                }
                // Advance along Tvar that is farther behind, except when the one that's behind is at its end
                else if (idx2 == t2Count-1 || (idx1 != t1Count-1 && nextDate2 > nextDate1))
                {
                    // Advance along T1
                    idx1 = nextIdx1;
                    val1 = t1.Values[idx1];
                    result.Add(nextDate1, new List<Hval>(){val1,val2});
                }
                else 
                {
                    // Advance along T2
                    idx2 = nextIdx2;
                    val2 = t2.Values[idx2];
                    result.Add(nextDate2, new List<Hval>(){val1,val2});
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all time points and assocated values from the input Tvar objects.
        /// </summary>
        protected static SortedList<DateTime,List<Hval>> TimePointValuesForMultipleTvars(params Tvar[] list)
        {
            SortedList<DateTime,List<Hval>> result = new SortedList<DateTime, List<Hval>>();

            // Foreach time point
            foreach (DateTime d in TimePoints(list))
            {
                List<Hval> vals = new List<Hval>();

                // Make list of values at that point in time
                foreach (Tvar h in list)
                {
                    // This is part of why this method is slow
                    vals.Add(h.ObjectAsOf(d));
                }

                result.Add(d,vals);
            }

            return result;
        }

        /// <summary>
        /// Gets all time points in a set of Tvar objects.
        /// </summary>
        public static List<DateTime> TimePoints(params Tvar[] list)
        {
            List<Tvar> arrayToList = new List<Tvar>(list.Length);
            arrayToList.AddRange(list);
            return TimePoints(arrayToList);
        }

        public static List<DateTime> TimePoints(List<Tvar> list)
        {
            List<DateTime> bps = new List<DateTime>();
            
            foreach (Tvar v in list)
            {
                foreach (DateTime d in v.TimePoints())
                {
                    if (!bps.Contains(d))
                    {
                        bps.Add(d);
                    }
                }
            }
            
            return bps;
        }
        
        /// <summary>
        /// Apply a function to all values in a list of zipped Tvar values.
        /// </summary>
        public static T ApplyFcnToTimeline<T>(Func<List<Hval>,Hval> fcn, params Tvar[] list) where T : Tvar
        {
            T result = (T)Util.ReturnProperTvar<T>();

            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(list))
            {  
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known)
                {
                    result.AddState(slice.Key, new Hval(null, top));
                }
                else
                {
                    result.AddState(slice.Key, fcn(slice.Value));
                }
            }

            return result.LeanTvar<T>();
        }

        /// <summary>
        /// Apply a function to the values in a single Tvar.
        /// </summary>
        /// <remarks>
        /// This is used for unary functions (those that only operate on a single Tvar).
        /// </remarks>
        public static T ApplyFcnToTimeline<T>(Func<Hval,Hval> fcn, Tvar tv) where T : Tvar
        {
            T result = (T)Util.ReturnProperTvar<T>();

            foreach(KeyValuePair<DateTime,Hval> slice in tv.IntervalValues)
            {  
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known)
                {
                    result.AddState(slice.Key, new Hval(null, top));
                }
                else
                {
                    result.AddState(slice.Key, fcn(slice.Value));
                }
            }

            return result.LeanTvar<T>();
        }
    }    
}