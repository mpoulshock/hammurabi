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
        /// Initializes a new instance of the Tnum class and loads
        /// a list of date-value pairs.
        /// </summary>
        public static Tnum MakeTnum(params object[] list)
        {
            Tnum result = new Tnum();
            for(int i=0; i < list.Length - 1; i+=2)  
            {
                try
                {
                    Hstate h = (Hstate)list[i+1];
                    if (h != Hstate.Known)
                    {
                        result.AddState(Convert.ToDateTime(list[i]),
                                    new Hval(null,h));
                    }
                }
                catch
                {
                    decimal d = Convert.ToDecimal(list[i+1]);
                    result.AddState(Convert.ToDateTime(list[i]),
                                new Hval(d));
                }
            }
            return result;
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
        /// Converts an eternal Tnum to a string formatted as U.S. dollars.
        /// </summary>
        public string ToUSD
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return this.FirstValue.ToString;

                return String.Format("{0:C}", Convert.ToDecimal(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Returns the value of the Tnum at a specified point in time.
        /// </summary>
        public Tnum AsOf(Tdate dt)
        {
            return this.AsOf<Tnum>(dt);
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
            if (state != Hstate.Known) 
            {
                return new Tnum(state);
            }

            // Determine the maximum value
            decimal max = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                // TODO: Handle unknowns

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
            if (state != Hstate.Known) 
            {
                return new Tnum(state);
            }

            // Determine the maximum value
            decimal min = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                // TODO: Handle unknowns

                if (Convert.ToDecimal(s.Val) < min)
                {
                    min = Convert.ToDecimal(s.Val);
                }
            }
            return new Tnum(min);
        }                                                                 
    }
}
