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
        /// Provides a running count of how many years a Tbool has been continuously true.
        /// </summary>
        public Tnum YearsContinuouslyTrue 
        {
            get
            {
                return Max(this.ContinuousElapsedIntervals(TheYear) - 1, 0);
            }
        }

        /// <summary>
        /// Provides a running count of how many months a Tbool has been continuously true.
        /// </summary>
        /// <remarks>
        /// Use judiciously, as this can involve thousands of time intervals.
        /// </remarks>
        public Tnum MonthsContinuouslyTrue 
        {
            get
            {
                return Max(this.ContinuousElapsedIntervals(TheMonth) - 1, 0);
            }
        }

        /// <summary>
        /// Provides a running count of how many weeks a Tbool has been continuously true.
        /// </summary>
        /// <remarks>
        /// Use judiciously, as this can involve thousands of time intervals.
        /// </remarks>
        public Tnum WeeksContinuouslyTrue 
        {
            get
            {
                return Max(this.ContinuousElapsedIntervals(TheCalendarWeek) - 1, 0);
            }
        }

        /// <summary>
        /// Provides a running count of how many days a Tbool has been continuously true.
        /// </summary>
        /// <remarks>
        /// Use judiciously, as this can involve tens of thousands of time intervals.
        /// </remarks>
        public Tnum DaysContinuouslyTrue
        {
            get
            {
                return Max(this.ContinuousElapsedIntervals(TheDay) - 1, 0);
            }
        }

        /// <summary>
        /// Provides a running count of how many whole intervals a Tbool 
        /// has been continuously true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--FTFTTF-->
        ///     tb.ICT = <--010120-->
        /// </remarks>
        public Tnum ContinuousElapsedIntervals(Tnum interval)  
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tnum(baseState);

            int intervalCount = 0;

            Tnum result = new Tnum();

            // Iterate through the time intervals in the input Tnum
            for (int i=0; i < interval.IntervalValues.Count-1; i++)
            {
                DateTime start = interval.IntervalValues.Keys[i];
                DateTime end = interval.IntervalValues.Keys[i+1];

                // If base Tbool is always true during the interval, increment the count
                if (this.IsAlwaysTrue(start, end))
                {
                    intervalCount++;
                }
                else
                {
                    intervalCount = 0;
                }

                result.AddState(start, intervalCount);
            }

            return result.Lean;
        }
    }
}