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

namespace Hammurabi
{    
    public partial class Tbool
    {
        /// <summary>
        /// Provides a running count of the number of whole intervals 
        /// that a Tbool has been true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--FTFTTFF-->
        ///     tb.REI = <--0011233-->
        /// 
        /// Note: An elapsed interval is counted in the subsequent interval.
        /// </remarks>
        public Tnum RunningElapsedIntervals(Tnum interval)  
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tnum(baseState);

            int intervalCount = 0;
            DateTime dateNextTrue = this.DateNextTrue(Time.DawnOf);
            DateTime dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));

            Tnum result = new Tnum(0);

            // Iterate through the time intervals in the input Tnum
            for (int i=0; i < interval.IntervalValues.Count-1; i++)
            {
                DateTime start = interval.IntervalValues.Keys[i];
                DateTime end = interval.IntervalValues.Keys[i+1];

                // If base Tbool is always true during the interval, increment the count
                if (end <= dateNextTrueIntervalEnds)
                {
                    if (start >= dateNextTrue)
                    {
                        intervalCount++;
                        result.AddState(end, intervalCount);
                        continue;
                    }
                }
                else
                {
                    // Otherwise, skip to next true interval
                    dateNextTrue = this.DateNextTrue(end);
                    dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));
                }
            }

            return result;
        }

//        /// <summary>
//        /// Provides a running count of how many intervals (years, days, etc.) a Tbool 
//        /// has been true.  The count carries over from one true interval to the next.
//        /// </summary>
//        /// <remarks>
//        /// An interval is counted in the subsequent interval, meaning that the count
//        /// reflects how many past intervals have been true.
//        /// 
//        /// Example:
//        /// 
//        ///         tb = <--FTTTTTFFFTTTTFF-->
//        ///     tb.DCT = <--001234500067890-->
//        /// 
//        /// Use these methods judiciously, as they can involve tens of thousands of intervals.
//        /// </remarks>
//        /// NOT DELETING ONLY BECAUSE THIS IS A VERY FAST METHOD
//        public Tnum RunningElapsedTime(Time.IntervalType interval)  
//        {
//            // If base Tnum is ever unknown during the time period, return 
//            // the state with the proper precedence
//            Hstate baseState = PrecedenceForMissingTimePeriods(this);
//            if (baseState != Hstate.Known) return new Tnum(baseState);
//
//            Tnum result = new Tnum();
//            int count = 0;
//
//            // Iterate through the time intervals in the Tbool
//            for (int i=0; i < this.IntervalValues.Count; i++)
//            {
//                // Get interval start date
//                DateTime intervalStart = this.IntervalValues.Keys[i];
//
//                if (this.IntervalValues.Values[i].IsTrue) 
//                {
//                    // Determine the end of the interval
//                    DateTime nextIntervalStart = new DateTime();
//                    if (i == this.IntervalValues.Count-1)
//                    {
//                        nextIntervalStart = Time.EndOf.AddYears(-1);
//                    }
//                    else
//                    {
//                        nextIntervalStart = this.IntervalValues.Keys[i+1];
//                    }
//
//                    // Variables to keep track of count and date
//                    DateTime indexDate = intervalStart;
//            
//                    // Begin counting off intervals
//                    while (indexDate < nextIntervalStart) 
//                    {
//                        result.AddState(indexDate, count);
//                        count++;
//                        indexDate = indexDate.AddInterval(interval, 1);
//                    }
//                }
//                else
//                {
//                    result.AddState(intervalStart, count);
//                }
//            }
//            
//            return result.Lean;
//        }
    }
}