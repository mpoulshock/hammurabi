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

namespace Hammurabi
{    
    public partial class Tbool
    {
        /// <summary>
        /// Provides a running count of how many years a Tbool has been true.  
        /// The count carries over from one true interval to the next.
        /// </summary>
        /// <remarks>
        /// This method has much better performance than RunningElapsedYears, which
        /// should be used only if fractional years are relevant.
        /// </remarks>            
        public Tnum RunningElapsedWholeYears
        {
            get
            {
                return RunningElapsedTime(Time.IntervalType.Year);
            }
        }

        /// <summary>
        /// Provides a running count of how many years a Tbool has been true.  
        /// The count carries over from one true interval to the next.
        /// </summary>
        public Tnum RunningElapsedYears
        {
            get
            {
                Tnum unrounded = RunningElapsedTime(Time.IntervalType.Day) / 365;
                return unrounded.RoundDown(1);
            }
        }

        // TODO: Implement RunningElapsedMonths using a better method...

        /// <summary>
        /// Provides a running count of how many weeks a Tbool has been true.  
        /// The count carries over from one true interval to the next.
        /// </summary>
        public Tnum RunningElapsedWeeks
        {
            get
            {
                Tnum unrounded = RunningElapsedTime(Time.IntervalType.Day) / 7;
                return unrounded.RoundDown(1);
            }
        }

        /// <summary>
        /// Provides a running count of how many days a Tbool has been true.  
        /// The count carries over from one true interval to the next.
        /// </summary>
        public Tnum RunningElapsedDays
        {
            get
            {
                return RunningElapsedTime(Time.IntervalType.Day);
            }
        }

        /// <summary>
        /// Provides a running count of how many intervals (years, days, etc.) a Tbool 
        /// has been true.  The count carries over from one true interval to the next.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--f--|--t--|-f-|--t--|--f-->
        ///     tb.DCT = <--0--|01234|-4-|56789|--9-->
        /// 
        /// Use these methods judiciously, as they can involve tens of thousands of intervals.
        /// </remarks>
        private Tnum RunningElapsedTime(Time.IntervalType interval)  
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tnum(baseState);

            Tnum result = new Tnum();
            int count = 0;

            // Iterate through the time intervals in the Tbool
            for (int i=0; i < this.IntervalValues.Count; i++)
            {
                // Get interval start date
                DateTime intervalStart = this.IntervalValues.Keys[i];

                if (this.IntervalValues.Values[i].IsTrue) 
                {
                    // Determine the end of the interval
                    DateTime nextIntervalStart = new DateTime();
                    if (i == this.IntervalValues.Count-1)
                    {
                        nextIntervalStart = Time.EndOf.AddYears(-1);
                    }
                    else
                    {
                        nextIntervalStart = this.IntervalValues.Keys[i+1];
                    }

                    // Variables to keep track of count and date
                    DateTime indexDate = intervalStart;
            
                    // Begin counting off intervals
                    while (indexDate < nextIntervalStart) 
                    {
                        result.AddState(indexDate, count);
                        count++;
                        indexDate = indexDate.AddInterval(interval, 1);
                    }
                }
                else
                {
                    result.AddState(intervalStart, count);
                }
            }
            
            return result.Lean;
        }
    }
}