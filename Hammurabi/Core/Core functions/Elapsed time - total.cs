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
        /// Returns the total number of elapsed intervals between two dates.
        /// </summary>
        public Tnum TotalElapsedIntervals(Tnum interval, Tdate start, Tdate end)
        {
            Tnum rei = RunningElapsedIntervals(interval);

            return rei.AsOf(end) - rei.AsOf(start);
        }

//        /// <summary>
//        /// Returns the total elapsed days, between two given DateTimes, during
//        /// which a Tbool is true. 
//        /// </summary>
//		  /// NOT DELETING BECAUSE THIS IS VERY FAST
//        public Tnum TotalElapsedDays(Tdate start, Tdate end)  
//        {
//            // This unknown-handling logic is not totally correct:
//
//            // If start or end dates are unknown...
//            Hstate top = PrecedingState(start.FirstValue, end.FirstValue);
//            if (top != Hstate.Known) return new Tnum(new Hval(null,top));
//
//            // If base Tnum is ever unknown during the time period, return 
//            // the state with the proper precedence
//            Hstate returnState = PrecedenceForMissingTimePeriods(this);
//            if (returnState != Hstate.Known) return new Tnum(returnState);
//
//            // Get start and end dates
//            DateTime startDT = Convert.ToDateTime(start.FirstValue.Val);
//            DateTime endDT = Convert.ToDateTime(end.FirstValue.Val);
//
//            // Else, calculate how much time has elapsed while the Tvar had that value.
//            double days = Convert.ToDouble(TotalElapsedTime(startDT, endDT).TotalDays);
//            return new Tnum(days);
//        }
    }    
}