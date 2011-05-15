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
	/// The (abstract) base class of all temporal variables (Tvars).
	/// Ordinarily, the functions in this class should only be called by core
	/// functions, not law-related ones.
	/// </summary>
	public abstract class Tvar : H 
	{
		/// <summary>
		/// The core Tvar data structure: a timeline of dates and associated values.
		/// </summary>
		protected SortedList<DateTime, object> TimeLine = new SortedList<DateTime, object>();
		
		/// <summary>
		/// The accessor for TimeLine.
		/// </summary>
		public SortedList<DateTime, object> IntervalValues
		{
			get
			{
				return TimeLine;
			}
		}

		/// <summary>
		/// Indicates whether a Tvar is "unknown" - that is, if it has no
		/// time-value states. 
		/// </summary>
		public bool IsUnknown
		{
			get
			{
				return TimeLine.Count == 0;
			}
		}
		
        /// <summary>
        /// Indicates whether a Tvar is "known" - that is, if it has at least
        /// one time-value state. 
        /// </summary>
//        public Tbool IsKnown
//        {
//            get
//            {
//                return new Tbool(TimeLine.Count > 0);
//            }
//        }
        
		/// <summary>
		/// Adds an time-value state to the TimeLine. 
		/// </summary>
		public void AddState(DateTime dt, object val)
		{
            // If a state (DateTime) is added to a Tvar that already has that 
            // state, an error occurs.  We could check for duplicates here, but
            // this function is used extremely frequently and doing so would
            // probably affect performance.
			TimeLine.Add(dt,val);
		}
		
		/// <summary>
		/// Sets a Tvar to an "eternal" value (the same at all points in time). 
		/// </summary>
		public void SetEternally(object val)
		{
			TimeLine.Add(Time.DawnOf,val);	
		}

		/// <summary>
		/// Displays a timeline showing the state of the object at various
		/// points in time.
		/// </summary>
		public string Timeline
		{
			get 
			{  
				string result = "";
				
				if (IsUnknown)
				{
					result = "Unknown";
				}
				else
				{
					foreach(KeyValuePair<DateTime,object> de in this.TimeLine)
					{
						// TODO: Create timeline output for null objects
						
		       	    	result += de.Key + " " + Convert.ToString(de.Value) + "\n";	
					}
				}
				return result;
		  	}
		}
		
		/// <summary>
		/// Displays a timeline indicating the state of the object at various
		/// points in time.  Same as .Timeline but without line breaks.
		/// Used for test cases only.
		/// </summary>
		public string TestOutput
		{
			get
			{
				return Timeline.Replace("\n"," ");
			}
		}
		
		/// <summary>
		/// Returns the value of a Tvar at a specified point in time. 
		/// </summary>
		public object AsOf<T>(DateTime dt) where T : Tvar
		{
			SortedList<DateTime, object> line = TimeLine;
			
			object result = line.Values[line.Count-1];
			
			for (int i = 0; i < line.Count-1; i++ ) 
			{
				// If value is between two adjacent points on the timeline...
				if (dt >= line.Keys[i])
				{
					if (dt < line.Keys[i+1])
					{
						result = line.Values[i];
					}
				}
			}

			return Auxiliary.ReturnProperTvar<T>(result);
		}
		
		/// <summary>
		/// Returns an object value of the Tvar at a specified point in time.
		/// </summary>
		public object ObjectAsOf(DateTime dt)
		{
			for (int i = 0; i < TimeLine.Count-1; i++ ) 
			{
				// If value is between two adjacent points on the timeline...
				if (dt >= TimeLine.Keys[i])
				{
					if (dt < TimeLine.Keys[i+1])
					{
						return TimeLine.Values[i];
					}
				}
			}
			
			// If value is on or after last point on timeline...
			return TimeLine.Values[TimeLine.Count-1];
		}
		
		/// <summary>
		/// Removes redundant intervals from the Tvar. 
		/// </summary>
		public T LeanTvar<T>() where T : Tvar
		{
			T n = (T)this;
			
			// Identify redundant intervals
			List<DateTime> dupes = new List<DateTime>();
			
			if (TimeLine.Count > 0)
			{
				for (int i=0; i < TimeLine.Count-1; i++ ) 
				{
					if (object.Equals(TimeLine.Values[i+1],TimeLine.Values[i]))
					{
						dupes.Add(TimeLine.Keys[i+1]);
					}
				}
			}
			
			// Remove redundant intervals
			foreach (DateTime d in dupes)
			{
				TimeLine.Remove(d);	
			}
				
			return n;
		}	

		/// <summary>
		/// Gets all points in time at which value of the Tvar changes.
		/// </summary>
		public IList<DateTime> TimePoints()
		{
			return (IList<DateTime>)TimeLine.Keys;
		}
		
		
		// **********************************************************
		//	Equal / not equal
		// **********************************************************
		
		/// <summary>
		/// Returns true when two Tvar values are equal. 
		/// </summary>
		public static Tbool EqualTo(Tvar tb1, Tvar tb2)
		{
			// Result is unknown if any input is unknown
			if (AnyAreUnknownTvar(tb1, tb2)) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(tb1,tb2))
			{	
				result.AddState(slice.Key, object.Equals(slice.Value[0], slice.Value[1]));
			}
			
			return result.Lean;
		}
		
		
		// ********************************************************************
		// IsAlways / IsEver
		// ********************************************************************
		
		/// <summary>
		/// Returns true if the Tvar always has a given value. 
		/// </summary>
		protected Tbool IsAlwaysTvar<T>(object val, DateTime start, DateTime end) where T : Tvar
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool equalsVal = EqualTo(this, new Tnum(val)); // only works b/c Tnums take objects
			
			Tbool isDuringInterval = TheTime.IsBetween(start, end);
			
			Tbool isOverlap = equalsVal & isDuringInterval;
			
			Tbool overlapAndIntervalAreCoterminous = isOverlap == isDuringInterval;
			
			return !overlapAndIntervalAreCoterminous.IsEver(false);
		}
		
		/// <summary>
		/// Returns true if the Tvar ever has a given value. 
		/// </summary>
		protected Tbool IsEverTvar(object val)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			return new Tbool(TimeLine.ContainsValue(val));
		}
		
		/// <summary>
		/// Returns true if the Tvar ever has a given value between two given dates. 
		/// </summary>
		protected Tbool IsEverTvar<T>(object val, DateTime start, DateTime end) where T : Tvar
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool equalsVal = EqualTo(this, new Tnum(val)); // only works b/c Tnums take objects
			
			Tbool isDuringInterval = TheTime.IsBetween(start, end);
			
			Tbool isOverlap = equalsVal & isDuringInterval;
			
			return isOverlap.IsEver(true);
		}
        
        
        // ********************************************************************
        // Elapsed Time
        // ********************************************************************
        
        /// <summary>
        /// Returns the total elapsed days that a Tvar has a given value,
        /// for each of a given set of intervals.
        /// Example: meetsAnnualTest = var.ElapsedDaysPerInterval(theYear) > 183;
        /// </summary>
        public Tnum ElapsedDaysPer(Tnum period, object val)
        {
            if (this.IsUnknown || period.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            int count = period.TimeLine.Count;
            for (int i=0; i < count; i++)
            {
                DateTime spanEnd = Time.EndOf;
                if (i < count-1)
                {
                    spanEnd = period.TimeLine.Keys[i+1];
                }
                
                TimeSpan time = this.ElapsedTime(val, period.TimeLine.Keys[i], spanEnd);
                result.AddState(period.TimeLine.Keys[i], time.TotalDays);
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns the total elapsed days, between two given DateTimes, during
        /// which a Tvar has a given value.
        /// </summary>
        public Tnum ElapsedDays(object val, DateTime start, DateTime end)
        {
            if (this.IsUnknown) { return new Tnum(); }
            
            double days = Convert.ToDouble(ElapsedTime(val, start, end).TotalDays);
            return new Tnum(days);
        }
        
        /// <summary>
        /// Returns the total elapsed days during which a Tvar has a given value. 
        /// </summary>
        public Tnum ElapsedDays(object val)
        {
            return ElapsedDays(val, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns the total elapsed time, between two given DateTimes, during
        /// which a Tvar has a given value.
        /// </summary>
        private TimeSpan ElapsedTime(object val, DateTime start, DateTime end)
        {
            TimeSpan result = new TimeSpan();
            int count = this.TimeLine.Count;
            for (int i=0; i < count; i++)
            {
                if (object.Equals(this.TimeLine.Values[i], val))
                {
                    DateTime spanStart = Time.Latest(TimeLine.Keys[i],start);
                    DateTime spanEnd = end;
                    if (i < count-1)
                    {
                        spanEnd = Time.Earliest(TimeLine.Keys[i+1], end);
                    }
        
                    if (spanStart < spanEnd)
                    {
                        TimeSpan newDuration = spanEnd - spanStart;
                        result += newDuration;
                    }
                }
            }
        
            return result;
        }
        
        /// <summary>
        /// Returns the total elapsed time during which a Tvar has a given value. 
        /// </summary>
        private TimeSpan ElapsedTime(object val)
        {
            return ElapsedTime(val, Time.DawnOf, Time.EndOf);
        }

        
        // ********************************************************************
        // DateFirst()
        // ********************************************************************
        
        /// <summary>
        /// Returns the DateTime when a Tvar first has a given value.
        /// </summary>
        public DateTime DateFirst<T>(object val) where T : Tvar
        {
            SortedList<DateTime, object> line = this.TimeLine;
            
            for (int i = 0; i < line.Count; i++) 
            {
                if (object.Equals(line.Values[i], val))
                {
                    return line.Keys[i];
                }
            }

            return Time.EndOf; // need to think about this...
        }
	}	
}
