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
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalWeeks(Tnum numberOfWeeks)
        {
            Tnum lastN = this.AlwaysPer(TheCalendarWeek).CountPastNIntervals(TheCalendarWeek, numberOfWeeks + 1, 1);
            return this & lastN >= numberOfWeeks;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalWeeks(Tnum numberOfWeeks)
        {
            Tnum lastN = this.EverPer(TheCalendarWeek).CountPastNIntervals(TheCalendarWeek, numberOfWeeks + 1, 1);
            return this & lastN >= numberOfWeeks;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar months, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalMonths(Tnum numberOfMonths)
        {
            Tnum lastN = this.AlwaysPer(TheMonth).CountPastNIntervals(TheMonth, numberOfMonths+1, 1);
            return this & lastN >= numberOfMonths;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar months, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalMonths(Tnum numberOfMonths)
        {
            Tnum month = TheTime.TheMonth;
            Tnum lastN = this.EverPer(month).CountPastNIntervals(month, numberOfMonths + 1, 1);
            return this & lastN >= numberOfMonths;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar quarters, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalQtrs(Tnum numberOfQtrs)
        {
            Tnum lastN = this.AlwaysPer(TheQuarter).CountPastNIntervals(TheQuarter, numberOfQtrs + 1, 1);
            return this & lastN >= numberOfQtrs;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar quarters, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalQtrs(Tnum numberOfQtrs)
        {
            Tnum qtr = TheTime.TheQuarter;
            Tnum lastN = this.EverPer(qtr).CountPastNIntervals(qtr, numberOfQtrs + 1, 1);
            return this & lastN >= numberOfQtrs;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalYears(Tnum numberOfYears)
        {
            Tnum lastN = this.AlwaysPer(TheYear).CountPastNIntervals(TheYear, numberOfYears + 1, 1);
            return this & lastN >= numberOfYears;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalYears(Tnum numberOfYears)
        {
            Tnum year = TheTime.TheYear;
            Tnum lastN = this.EverPer(year).CountPastNIntervals(year, numberOfYears + 1, 1);
            return this & lastN >= numberOfYears;
        }

        /// <summary>
        /// Returns the number of intervals that the Tbool is true,
        /// going back a specified number of intervals.
        /// </summary>
        /// <remarks>
        /// The current interval is counted.  So, for example, if the range
        /// equals 2, the function would evaluate the current interval and the
        /// prior interval.
        /// </remarks>
        public Tnum CountPastNIntervals(Tnum intervals, Tnum rangeSpan)
        {
            return CountPastNIntervals(intervals, rangeSpan, 0);
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
        /// </remarks>
        public Tnum CountPastNIntervalsOld(Tnum intervals, Tnum rangeStart, Tnum rangeEnd)
        {
            // TODO: Implement unknowns properly for all inputs

            int rangeStartInt = Convert.ToInt32(rangeStart.FirstValue.Val);
            int rangeEndInt = Convert.ToInt32(rangeEnd.FirstValue.Val);
            Tnum result = new Tnum();
            SortedList<DateTime, Hval> intrvl = intervals.IntervalValues;

            // TODO: Improve performance by only examining changes to the start
            //       and end of the range.  (Values in the middle stay the same
            //       as the windown shifts forward in time.)

            // Count forward from the beginning of time through each interval
            for (int b = 0; b < intrvl.Count - 1; b++)
            {
                int count = 0;

                // Collect unknown states during the relevant time period
                List<Hval> states = new List<Hval>();

                // Look back the specified number of intervals
                for (int n = rangeEndInt; n < rangeStartInt; n++)
                {
                    int index = b - n;
                    if (index >= rangeEndInt)
                    {
                        // Handle unknowns
                        Hval baseState = this.ObjectAsOf(intrvl.Keys[index]);
                        if (!baseState.IsKnown)
                        {
                            states.Add(baseState);
                        }
                        else if (Convert.ToBoolean(baseState.Val))
                        {
                            count++;
                        }
                    }
                }

                if (states.Count > 0)
                {
                    result.AddState(intrvl.Keys[b], PrecedingState(states));
                }
                else
                {
                    result.AddState(intrvl.Keys[b], count);
                }
            }

            return result.Lean;
        }


        public Tnum CountPastNIntervals(Tnum intervals, Tnum rangeStart, Tnum rangeEnd)
        {
            // TODO: Implement unknowns properly for all inputs

            int rangeStartInt = Convert.ToInt32(rangeStart.FirstValue.Val);
            int rangeEndInt = Convert.ToInt32(rangeEnd.FirstValue.Val);
            int rangeWidth = rangeStartInt - rangeEndInt + 1;

            Tnum result = new Tnum();
            result.AddState(Time.DawnOf, 0);

            SortedList<DateTime, Hval> intrvl = intervals.IntervalValues;

            DateTime nextChangeDate = Time.DawnOf;
            DateTime windowStart = Time.DawnOf;
            DateTime windowEnd = Time.EndOf;

            // Count forward from the beginning of time through each interval
            for (int b = 0; b < intrvl.Count - 1; b++)
            {
                // Determine start and end of current window
                windowEnd = intrvl.Keys[b];
                if (b > rangeWidth) windowStart = intrvl.Keys[b - rangeWidth];

                if (windowEnd <= nextChangeDate)
                {
                    // Skip ahead until the next time the Tbool changes value
                }
                else
                {
                    int count = 0;

                    // Collect unknown states during the relevant time period
                    List<Hval> states = new List<Hval>();

                    // Look back the specified number of intervals
                    for (int n = rangeEndInt; n < rangeStartInt; n++)
                    {
                        int index = b - n;
                        if (index >= rangeEndInt)
                        {
                            // Handle unknowns
                            Hval baseState = this.ObjectAsOf(intrvl.Keys[index]);
                            if (!baseState.IsKnown)
                            {
                                states.Add(baseState);
                            }
                            else if (Convert.ToBoolean(baseState.Val))
                            {
                                // Count how many of the relevant intervals are true
                                count++;
                            }
                        }
                    }

                    if (states.Count > 0)
                    {
                        result.AddState(intrvl.Keys[b], PrecedingState(states));
                    }
                    else
                    {
                        result.AddState(intrvl.Keys[b], count);
                    }

                    // Determine when the Tbool next changes its value
                    for (int j=0; j < this.IntervalValues.Count; j++)
                    {
                        if (this.IntervalValues.Keys[j] > windowEnd &&
                            this.IntervalValues.Keys[j-1] < windowStart)
                        {
                            nextChangeDate = this.IntervalValues.Keys[j];
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(result.IntervalValues.Count);     // diagnostic
            return result.Lean;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive days, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveDays(Tnum numberOfDays)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Day, numberOfDays);
        }
        
        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveWeeks(Tnum numberOfWeeks)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Week, numberOfWeeks);
        }
        
        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive months, and (2) it is still true.
        /// </summary>
        /// <remarks>
        /// Example: if isEmployed.ForConsecutiveMonths(6), if the person is 
        /// employed from 1/1 to 12/31, this function will return true from 
        /// 7/1 - 12/31.
        /// </remarks>
        public Tbool ConsecutiveMonths(Tnum numberOfMonths)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Month, numberOfMonths);
        }
        
        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveYears(Tnum numberOfYears)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Year, numberOfYears);
        }
        
        /// <summary>
        /// General function that determines whether a Tbool is true for a given number
        /// of intervals of a specified type. 
        /// </summary>
        private Tbool ForConsecutiveIntervals(Time.IntervalType type, Tnum numberOfIntervals)
        {
            // If val is unknown and base Tvar is eternally unknown,
            // return the typical precedence state
            Hval intervals = numberOfIntervals.FirstValue;
            if (!intervals.IsKnown && this.TimeLine.Count == 1)
            {
                if (!this.FirstValue.IsKnown)
                {
                    Hstate s = PrecedingState(this.FirstValue, intervals);
                    return new Tbool(s);
                }
            }

            // If val is unknown, return its state
            if (!intervals.IsKnown) return new Tbool(intervals);

            // Else begin counting intervals...
            Tbool result = new Tbool();
            result.AddState(Time.DawnOf, false);
            
            SortedList<DateTime, Hval> t = this.TimeLine;
            
            for (int i = 0; i < t.Count; i++)
            {
                // If any interval is unknown, return that value
                // This could be refined later (some unknown intervals are irrelevant)
                if (!t.Values[i].IsKnown)
                {
                    return new Tbool(t.Values[i]);
                }

                if (t.Values[i].IsTrue)
                {
                    DateTime start = t.Keys[i];
                    int noOfIntervals = Convert.ToInt32(numberOfIntervals.FirstValue.Val);
                    DateTime thresholdReached = start.AddInterval(type, noOfIntervals);

                    if ((i < t.Count-1 && thresholdReached <= t.Keys[i+1]) || i == t.Count-1)
                    {
                        
                        result.AddState(thresholdReached, true);
                    }
                }
            }

            // If the underlying Tbool (this) is no longer true, the result should
            // not be true either.
            Tbool trueForPeriod = this && result;
            
            return trueForPeriod.Lean;
        }
    }
}