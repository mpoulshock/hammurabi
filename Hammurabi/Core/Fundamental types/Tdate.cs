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
        public DateTime? ToDateTime
        {
            get
            {
                if (this.IsUnknown || TimeLine.Count > 1) { return null; }
                
                return (Convert.ToDateTime(TimeLine.Values[0]));
            }
        }
        
        /// <summary>
        /// Returns the value of the Tdate at a specified point in time.
        /// </summary>
        public Tdate AsOf(DateTime dt)
        {
            if (this.IsUnknown) { return new Tdate(); }
            
            return (Tdate)this.AsOf<Tdate>(dt);
        }
        
        
        // ********************************************************************
        // IsAlways / IsEver
        // ********************************************************************
        
        /// <summary>
        /// Returns an eternally true Tbool if the Tdate always has a specified
        /// value.
        /// </summary>
        public Tbool IsAlways(DateTime val)
        {
            return IsAlwaysTvar<Tdate>(val, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns an eternally true Tbool if the Tdate always has a specified
        /// value between two given dates.
        /// </summary>
        public Tbool IsAlways(DateTime val, DateTime start, DateTime end)
        {
            return IsAlwaysTvar<Tdate>(val, start, end);
        }
        
        /// <summary>
        /// Returns an eternally true Tbool if the Tdate ever has a specified
        /// value.
        /// </summary>
        public Tbool IsEver(DateTime val)
        {
            return IsEverTvar(val);
        }
        
        /// <summary>
        /// Returns an eternally true Tbool if the Tdate ever has a specified
        /// value between two given dates.
        /// </summary>
        public Tbool IsEver(DateTime val, DateTime start, DateTime end)
        {
            return IsEverTvar<Tdate>(val, start, end);
        }
        
        
        // ********************************************************************
        // AddTimeInterval
        // ********************************************************************
    
        /// <summary>
        /// Adds a specified number of days to each DateTime in a Tdate.
        /// </summary>        
        public Tdate AddDays(int days)
        {        
            if (this.IsUnknown) { return new Tdate(); }
            
            Tdate result = new Tdate();

            foreach(KeyValuePair<DateTime,object> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value);
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
            if (this.IsUnknown) { return new Tdate(); }
            
            Tdate result = new Tdate();

            foreach(KeyValuePair<DateTime,object> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value);
                DateTime newDt = val.AddMonths(months);
                result.AddState(slice.Key, newDt);
            }
            
            return result.Lean;    
        }
        
        /// <summary>
        /// Adds a specified number of years to each DateTime in a Tdate.
        /// </summary>
        public Tdate AddYears(int years)
        {        
            if (this.IsUnknown) { return new Tdate(); }
            
            Tdate result = new Tdate();

            foreach(KeyValuePair<DateTime,object> slice in this.IntervalValues)
            {    
                DateTime val = Convert.ToDateTime(slice.Value);
                DateTime newDt = val.AddYears(years);
                result.AddState(slice.Key, newDt);
            }
            
            return result.Lean;    
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
        
        private static decimal TimeDiff(Time.IntervalType t, List<object> list)
        {
            DateTime earlierDate = Convert.ToDateTime(list[0]);
            DateTime laterDate = Convert.ToDateTime(list[1]);
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

        
        // ********************************************************************
        // Apply a function to the values in a Tdate
        // ********************************************************************
        
        private static Tnum ApplyDtFcnToTimeline(Func<Time.IntervalType,List<object>,object> fcn, Time.IntervalType t, params Tdate[] list)
        {
            if (AnyAreUnknown(list)) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(list))
            {    
                result.AddState(slice.Key, fcn(t, slice.Value));
            }
            
            return result.Lean;
        }
        
    }
    
    #pragma warning restore 660, 661
}