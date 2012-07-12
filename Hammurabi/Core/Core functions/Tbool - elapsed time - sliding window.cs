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
        /// Provides a running count of how many intervals years a Tbool 
        /// has been true within some sliding window of time.
        /// </summary>
        public Tnum ElapsedYearsInSlidingWindow(int windowSize, string windowIntervalType)  
        {
            int days = SizeOfWindowInDays(windowSize, windowIntervalType);
            return ElapsedDaysInSlidingWindow(days) / 365.24;
        }

        /// <summary>
        /// Provides a running count of how many days years a Tbool 
        /// has been true within some sliding window of time.
        /// </summary>
        public Tnum ElapsedDaysInSlidingWindow(int windowSize, string windowIntervalType)  
        {
            int days = SizeOfWindowInDays(windowSize, windowIntervalType);
            return ElapsedDaysInSlidingWindow(days);
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
        ///                     tb = <-TTT-TTT->   where T = true and "-" = false
        ///     tb.EDISW(3, "Day") = <001232223>
        /// </remarks>
        private Tnum ElapsedDaysInSlidingWindow(int windowSizeInDays)  
        {
            // Range of time to be analyzed
            DateTime start = Time.DawnOf;
            DateTime end = Time.EndOf;

            // If possible, narrow the range of analysis (for performance reasons)
//            if (this.IsEverTrue()) 
//            {
//                start = this.DateFirstTrue.ToDateTime.AddDays(windowSizeInDays * -1);
//                end = this.DateLastTrue.ToDateTime.AddDays(windowSizeInDays + 2);
//            }

            Tnum theDay = Time.IntervalsSince(start, end, Time.IntervalType.Day, 0);

            return CountPastNIntervals(theDay, windowSizeInDays + 1, 1);
        }

        /// <summary>
        /// Computes the number of days in a time period consisting of N intervals of T type.
        /// </summary>
        /// <example>
        /// SizeOfWindowInDays(2, "Week") = 14
        /// </example>
        private int SizeOfWindowInDays(int windowSize, string windowIntervalType) 
        {
            double factor = 1;

            if (windowIntervalType == "Year")         factor = 365.24;
            else if (windowIntervalType == "Quarter") factor = 91.31;
            else if (windowIntervalType == "Month")   factor = 30.436;
            else if (windowIntervalType == "Week")    factor = 7;

            return Convert.ToInt32(windowSize * factor);
        }
    }
}