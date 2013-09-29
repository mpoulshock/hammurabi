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
using System.Collections.Generic;

namespace Akkadian
{
    /// <summary>
    /// An object that represents DateTime values along a timeline.
    /// </summary>
    public partial class Tnum : Tvar
    {
        /// <summary>
        /// Constructs an unknown Tnum. 
        /// </summary>
        public Tnum()
        {
        }

        public Tnum(int n)
        {
            this.SetEternally(n);
        }

        public Tnum(decimal n)
        {
            this.SetEternally(n);
        }

        public Tnum(double n)
        {
            this.SetEternally(n);
        }

        public Tnum(Hstate state)
        {
            this.SetEternally(state);
        }

        public Tnum(Hval val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Implicitly converts ints to Tnums.
        /// </summary>
        public static implicit operator Tnum(int i) 
        {
            return new Tnum(i);
        }
        
        /// <summary>
        /// Implicitly converts decimals to Tnums.
        /// </summary>
        public static implicit operator Tnum(decimal d) 
        {
            return new Tnum(d);
        }
        
        /// <summary>
        /// Implicitly converts doubles to Tnums.
        /// </summary>
        public static implicit operator Tnum(double d) 
        {
            return new Tnum(d);
        }
        
        /// <summary>
        /// Removes redundant intervals from the Tnum timeline. 
        /// </summary>
        public Tnum Lean
        {
            get
            {
                return this.LeanTvar<Tnum>();
            }
        }
        
        /// <summary>
        /// Converts a Tnum to a nullable integer.
        /// Returns null if the Tnum is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public int? ToInt
        {
            get
            {
                if (TimeLine.Count > 1) return null;

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToInt32(this.FirstValue.Val));
            }
        }

        /// <summary>
        /// Converts a Tnum to an integer.  Should only be used when it is
        /// not possible for the value to be unknown.
        /// </summary>
        public int ToHardInt
        {
            get
            {
                return Convert.ToInt32(this.ToInt);
            }
        }

        /// <summary>
        /// Converts a Tnum to a nullable decimal.
        /// Returns null if the Tnum is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public decimal? ToDecimal
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToDecimal(this.FirstValue.Val));
            }
        }

        /// <summary>
        /// Converts a Tnum to an decimal.  Should only be used when it is
        /// not possible for the value to be unknown.
        /// </summary>
        public decimal ToHardDecimal
        {
            get
            {
                return Convert.ToDecimal(this.ToDecimal);
            }
        }

        /// <summary>
        /// Returns the value of the Tnum at a specified point in time.
        /// </summary>
        public Tnum AsOf(Tdate dt)
        {
            return this.AsOf<Tnum>(dt);
        }

        /// <summary>
        /// Returns a Tnum in which the values are shifted in time relative to
        /// the dates.
        /// </summary>
        public Tnum Shift(int offset, Tnum temporalPeriod)
        {
            return this.Shift<Tnum>(offset, temporalPeriod);
        }

        /// <summary>
        /// Returns a Tnum in which the last value in a time period is the
        /// final value.
        /// </summary>
        public Tnum PeriodEndVal(Tnum temporalPeriod)
        {
            return this.PeriodEndVal<Tnum>(temporalPeriod).Lean;
        }

        /// <summary>
        /// Converts a Tnum value in days to the equivalent (fractional) years.
        /// </summary>
        public Tnum DaysToYears
        {
            get
            {
                return this / Time.DaysPerYear;
            }
        }

        /// <summary>
        /// Converts a Tnum value in days to the equivalent (fractional) months.
        /// </summary>
        public Tnum DaysToMonths
        {
            get
            {
                return this / Time.DaysPerMonth;
            }
        }

        /// <summary>
        /// Converts a Tnum value in days to the equivalent (fractional) weeks.
        /// </summary>
        public Tnum DaysToWeeks
        {
            get
            {
                return this / 7;
            }
        }

        // *************************************************************
        // All-time min / max
        // *************************************************************
        
        // TODO: Max(startDate,endDate), Min(startDate,endDate)
        
        /// <summary>
        /// Returns the all-time maximum value of the Tnum. 
        /// </summary>
        public Tnum Max()
        {
            // Deal with unknowns
            Hstate state = PrecedenceForMissingTimePeriods(this);
            if (state != Hstate.Known) { return new Tnum(state); }

            // Determine the maximum value
            decimal max = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s.Val) > max)
                {
                    max = Convert.ToDecimal(s.Val);
                }
            }
            return new Tnum(max);
        }


        /// <summary>
        /// Returns the all-time minimum value of the Tnum. 
        /// </summary>
        public Tnum Min() 
        {     
            // Deal with unknowns
            Hstate state = PrecedenceForMissingTimePeriods(this);
            if (state != Hstate.Known) { return new Tnum(state); }

            // Determine the maximum value
            decimal min = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s.Val) < min)
                {
                    min = Convert.ToDecimal(s.Val);
                }
            }
            return new Tnum(min);
        }

        /// <summary>
        /// Converts a Tnum to a Tstr formatted as U.S. dollars.
        /// </summary>
        public Tstr ToUSD
        {
            get
            {
                return ApplyFcnToTimeline<Tstr>(x => CoreToUSD(x), this);
            }
        }
        private static Hval CoreToUSD(Hval h)
        {
            return String.Format("{0:C}" ,Convert.ToDecimal(h.Val));
        }

        /// <summary>
        /// Converts Uncertain and Stub time periods to a given value.
        /// </summary>
        /// <remarks>
        /// Used to rid a Tnum of uncertainty.  Note that it does not convert Unstated 
        /// periods because doing so would break the backward chaining interview.
        /// </remarks>
        public Tnum NormalizedTo(decimal val)
        {
            Tnum result = new Tnum();

            foreach (KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
            {
                Hval theVal = slice.Value;
                
                if (theVal.IsUncertain || theVal.IsStub)
                {
                    result.AddState(slice.Key, val);
                }
                else
                {
                    result.AddState(slice.Key, slice.Value);
                }

            }
            
            return result.Lean;
        }
    }
}