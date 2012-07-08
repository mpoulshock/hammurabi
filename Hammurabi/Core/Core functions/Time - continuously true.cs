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
    /*
     * TODO:
     *  - Overflows
     *  - Performance
     *  - Years, months, weeks, days
     *  - Uncertainty
     *  - Unit tests
     */

    public partial class Tbool
    {
        /// <summary>
        /// Provides a running count of how many days a Tbool has been continuously true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--f--|--t--|-f-|---t---|--f-->
        ///     tb.DCT = <--0--|01234|-0-|0123456|--0-->
        /// </remarks>
        public Tnum YearsContinuouslyTrue()  
        {
            // TODO: Handle uncertainty

            Tnum result = new Tnum();

            // Iterate through the time intervals in the Tbool
            for (int i=0; i < this.IntervalValues.Count; i++)
            {
                // Get interval start date
                DateTime intervalStart = this.IntervalValues.Keys[i];

                if (this.IntervalValues.Values[i].IsTrue) 
                {
                    // How many days are in this interval?
                    DateTime nextIntervalStart = new DateTime();
                    if (i == this.IntervalValues.Count-1)
                    {
                        nextIntervalStart = Time.EndOf;
                    }
                    else
                    {
                        nextIntervalStart = this.IntervalValues.Keys[i+1];
                    }

                    // How many days are in this interval?
                    TimeSpan diff = nextIntervalStart - intervalStart;
                    int timeDiff = (int)(diff.TotalDays / 365);

                    // Begin counting days
                    for (int c=0; c<timeDiff; c++)
                    {
                        result.AddState(intervalStart.AddYears(c), c);
                    }
                }
                else
                {
                    // Do nothing; count is 0 throughout the interval.
                     result.AddState(intervalStart,0);
                }
            }
            
            return result;
        }
    }
}