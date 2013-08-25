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

namespace Hammurabi
{
    /*
     * The functions below analyze Tbools to assess the intervals (days, weeks, 
     * months, years, etc.) in which the Tbool is true. 
     * 
     * These functions answer questions like:
     * 
     *  - Is the Tbool always or ever true during each interval?
     *  - How much total time has elapsed during which the Tbool is true?
     *  - In how many subintervals, within a given (larger type of) interval, is
     *    the Tbool true?
     *  - In how many intervals before a given interval is the Tbool true?
     *  - When is the Tbool true for a consecutive number of intervals?
     * 
     */

    public partial class Tbool : Tvar
    {
        /// <summary>
        /// Indicates, for each time interval in a given Tnum, whether the Tbool
        /// is ever true during that interval.
        /// </summary>
        public Tbool EverPer(Tnum intervals)
        {
            // If the interval Tnum is eternally unknown, return unknown
            if (intervals.IntervalValues.Count == 1 &&
                !intervals.FirstValue.IsKnown)
            {
                return new Tbool(intervals.FirstValue);
            }

            Tbool result = new Tbool();

            IList<DateTime> tPoints = intervals.TimePoints();

            // Check each time interval to see if condition is true
            for (int i = 0; i < tPoints.Count-1; i++) 
            {
                Hval isEverTrue = this.IsEverTrue(tPoints[i], tPoints[i+1]).FirstValue;
                result.AddState(tPoints[i], isEverTrue);
            }
            
            // This doesn't use .Lean because the output of EverPer() is often
            // the input to a function that counts the number of discrete 
            // intervals.  If you want a "lean" result, append .Lean when using
            // this function.
            return result;
        }
        
        /// <summary>
        /// Indicates, for each time interval in a given Tnum, whether the Tbool
        /// is always true during that interval.
        /// </summary>
        public Tbool AlwaysPer(Tnum intervals)
        {
            // If the interval Tnum is eternally unknown, return unknown
            if (intervals.IntervalValues.Count == 1 &&
                !intervals.FirstValue.IsKnown)
            {
                return new Tbool(intervals.FirstValue);
            }

            Tbool result = new Tbool();

            IList<DateTime> tPoints = intervals.TimePoints();

            // Foreach interval in intervals
            for (int i = 0; i < tPoints.Count-1; i++) 
            {
                Hval isAlwaysTrue = this.IsAlwaysTrue(tPoints[i], tPoints[i+1]).FirstValue;
                result.AddState(tPoints[i], isAlwaysTrue);
            }
            
            // Doesn't use .Lean.  See explanation in EverPer() above.
            return result;
        }
        
        /// <summary>
        /// Returns a total count of the number of complete subintervals
        /// within each interval in which the Tvar (this) is true.
        /// </summary>
        /// <remarks>
        /// This function should be used as an extension method to EverPer()
        /// or AlwaysPer().
        /// Example: Count the number of weeks each year during which
        /// a person was employed.
        /// </remarks>
        // TODO: Add support for counting partial subintervals
        public Tnum CountPer(Tnum intervals)
        {
            // TODO: Handle unknowns...
            
            Tnum result = new Tnum();
            
            SortedList<DateTime, Hval> big = intervals.IntervalValues;
            SortedList<DateTime, Hval> small = this.IntervalValues;
            
            for (int b = 0; b < big.Count-1; b++ ) 
            {
                int count = 0;
                DateTime bStart = big.Keys[b];
                DateTime bEnd = big.Keys[b+1];
                
                for (int s = 0; s < small.Count-1; s++ ) 
                {
                    DateTime sStart = small.Keys[s];
                    DateTime sEnd = small.Keys[s+1];
                    
                    if (sStart >= bStart && sEnd <= bEnd && this.AsOf(sStart).ToBool == true)
                    {
                        count++;
                    }
                }
                
                result.AddState(bStart,count);
                count = 0;
            }
        
            return result.Lean;
        }
        
        /// <summary>
        /// Returns a running count (over time) of the number of subintervals
        /// within each interval in which the Tvar (this) is true.
        /// </summary>
        /// <remarks>
        /// This function should be used as an extension method to EverPer()
        /// or AlwaysPer().
        /// Example: Count the number of weeks each year during which
        /// a person was employed.  The first week of employment would be 
        /// numbered 0, the second 1, etc.
        /// </remarks>
        // TODO: Fix broken test case for this function.
        public Tnum RunningCountPer(Tnum intervals)
        {
            // TODO: Implement unknowns
            
            Tnum result = new Tnum();
            result.AddState(Time.DawnOf,0);
            
            int count = 0;
            decimal? prevBig = 0;
            SortedList<DateTime, Hval> sub = this.IntervalValues;
            
            // Iterate through the sub-intervals
            for (int i = 0; i < sub.Count-1; i++ ) 
            {
                DateTime dt = sub.Keys[i];
                
                // Reset count for each new (big, not sub-) interval
                decimal? big = intervals.AsOf(dt).ToDecimal;
                if (big != prevBig) count = 0;
                prevBig = big;
                
                // If the Tbool is true during the subinterval, increment
                // the subsequent subinterval
                if (this.AsOf(dt).ToBool == true)
                {
                    count++;
                }
                
                result.AddState(sub.Keys[i+1], count);
            }
        
            return result.Lean;
        }

        /// <summary>
        /// Returns the number of intervals that the Tbool is true,
        /// covering a range of intervals starting x intervals in the past
        /// and ending y intervals in the past (or, if y=0, in the present
        /// interval).
        /// </summary>
        /// <remarks>
        /// 
        /// Interval #     4    3    2    1    0
        ///             +----+----+----+----+----+----+----+
        ///                                    ^
        ///                                  "now"
        ///                      (the interval being analyzed)
        /// 
        /// The current interval is counted.  So, for example, if the range
        /// equals 2, the function would evaluate the current interval and the
        /// prior interval.
        /// </remarks>
//        private Tnum CountPastNIntervals(Tnum intervals, Tnum rangeStart, Tnum rangeEnd)
//        {
//            // If base Tnum is ever unknown during the time period, return 
//            // the state with the proper precedence
//            Hstate tnumState = PrecedenceForMissingTimePeriods(this);
//            if (tnumState != Hstate.Known) return new Tnum(tnumState);
//
//            // Determine the nature of the range (the time window to analyze)
//            int rangeStartInt = Convert.ToInt32(rangeStart.FirstValue.Val);
//            int rangeEndInt = Convert.ToInt32(rangeEnd.FirstValue.Val);
//            int rangeWidth = rangeStartInt - rangeEndInt + 1;
//
//            // Set up the result Tnum
//            Tnum result = new Tnum();
//            result.AddState(Time.DawnOf, 0);  // Probably not ideal, but good enough
//
//            // Tvars we'll be working with
//            SortedList<DateTime, Hval> intrvl = intervals.IntervalValues;
//            SortedList<DateTime, Hval> baseTbool = this.IntervalValues;
//
//            // Time window start and end, and date Tbool next changes
//            DateTime windowStart = Time.DawnOf;
//            DateTime windowEnd = Time.EndOf;
//            DateTime nextChangeDate = Time.DawnOf;
//
//            // Count forward from the beginning of time through each interval
//            for (int b = 0; b < intrvl.Count - 1; b++)
//            {
//                // Determine the start and end dates of current window
//                windowEnd = intrvl.Keys[b];
//                if (b > rangeWidth) windowStart = intrvl.Keys[b - rangeWidth];
//
//                // Only analyze the window if its end passes the next change date
//                // Otherwise, skip ahead until the next time the Tbool changes value
//                // (for performance).
//                if (windowEnd > nextChangeDate)
//                {
//                    // To count how many of the relevant intervals are true
//                    int count = 0;
//
//                    // Collect unknown states during the relevant time period
//                    List<Hval> states = new List<Hval>();
//
//                    // Look back the specified number of intervals
//                    for (int n = rangeEndInt; n < rangeStartInt; n++)
//                    {
//                        int index = b - n;
//                        if (index >= rangeEndInt)
//                        {
//                            // Handle unknowns
//                            Hval baseState = this.ObjectAsOf(intrvl.Keys[index]);
//                            if (!baseState.IsKnown)
//                            {
//                                states.Add(baseState);
//                            }
//                            else if (Convert.ToBoolean(baseState.Val))
//                            {
//                                count++;
//                            }
//                        }
//                    }
//
//                    // Add the appropriate state
//                    if (states.Count > 0)
//                    {
//                        result.AddState(intrvl.Keys[b], PrecedingState(states));
//                    }
//                    else
//                    {
//                        result.AddState(intrvl.Keys[b], count);
//                    }
//
//                    // Determine when the Tbool next changes its value
//                    // First, if the window has moved past the last Tbool change date,
//                    // then we can jump ahead to the end of time.
//                    if (windowStart > baseTbool.Keys[baseTbool.Count-1])
//                    {
//                        nextChangeDate = Time.EndOf;
//                    }
//                    else
//                    {
//                        for (int j=0; j < baseTbool.Count; j++)
//                        {
//                            // If the window is subsumed by a single interval on the Tbool,
//                            // then we can jump ahead to that point in time.
//                            if (windowEnd < baseTbool.Keys[j] &&
//                                windowStart > baseTbool.Keys[j-1])
//                            {
//                                nextChangeDate = baseTbool.Keys[j];
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//
//            return result.Lean;
//        }
    }
}