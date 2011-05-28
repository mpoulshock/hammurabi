// Copyright (c) 2011 The Hammurabi Project
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
        
        /// <summary>
        /// Constructs a Tnum set eternally to a specified value.
        /// This value can be an int, double, or decimal.
        public Tnum(object val)
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
                if (this.IsUnknown || TimeLine.Count > 1) { return null; }
                
                return (Convert.ToInt32(TimeLine.Values[0]));
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
                if (this.IsUnknown || TimeLine.Count > 1) { return null; }
                
                return (Convert.ToDecimal(TimeLine.Values[0]));
            }
        }
        
        /// <summary>
        /// Returns the value of the Tnum at a specified point in time.
        /// </summary>
        public Tnum AsOf(DateTime dt)
        {
            if (this.IsUnknown) { return new Tnum(); }
            
            return (Tnum)this.AsOf<Tnum>(dt);
        }
        
        
        // ********************************************************************
        // IsAlways / IsEver / DateFirst
        // ********************************************************************
        
        // TODO: Make inputs ints, decimals, or something...
        
        /// <summary>
        /// Returns true if the Tnum always has the specified value. 
        /// </summary>
        public Tbool IsAlways(object val)
        {
            return IsAlwaysTvar<Tnum>(val, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns true if the Tnum always has the specified value between
        /// two given dates. 
        /// </summary>
        public Tbool IsAlways(object val, DateTime start, DateTime end)
        {
            return IsAlwaysTvar<Tnum>(val, start, end);
        }
        
        /// <summary>
        /// Returns true if the Tnum ever has the specified value. 
        /// </summary>
        public Tbool IsEver(object val)
        {
            return IsEverTvar(val);
        }
        
        /// <summary>
        /// Returns true if the Tnum ever has the specified value between
        /// two given dates. 
        /// </summary>
        public Tbool IsEver(object val, DateTime start, DateTime end)
        {
            return IsEverTvar<Tnum>(val, start, end);
        }
        
        /// <summary>
        /// Returns the DateTime when the Tnum first has a given value
        /// </summary>
        public DateTime DateFirst(decimal val) 
        {
            return DateFirst<Tnum>(val);
        }
        public DateTime DateFirst(int val) 
        {
            return DateFirst<Tnum>(val);
        }
        public DateTime DateFirst(double val) 
        {
            return DateFirst<Tnum>(val);
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
            if (this.IsUnknown) { return new Tnum(); }
                
            Tnum result = new Tnum();
            
            decimal max = Convert.ToDecimal(TimeLine.Values[0]);
            
            foreach(object s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s) > max)
                {
                    max = Convert.ToDecimal(s);
                }
            }
            
            result.SetEternally(max);
            
            return result;
        }
        
        /// <summary>
        /// Returns the all-time minimum value of the Tnum. 
        /// </summary>
        public Tnum Min() 
        {     
            if (this.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            decimal min = Convert.ToDecimal(TimeLine.Values[0]);
            
            foreach(object s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s) < min)
                {
                    min = Convert.ToDecimal(s);
                }
            }
            
            result.SetEternally(min);
            
            return result;
        }
        
        
        // ********************************************************************
        //    To U.S. dollars
        // ********************************************************************    
        
        /// <summary>
        /// Converts a Tnum to a Tstr formatted as U.S. dollars
        /// </summary>
        public Tstr ToUSD
        {
            get
            {
                return TheToUSD(this);
            }
        }

        /// <summary>
        /// Private non-temporal ToUSDollars function
        /// </summary>
        private static Tstr TheToUSD(Tnum input)
        {
            if (input.IsUnknown) { return new Tstr(); }
            
            Tstr result = new Tstr();
            
            foreach (KeyValuePair<DateTime,object> slice in input.IntervalValues)
            {
                // TODO: Displays negative values as parentheses - fix
                
                string r = String.Format("{0:C}" ,Convert.ToDecimal(slice.Value)).TrimStart('$');
                
                result.AddState(slice.Key, r);
            }
            
            return result;
        }

                                                                 
    }
}
