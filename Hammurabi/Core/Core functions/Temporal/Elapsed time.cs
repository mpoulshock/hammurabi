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

namespace Akkadian
{    
    public partial class Tbool
    {
        /// <summary>
        /// Returns the total number of elapsed intervals between two dates.
        /// </summary>
        public Tnum TotalElapsedIntervals(Tnum interval, Tdate start, Tdate end)
        {
            Tnum rei = RunningElapsedIntervals(interval);

            return rei.AsOf(end) - rei.AsOf(start);
        }

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

        /// <summary>
        /// Provides a running count of how many whole intervals a Tbool 
        /// has been continuously true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--FTFTTF-->
        ///     tb.ICT = <--010120-->
        /// 
        /// Use judiciously with TheDay and TheCalendarWeek, as they have thousands of time intervals.
        /// </remarks>
        public Tnum ContinuousElapsedIntervals(Tnum interval)  
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
                    intervalCount = 0;
                    result.AddState(end, intervalCount);
                    dateNextTrue = this.DateNextTrue(end);
                    dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));
                }
            }

            return result;
        }

        /// <summary>
        /// Provides a running count of how many intervals (years, days, etc.) a Tbool 
        /// has been true within some sliding window of time.  At the end of that sliding 
        /// window, this function returns the amount of time that has elapsed within the 
        /// window during which the Tbool was true.
        /// </summary>
        /// <remarks>
        /// Example: For a given Tbool, at any given point in time, for how many days during the
        /// previous 3 days is the Tbool true?
        /// 
        ///                     tb = <FTTTFTTTFFFF>
        ///      tb.SEI(3, TheDay) = <001232223210>
        /// </remarks>
        public Tnum SlidingElapsedIntervals(Tnum interval, Tnum windowSize)
        {
            // If a Tbool is eternally true, return windowSize
            if (this.IsTrue)
            {
                return windowSize;
            }

            // The number of true intervals in a sliding window of time equals
            // the running count as of time1 minus the running count as of time0.
            Tnum r = this.RunningElapsedIntervals(interval);

            int size = Convert.ToInt32(windowSize.FirstValue.Val) * -1;

            // Counts the current inerval
            return r - r.Shift(size, interval);  
        }
    }    
}