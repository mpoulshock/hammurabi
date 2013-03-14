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
        /// over a given type of time interval to obtain a running total.
        /// </summary>
        /// <example>
        /// Calculate lifetime accrued income, given a person's annual income:
        /// 
        ///   AccruedIncome = AnnualIncome.Accumulated(TheYear)
        /// 
        /// The time units cancel: [$/year] * [year] yields [$].
        /// </example>
        public Tnum Accumulated(Tnum interval)  
        {
            // If base Tnum is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tnum(baseState);

            // Start accumulating...
            Tnum result = new Tnum(0);
            decimal total = 0;

            // Iterate through the intervals, totaling
            for (int i=1; i < interval.TimeLine.Count-1; i++)
            {
                // TODO: end v. beginning of interval?
                total += this.AsOf(interval.TimeLine.Keys[i]).ToHardDecimal;
                result.AddState(interval.TimeLine.Keys[i], total);
            }
            
            return result.Lean;
        }

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
        //  TODO: Work on performance.  AO(90, days) takes ~ 2 sec.
        public Tnum AccumulatedOver(Tnum numberOfIntervals, Tnum interval)  
        {
            // Handle eternal values
            if (this.IsEternal)
            {
                // If base Tnum is ever unknown during the time period, return 
                // the state with the proper precedence
                Hstate baseState = PrecedenceForMissingTimePeriods(this);
                if (baseState != Hstate.Known) return new Tnum(baseState);
                
                return this * numberOfIntervals;
            }
            
            // Start accumulating...
            int num = numberOfIntervals.ToHardInt;  // TODO: Handle unknowns
            
            // Get first accumulated value
            decimal firstVal = 0;
            for (int j=0; j<num; j++)
            {
                // Don't walk off the end of the timeline
                if (j < interval.TimeLine.Count)
                {
                    firstVal += this.AsOf(interval.TimeLine.Keys [j]).ToHardDecimal;
                }
            }
            Tnum result = new Tnum(firstVal);
            
            // Iterate through the subsequent intervals
            decimal previousVal = firstVal;
            for (int i=1; i < interval.TimeLine.Count-num; i++)
            {
                // Take the value from the last iteration, and slide it the time window to the right, 
                // subtracting the left interval and adding the new right one.
//                decimal lastOldInterval = Convert.ToDecimal(this.ObjectAsOf(interval.TimeLine.Keys[i-1]).Val);
//                decimal nextNewInterval = Convert.ToDecimal(this.ObjectAsOf(interval.TimeLine.Keys[i+num-1]).Val);
                decimal lastOldInterval = this.AsOf(interval.TimeLine.Keys[i-1]).ToHardDecimal;
                decimal nextNewInterval = this.AsOf(interval.TimeLine.Keys[i+num-1]).ToHardDecimal;
                decimal newVal = previousVal - lastOldInterval + nextNewInterval;

                // Only add changepoint if value actually changes
                if (newVal != previousVal)
                {
                    result.AddState(interval.TimeLine.Keys[i], newVal);
                }

                // Set for next iteration
                previousVal = newVal;
            }
            
            return result;
        }
    }
}