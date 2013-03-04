// Copyright (c) 2013 Hammura.bi LLC
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
    public partial class Tnum
    {
        /// <summary>
        /// Takes a Tnum representing some value per unit time, and accumulates it
        /// over a given number of successive time intervals.
        /// </summary>
        /// <example>
        /// Calculate income accumulated over a three month period, where the
        /// person earned $3,000/month:
        /// 
        ///   Income = MonthlyIncome.AccumulatedOver(3, TheMonth)
        /// 
        /// The time units cancel: [$/mo.] * [mo.] yields [$].
        /// </example>
        public Tnum AccumulatedOver(Tnum numberOfIntervals, Tnum interval)  
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tnum(baseState);

            // Handle eternal values
            if (this.IsEternal)
            {
                return this * numberOfIntervals;
            }

            // Start accumulating...
            int num = numberOfIntervals.ToHardInt;  // TODO: Handle unknowns

            // TODO: For efficiency, first bind base Tnum to interval Tnum

            // Set first value as accumulation of first N intervals (these cover
            // an insigificant time period near the dawn of time.
            Tnum result = new Tnum();
            decimal firstVal = Convert.ToDecimal(this.FirstValue.Val) * num;
            result.AddState(Time.DawnOf, firstVal);

            // Iterate through the intervals
            for (int i=num; i < interval.TimeLine.Count-num; i++)
            {
                decimal total = 0;

                // Add up the amount in this interval and N subsequent intervals
                for (int j=0; j<num; j++)
                {
                    // Don't walk off the end of the timeline
                    if (i+j < interval.TimeLine.Count-num)
                    {
                        total += this.AsOf(interval.TimeLine.Keys[i+j]).ToHardDecimal;
                    }
                }

                result.AddState(interval.TimeLine.Keys[i], total);
            }

            return result.Lean;
        }
    }
}