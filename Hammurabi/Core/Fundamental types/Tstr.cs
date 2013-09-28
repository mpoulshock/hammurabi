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
    #pragma warning disable 660, 661
    
    /// <summary>
    /// A string object whose value changes at various  points in time. 
    /// </summary>
    public partial class Tstr : Tvar
    {
        
        /// <summary>
        /// Constructs an unknown Tstr. 
        /// </summary>
        public Tstr()
        {
        }
        
        /// <summary>
        /// Constructs a Tstr that is eternally set to a given value. 
        /// </summary>
        public Tstr(string val)
        {
            this.SetEternally(val);
        }

        public Tstr(Hval val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Implicitly converts strings to Tstrs.
        /// </summary>
        public static implicit operator Tstr(string s) 
        {
            return new Tstr(s);
        }
        
        /// <summary>
        /// Removes redundant intervals from a Tstr. 
        /// </summary>
        public Tstr Lean
        {
            get
            {
                return this.LeanTvar<Tstr>();
            }
        }
        
        /// <summary>
        /// Converts a Tstr to a string.
        /// Returns null if the Tstr is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        new public string ToString
        {
            get
            {
                if (TimeLine.Count > 1) return null;

                if (!this.FirstValue.IsKnown) return null;
                
                return (Convert.ToString(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Returns the value of a Tstr at a specified point in time. 
        /// </summary>
        public Tstr AsOf(Tdate dt)
        {
            return this.AsOf<Tstr>(dt);
        }

        /// <summary>
        /// Returns a Tstr in which the values are shifted in time relative to
        /// the dates.
        /// </summary>
        public Tstr Shift(int offset, Tnum temporalPeriod)
        {
            return this.Shift<Tstr>(offset, temporalPeriod);
        }

        /// <summary>
        /// Returns a Tstr in which the last value in a time period is the
        /// final value.
        /// </summary>
        public Tstr PeriodEndVal(Tnum temporalPeriod)
        {
            return this.PeriodEndVal<Tstr>(temporalPeriod).Lean;
        }

        /// <summary>
        /// Returns true when two Tstrs are equal. 
        /// </summary>
        public static Tbool operator == (Tstr ts1, Tstr ts2)
        {
            return EqualTo(ts1,ts2);
        }
        
        /// <summary>
        /// Returns true when two Tstrs are not equal. 
        /// </summary>
        public static Tbool operator != (Tstr ts1, Tstr ts2)
        {
            return NotEqualTo(ts1,ts2);
        }

        /// <summary>
        /// Concatenates two or more Tstrs. 
        /// </summary>
        public static Tstr operator + (Tstr ts1, Tstr ts2)    
        {
            return ApplyFcnToTimeline<Tstr>(x => Concat(x), ts1, ts2);
        }
        private static Hval Concat(List<Hval> list)
        {
            return Convert.ToString(list[0].Val) + Convert.ToString(list[1].Val);
        }

    }
    
    #pragma warning restore 660, 661
}
