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
            if (this.IsUnknown) { return new Tbool(); }

            Tbool result = new Tbool();

            IList<DateTime> tPoints = intervals.TimePoints();

            // Check each time interval to see if condition is true
            for (int i = 0; i < tPoints.Count-1; i++) 
            {
                bool? isEverTrue = this.IsEver(true, tPoints[i], tPoints[i+1]).ToBool;
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
            if (this.IsUnknown) { return new Tbool(); }

            Tbool result = new Tbool();

            IList<DateTime> tPoints = intervals.TimePoints();

            // Foreach interval in intervals
            for (int i = 0; i < tPoints.Count-1; i++) 
            {
                bool? isAlwaysTrue = this.IsAlways(true, tPoints[i], tPoints[i+1]).ToBool;
                result.AddState(tPoints[i], isAlwaysTrue);
            }
            
            // Doesn't use .Lean.  See explanation in EverPer() above.
            return result;
        }

        /// <summary>
        /// Returns the total elapsed days that a Tbool is true,
        /// for each of a given set of intervals.
        /// </summary>
        public Tnum ElapsedDaysPer(Tnum period)
        {
            return ElapsedDaysPer(period, true);
        }
        
        /// <summary>
        /// Returns the total elapsed days during which a Tbool is true. 
        /// </summary>
        public Tnum ElapsedDays()
        {
            return ElapsedDays(true, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns the total elapsed days, between two given DateTimes, during
        /// which a Tbool is true. 
        /// </summary>
        public Tnum ElapsedDays(DateTime start, DateTime end)
        {
            return ElapsedDays(true, start, end);
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
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            SortedList<DateTime, object> big = intervals.IntervalValues;
            SortedList<DateTime, object> small = this.IntervalValues;
            
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
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            result.AddState(Time.DawnOf,0);
            
            int count = 0;
            decimal? prevBig = 0;
            SortedList<DateTime, object> sub = this.IntervalValues;
            
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
        /// going back a specified number of intervals.
        /// </summary>
        /// <remarks>
        /// The current interval is counted.  So, for example, if the range
        /// equals 2, the function would evaluate the current interval and the
        /// prior interval.
        /// </remarks>
        public Tnum CountPastNIntervals(Tnum intervals, int rangeSpan)
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
        public Tnum CountPastNIntervals(Tnum intervals, int rangeStart, int rangeEnd)
        {
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }

            Tnum result = new Tnum();
            SortedList<DateTime, object> t = intervals.IntervalValues;

            // Count forward from the beginning of time through each interval
            for (int b = 0; b < t.Count - 1; b++)
            {
                int count = 0;

                // Look back the specified number of intervals
                for (int n = rangeEnd; n < rangeStart; n++)
                {
                    int index = b - n;
                    if (index >= rangeEnd)
                    {
                        if (this.AsOf(t.Keys[index]).ToBool == true)
                        {
                            count++;
                        }
                    }
                }

                result.AddState(t.Keys[b], count);
            }

            return result.Lean;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalWeeks(int numberOfWeeks)
        {
            Tnum lastN = this.AlwaysPer(TheCalendarWeek).CountPastNIntervals(TheCalendarWeek, numberOfWeeks + 1, 1);
            return this & lastN >= numberOfWeeks;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalWeeks(int numberOfWeeks)
        {
            Tnum lastN = this.EverPer(TheCalendarWeek).CountPastNIntervals(TheCalendarWeek, numberOfWeeks + 1, 1);
            return this & lastN >= numberOfWeeks;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar months, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalMonths(int numberOfMonths)
        {
            Tnum lastN = this.AlwaysPer(TheMonth).CountPastNIntervals(TheMonth, numberOfMonths+1, 1);
            return this & lastN >= numberOfMonths;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar months, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalMonths(int numberOfMonths)
        {
            Tnum month = TheTime.TheMonth;
            Tnum lastN = this.EverPer(month).CountPastNIntervals(month, numberOfMonths + 1, 1);
            return this & lastN >= numberOfMonths;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar quarters, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalQtrs(int numberOfQtrs)
        {
            Tnum lastN = this.AlwaysPer(TheQuarter).CountPastNIntervals(TheQuarter, numberOfQtrs + 1, 1);
            return this & lastN >= numberOfQtrs;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar quarters, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalQtrs(int numberOfQtrs)
        {
            Tnum qtr = TheTime.TheQuarter;
            Tnum lastN = this.EverPer(qtr).CountPastNIntervals(qtr, numberOfQtrs + 1, 1);
            return this & lastN >= numberOfQtrs;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// full, consecutive calendar years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveFullCalYears(int numberOfYears)
        {
            Tnum lastN = this.AlwaysPer(TheYear).CountPastNIntervals(TheYear, numberOfYears + 1, 1);
            return this & lastN >= numberOfYears;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// partial, consecutive calendar years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutivePartCalYears(int numberOfYears)
        {
            Tnum year = TheTime.TheYear;
            Tnum lastN = this.EverPer(year).CountPastNIntervals(year, numberOfYears + 1, 1);
            return this & lastN >= numberOfYears;
        }

        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive days, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveDays(int numberOfDays)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Day, numberOfDays);
        }
        
        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive weeks, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveWeeks(int numberOfWeeks)
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
        public Tbool ConsecutiveMonths(int numberOfMonths)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Month, numberOfMonths);
        }
        
        /// <summary>
        /// Returns true whenever (1) a Tbool has been true for the previous n 
        /// consecutive years, and (2) it is still true.
        /// </summary>
        public Tbool ConsecutiveYears(int numberOfYears)
        {
            return ForConsecutiveIntervals(Time.IntervalType.Year, numberOfYears);
        }
        
        /// <summary>
        /// General function that determines whether a Tbool is true for a given number
        /// of intervals of a specified type. 
        /// </summary>
        private Tbool ForConsecutiveIntervals(Time.IntervalType type, int numberOfIntervals)
        {
            if (this.IsUnknown) { return new Tbool(); }
            
            Tbool result = new Tbool();
            result.AddState(Time.DawnOf, false);
            
            SortedList<DateTime, object> t = this.TimeLine;
            
            for (int i = 0; i < t.Count; i++)
            {
                if (Convert.ToBoolean(t.Values[i]) == true)
                {
                    DateTime start = t.Keys[i];
                    DateTime thresholdReached = start.AddInterval(type, numberOfIntervals);

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