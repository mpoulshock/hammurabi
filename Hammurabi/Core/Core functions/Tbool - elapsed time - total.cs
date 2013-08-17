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
        /// Returns the total elapsed days that a Tvar has a given value,
        /// for each of a given set of intervals.
        /// Example: meetsAnnualTest = var.ElapsedDaysPerInterval(theYear) > 183;
        /// </summary>
        public Tnum TotalElapsedDaysPer(Tnum period)
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            Hstate periodState = PrecedenceForMissingTimePeriods(period);
            Hstate top = PrecedingState(baseState, periodState);
            if (top != Hstate.Known) return new Tnum(top);

            Tnum result = new Tnum();
            
            int count = period.IntervalValues.Count;
            for (int i=0; i < count; i++)
            {
                DateTime spanEnd = Time.EndOf;
                if (i < count-1)
                {
                    spanEnd = period.IntervalValues.Keys[i+1];
                }
                
                TimeSpan time = this.TotalElapsedTime(period.IntervalValues.Keys[i], spanEnd);

                // Add the state, but not if it's at the end of time
                if (period.IntervalValues.Keys[i] < Time.EndOf)
                {
                    result.AddState(period.IntervalValues.Keys[i], time.TotalDays);
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Returns the total elapsed years between two given DateTimes, during
        /// which a Tbool is true.
        /// </summary>
        public Tnum TotalElapsedYears(Tdate start, Tdate end)
        {
            return this.TotalElapsedDays(start, end) / Time.DaysPerYear;
        }

        /// <summary>
        /// Returns the total elapsed days, between two given DateTimes, during
        /// which a Tbool is true. 
        /// </summary>
        public Tnum TotalElapsedDays(Tdate start, Tdate end)  
        {
            // EXPERIMENTAL (currently much slower):
//            return TotalElapsedIntervals(start, end, TheDay);



            // This unknown-handling logic is not totally correct:

            // If start or end dates are unknown...
            Hstate top = PrecedingState(start.FirstValue, end.FirstValue);
            if (top != Hstate.Known) return new Tnum(new Hval(null,top));

            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate returnState = PrecedenceForMissingTimePeriods(this);
            if (returnState != Hstate.Known) return new Tnum(returnState);

            // Get start and end dates
            DateTime startDT = Convert.ToDateTime(start.FirstValue.Val);
            DateTime endDT = Convert.ToDateTime(end.FirstValue.Val);

            // Else, calculate how much time has elapsed while the Tvar had that value.
            double days = Convert.ToDouble(TotalElapsedTime(startDT, endDT).TotalDays);
            return new Tnum(days);
        }

        /// <summary>
        /// EXPERIMENTAL - Returns the total number of elapsed intervals between two dates.
        /// </summary>
        public Tnum TotalElapsedIntervals(Tdate start, Tdate end, Tnum interval)
        {
            Tnum rei = RunningElapsedIntervals(interval);

            return rei.AsOf(end) - rei.AsOf(start);
        }

        /// <summary>
        /// Returns the total elapsed time, between two given DateTimes, during
        /// which a Tbool is true.
        /// </summary>
        private TimeSpan TotalElapsedTime(DateTime start, DateTime end)
        {
            TimeSpan result = new TimeSpan();
            int count = this.TimeLine.Count;
            for (int i=0; i < count; i++)
            {
                if (Convert.ToBoolean(this.TimeLine.Values[i].Val))
                {
                    DateTime spanStart = Time.Latest(TimeLine.Keys[i],start);
                    DateTime spanEnd = end;
                    if (i < count-1)
                    {
                        spanEnd = Time.Earliest(TimeLine.Keys[i+1], end);
                    }
        
                    if (spanStart < spanEnd)
                    {
                        TimeSpan newDuration = spanEnd - spanStart;
                        result += newDuration;
                    }
                }
            }
        
            return result;
        }
    }    
}
