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
	#pragma warning disable 660, 661
	
	/// <summary>
	/// An temporal object that represents boolean values along a timeline.
	/// </summary>
	public partial class Tbool : Tvar
	{
		
		/// <summary>
		/// Constructs an "unknown" Tbool - that is, one with no states 
		/// </summary>
		public Tbool()
		{
		}
		
		/// <summary>
		/// Constructs a Tbool that is eternally set to a specified boolean value
		/// </summary>
		public Tbool(bool val)
		{
			this.SetEternally(val);
		}
		
		/// <summary>
		/// Removes redundant intervals from the Tbool. 
		/// </summary>
		public Tbool Lean
		{
			get
			{
				return this.LeanTvar<Tbool>();
			}
		}

		/// <summary>
		/// Converts a Tbool to a nullable boolean.
		/// Returns null if the Tbool is unknown or if it's value changes over
		/// time (that is, if it's not eternal).
		/// </summary>
		public bool? ToBool
		{
			get
			{
				if (this.IsUnknown || TimeLine.Count > 1) { return null; }
				
				return (Convert.ToBoolean(TimeLine.Values[0]));
			}
		}
		
		/// <summary>
		/// Returns the value of a Tbool at a specified point in time. 
		/// </summary>
		public Tbool AsOf(DateTime dt)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			return (Tbool)this.AsOf<Tbool>(dt);
		}
		
		/// <summary>
		/// Returns true if the Tbool always has a specified boolean value. 
		/// </summary>
		public Tbool IsAlways(bool val)
		{
			return IsAlwaysTvar<Tbool>(val, Time.DawnOf, Time.EndOf);
		}
		
		/// <summary>
		/// Returns true if the Tbool always has a specified boolean value
		/// between two given dates. 
		/// </summary>
		public Tbool IsAlways(bool val, DateTime start, DateTime end)
		{
			return IsAlwaysTvar<Tbool>(val, start, end);
		}
		
		/// <summary>
		/// Returns true if the Tbool ever has a specified boolean value. 
		/// </summary>
		public Tbool IsEver(bool val)
		{
			return IsEverTvar(val);
		}
		
		/// <summary>
		/// Returns true if the Tbool ever has a specified boolean value
		/// between two given dates. 
		/// </summary>
		public Tbool IsEver(bool val, DateTime start, DateTime end)
		{
			return IsEverTvar<Tbool>(val, start, end);
		}
		
		/// <summary>
		/// Indicates, for each time interval in a given Tnum, whether the Tbool
		/// is ever true during that interval.
		/// </summary>
		public Tbool EverPer(Tnum intervals)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			IList<DateTime> tPoints = intervals.TimePoints();
			
			// Check each time interval to see if condition is true
			for (int i = 0; i < tPoints.Count-1; i++) 
			{
				bool? isEverTrue = this.IsEver(true, tPoints[i], tPoints[i+1]).ToBool;
				result.AddState(tPoints[i], isEverTrue);
			}
			
            // This doesn't use .Lean because the output of EverPer() is often
            // the input to a function that counts the number of discrete 
            // intervals.  If you want a "lean" result, append .Lean when using
            // this function.
			return result;
		}
		
		/// <summary>
		/// Indicates, for each time interval in a given Tnum, whether the Tbool
		/// is always true during that interval.
		/// </summary>
		public Tbool AlwaysPer(Tnum intervals)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			IList<DateTime> tPoints = intervals.TimePoints();
			
			// Foreach interval in intervals
			for (int i = 0; i < tPoints.Count-1; i++) 
			{
				bool? isAlwaysTrue = this.IsAlways(true, tPoints[i], tPoints[i+1]).ToBool;
				result.AddState(tPoints[i], isAlwaysTrue);
			}
			
            // Doesn't use .Lean.  See explanation in EverPer() above.
			return result;
		}
        
        /// <summary>
        /// Returns the total elapsed days that a Tbool is true,
        /// for each of a given set of intervals.
        /// </summary>
        public Tnum ElapsedDaysPer(Tnum period)
        {
            return ElapsedDaysPer(period, true);
        }
        
        /// <summary>
        /// Returns the total elapsed days during which a Tbool is true. 
        /// </summary>
        public Tnum ElapsedDays()
        {
            return ElapsedDays(true, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns the total elapsed days, between two given DateTimes, during
        /// which a Tbool is true. 
        /// </summary>
        public Tnum ElapsedDays(DateTime start, DateTime end)
        {
            return ElapsedDays(true, start, end);
        }
        
        /// <summary>
        /// Returns a total count of the number of complete subintervals
        /// within each interval in which the Tvar (this) is true.
        /// </summary>
        /// <remarks>
        /// This function should be used as an extension method to EverPerInterval()
        /// or AlwaysPerInterval().
        /// Example: Count the number of weeks each year during which
        /// a person was employed.
        /// </remarks>
        // TODO: Add support for counting partial subintervals
        public Tnum CountPer(Tnum intervals)
        {
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            SortedList<DateTime, object> big = intervals.IntervalValues;
            SortedList<DateTime, object> small = this.IntervalValues;
            
            for (int b = 0; b < big.Count-1; b++ ) 
            {
                int count = 0;
                DateTime bStart = big.Keys[b];
                DateTime bEnd = big.Keys[b+1];
                
                for (int s = 0; s < small.Count-1; s++ ) 
                {
                    DateTime sStart = small.Keys[s];
                    DateTime sEnd = small.Keys[s+1];
                    
                    if (sStart >= bStart && sEnd <= bEnd && this.AsOf(sStart).ToBool == true)
                    {
                        count++;
                    }
                }
                
                result.AddState(bStart,count);
                count = 0;
            }
        
            return result.Lean;
        }
        
        /// <summary>
        /// Returns a running count (over time) of the number of subintervals
        /// within each interval in which the Tvar (this) is true.
        /// </summary>
        /// <remarks>
        /// This function should be used as an extension method to EverPerInterval()
        /// or AlwaysPerInterval().
        /// Example: Count the number of weeks each year during which
        /// a person was employed.  The first week of employment would be 
        /// numbered 0, the second 1, etc.
        /// </remarks>
        // TODO: Fix broken test case for this function.
        public Tnum RunningCountPer(Tnum intervals)
        {
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            result.AddState(Time.DawnOf,0);
            
            int count = 0;
            decimal? prevBig = 0;
            SortedList<DateTime, object> sub = this.IntervalValues;
            
            // Iterate through the sub-intervals
            for (int i = 0; i < sub.Count-1; i++ ) 
            {
                DateTime dt = sub.Keys[i];
                
                // Reset count for each new (big, not sub-) interval
                decimal? big = intervals.AsOf(dt).ToDecimal;
                if (big != prevBig) count = 0;
                prevBig = big;
                
                // If the Tbool is true during the subinterval, increment
                // the subsequent subinterval
                if (this.AsOf(dt).ToBool == true)
                {
                    count++;
                }
                
                result.AddState(sub.Keys[i+1], count);
            }
        
            return result.Lean;
        }
        
        /// <summary>
        /// Returns the number of intervals that the Tbool (this) is true,
        /// going back a specified number of intervals.
        /// </summary>
        /// <remarks>
        /// The current interval is counted.  So, for example, if the range
        /// equals 2, the function would evaluate the current interval and the
        /// prior interval.
        /// </remarks>
        public Tnum CountPastNIntervals(Tnum intervals, int range)
        {
            if (this.IsUnknown || intervals.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            SortedList<DateTime, object> t = intervals.IntervalValues;
            
            // Count forward from the beginning of time through each interval
            for (int b = 0; b < t.Count-1; b++ ) 
            {
                int count = 0;
                
                // Look back the specified number of intervals
                for (int n = 0; n < range; n++)
                {
                    int index = b-n;
                    if (index >= 0) 
                    {
                        if (this.AsOf(t.Keys[index]).ToBool == true)
                        {
                            count++;
                        }
                    }
                }
                
                result.AddState(t.Keys[b],count);
            }
      
            return result.Lean;
        }
        
		/// <summary>
		/// Overloaded boolean operator: True.
		/// </summary>
		public static bool operator true (Tbool tb)
        {
            return tb.IsTrue;
        }
        
        /// <summary>
        /// Returns true only if the Tbool is true during the window of concern;
        /// otherwise false. 
        /// </summary>
        public bool IsTrue
        {
            get
            {
                if (Facts.WindowOfConcernIsDefault)
                {
                    return this.IntervalValues.Count == 1 && 
                           Convert.ToBoolean(this.IntervalValues.Values[0]) == true; 
                }
                if (Facts.WindowOfConcernIsPoint)
                {
                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
                }
                
                return Convert.ToBoolean(this.IsAlways(true, 
                                                       Facts.WindowOfConcernStart, 
                                                       Facts.WindowOfConcernEnd
                                                       ).ToBool);
            }
        }
        
		/// <summary>
		/// Overloaded boolean operator: False.
		/// </summary>
        public static bool operator false (Tbool tb)
        {
            return tb.IsFalse;
        }
		
        /// <summary>
        /// Returns true only if the Tbool is false during the window of concern;
        /// otherwise true. 
        /// </summary>
        public bool IsFalse
        {
            get
            {
                if (Facts.WindowOfConcernIsDefault)
                {
                    return this.IntervalValues.Count == 1 && 
                           Convert.ToBoolean(this.IntervalValues.Values[0]) == false; 
                }
                if (Facts.WindowOfConcernIsPoint)
                {
                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
                }
                
                return Convert.ToBoolean(this.IsAlways(false, 
                                                       Facts.WindowOfConcernStart, 
                                                       Facts.WindowOfConcernEnd
                                                       ).ToBool);
            }
        }
        
		/// <summary>
		/// Overloaded boolean operator: Equal.
		/// </summary>
		public static Tbool operator == (Tbool tb1, Tbool tb2)
		{
			return EqualTo(tb1,tb2);
		}
		public static Tbool operator == (Tbool tb, bool b)
		{
			return EqualTo(tb,new Tbool(b));
		}
		public static Tbool operator == (bool b, Tbool tb)
		{
			return EqualTo(tb,new Tbool(b));
		}
		
		/// <summary>
		/// Overloaded boolean operator: Not equal.
		/// </summary>
		public static Tbool operator != (Tbool tb1, Tbool tb2)
		{
			return !EqualTo(tb1,tb2);
		}
		public static Tbool operator != (Tbool tb, bool b)
		{
			return !EqualTo(tb,new Tbool(b));
		}
		public static Tbool operator != (bool b, Tbool tb)
		{
			return !EqualTo(tb,new Tbool(b));
		}

	}
	
	#pragma warning restore 660, 661
}