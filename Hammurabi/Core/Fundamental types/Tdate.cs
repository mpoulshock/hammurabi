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
using System.Collections.Generic;

namespace Hammurabi
{ 
    #pragma warning disable 660, 661

    /// <summary>
    /// An object that represents DateTime values along a timeline.
    /// </summary>
    public partial class Tdate : Tvar
    {

        /// <summary>
        /// Constructs an empty Tdate.
        /// </summary>
        public Tdate()
        {
        }
        
        /// <summary>
        /// Constructs a Tdate that is eternally set to a specified DateTime
        /// value.
        /// </summary>    
        public Tdate(DateTime val)
        {
            this.SetEternally(val);
        }

        public Tdate(Hstate state)
        {
            this.SetEternally(state);
        }

        public Tdate(Hval val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Initializes a new instance of the Tdate class and loads
        /// a list of date-value pairs.
        /// </summary>
        public static Tdate MakeTdate(params object[] list)
        {
            Tdate result = new Tdate();
            for(int i=0; i < list.Length - 1; i+=2)  
            {
                DateTime point = Convert.ToDateTime(list[i]);
                try
                {
                    Hstate h = (Hstate)list[i+1];
                    if (h != Hstate.Known)
                    {
                        result.AddState(point, new Hval(null,h));
                    }
                }
                catch
                {
                    DateTime d = Convert.ToDateTime(list[i+1]);
                    result.AddState(point,
                                new Hval(d));
                }
            }
            return result;
        }
        
        /// <summary>
        /// Implicitly converts DateTimes to Tdates.
        /// </summary>
        public static implicit operator Tdate(DateTime d) 
        {
            return new Tdate(d);
        }
        
        /// <summary>
        /// Constructs a Tdate that is eternally set to a specified year, 
        /// month, and day.
        /// </summary>
        public Tdate(int year, int month, int day)
        {
            this.SetEternally(new DateTime(year, month, day));
        }
        
        /// <summary>
        /// Eliminates redundant intervals in the Tdate.
        /// </summary>
        public Tdate Lean
        {
            get
            {
                return this.LeanTvar<Tdate>();
            }
        }
        
        /// <summary>
        /// Converts a Tdate to a nullable DateTime.
        /// Returns null if the Tdate is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public DateTime? ToNullDateTime
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToDateTime(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Converts a Tdate to a DateTime.  Is dangerous because it doesn't account
        /// for unknown or time-varying values.
        /// </summary>
        public DateTime ToDateTime
        {
            get
            {
                return (Convert.ToDateTime(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Returns the value of the Tdate at a specified point in time.
        /// </summary>
        public Tdate AsOf(Tdate dt)
        {
            return this.AsOf<Tdate>(dt);
        }

        
        // ********************************************************************
        // AddTimeInterval
        // ********************************************************************
    
        /// <summary>
        /// Adds a specified number of days to each DateTime in a Tdate.
        /// </summary>        
        public Tdate AddDays(int days)
        {        
            // Handle unknowns
            if (!this.FirstValue.IsKnown)
            {
                return this;
            }

            Tdate result = new Tdate();

            foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value.Val);
                DateTime newDt = val.AddDays(days);
                result.AddState(slice.Key, newDt);
            }
            
            return result.Lean;    
        }
        
        /// <summary>
        /// Adds a specified number of months to each DateTime in a Tdate.
        /// </summary>
        public Tdate AddMonths(int months)
        {        
            // Handle unknowns
            if (!this.FirstValue.IsKnown)
            {
                return this;
            }

            Tdate result = new Tdate();

            foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value.Val);
                DateTime newDt = val.AddMonths(months);
                result.AddState(slice.Key, newDt);
            }
            
            return result.Lean;    
        }
        
        /// <summary>
        /// Adds a specified number of years to each DateTime in a Tdate.
        /// </summary>
        public Tdate AddYears(Tnum years)
        {     
            // Handle unknowns
            Hstate top = PrecedingState(this.FirstValue, Year.FirstValue);
            if (top != Hstate.Known) 
            {
                return new Tdate(new Hval(null,top));
            }

            Tdate result = new Tdate();
            foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value.Val);
                int yearsInt = Convert.ToInt32(years.FirstValue.Val);
                DateTime newDt = val.AddYears(yearsInt);
                result.AddState(slice.Key, newDt);
            }
            
            return result.Lean;    
        }

        /// <summary>
        /// Extracts the year from each value in a Tdate.
        /// </summary>
        public Tnum Year
        {       
            get
            {
                Tnum result = new Tnum();
    
                foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
                {    
                    if (!slice.Value.IsKnown)
                    {
                        result.AddState(slice.Key,slice.Value);
                    }
                    else
                    {
                        DateTime val = Convert.ToDateTime(slice.Value.Val);
                        result.AddState(slice.Key, val.Year);
                    }
                }
                
                return result.Lean;  
            }
        }

        /// <summary>
        /// Extracts the month from each value in a Tdate.
        /// </summary>
        public Tnum Month
        {       
            get
            {
                Tnum result = new Tnum();
    
                foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
                {    
                    if (!slice.Value.IsKnown)
                    {
                        result.AddState(slice.Key,slice.Value);
                    }
                    else
                    {
                        DateTime val = Convert.ToDateTime(slice.Value.Val);
                        result.AddState(slice.Key, val.Month);
                    }
                }
                
                return result.Lean;  
            }
        }

        /// <summary>
        /// Extracts the day from each value in a Tdate.
        /// </summary>
        public Tnum Day
        {       
            get
            {
                Tnum result = new Tnum();
    
                foreach(KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
                {    
                    if (!slice.Value.IsKnown)
                    {
                        result.AddState(slice.Key,slice.Value);
                    }
                    else
                    {
                        DateTime val = Convert.ToDateTime(slice.Value.Val);
                        result.AddState(slice.Key, val.Day);
                    }
                }
                
                return result.Lean;  
            }
        }

        // ********************************************************************
        // TimeDiff
        // ********************************************************************
        
        /// <summary>
        /// Returns the number of days between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum DayDiff(Tdate td1, Tdate td2)
        {
            return ApplyDtFcnToTimeline((x,y) => TimeDiff(x,y), Time.IntervalType.Day, td1, td2);
        }

        /// <summary>
        /// Returns the number of weeks between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum WeekDiff(Tdate td1, Tdate td2)
        {
            return ApplyDtFcnToTimeline((x,y) => TimeDiff(x,y), Time.IntervalType.Week, td1, td2);
        }
        
        // TODO: Add MonthDiff and QuarterDiff functions
        
        /// <summary>
        /// Returns the number of years between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum YearDiff(Tdate td1, Tdate td2)
        {
            return ApplyDtFcnToTimeline((x,y) => TimeDiff(x,y), Time.IntervalType.Year, td1, td2);
        }
        
        private static decimal TimeDiff(Time.IntervalType t, List<Hval> list)
        {
            DateTime earlierDate = Convert.ToDateTime(list[0].Val);
            DateTime laterDate = Convert.ToDateTime(list[1].Val);
            TimeSpan s = laterDate - earlierDate;
            
            if (t == Time.IntervalType.Day)
            {
                return Convert.ToDecimal(s.TotalDays);
            }
            if (t == Time.IntervalType.Week)
            {
                int weeks = Convert.ToInt32(s.TotalDays) / 7;
                return Convert.ToDecimal(weeks);
            }
            if (t == Time.IntervalType.Year)
            {
                int years = (int) (s.TotalDays / 365.2425);
                return Convert.ToDecimal(years);
            }
            
            return -1;
        }
//        private static decimal TimeDiff(Time.IntervalType t, List<object> list)
//        {
//            DateTime earlierDate = Convert.ToDateTime(list[0]);
//            DateTime laterDate = Convert.ToDateTime(list[1]);
//            TimeSpan s = laterDate - earlierDate;
//            
//            if (t == Time.IntervalType.Day)
//            {
//                return Convert.ToDecimal(s.TotalDays);
//            }
//            if (t == Time.IntervalType.Week)
//            {
//                int weeks = Convert.ToInt32(s.TotalDays) / 7;
//                return Convert.ToDecimal(weeks);
//            }
//            if (t == Time.IntervalType.Year)
//            {
//                int years = (int) (s.TotalDays / 365.2425);
//                return Convert.ToDecimal(years);
//            }
//            
//            return -1;
//        }
        
        // ********************************************************************
        // Apply a function to the values in a Tdate
        // ********************************************************************

        private static Tnum ApplyDtFcnToTimeline(Func<Time.IntervalType,List<Hval>,Hval> fcn, Time.IntervalType t, params Tdate[] list)
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(list))
            {    
                // Any higher-precedence states go next
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    result.AddState(slice.Key, fcn(t, slice.Value));
                }
            }
            
            return result.Lean;
        }
    }
    
    #pragma warning restore 660, 661
}