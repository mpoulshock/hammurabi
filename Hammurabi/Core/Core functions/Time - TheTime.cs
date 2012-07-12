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
    public partial class H
    {
        /// <summary>
        /// Returns a Tnum representing the calendar year.  Spans all of
        /// "time": from 1800-01-01 to 2200-12-31.
        /// </summary>
        public static Tnum TheYear
        {
            get
            {
                return Time.IntervalsSince(Time.DawnOf, Time.EndOf, Time.IntervalType.Year, Time.DawnOf.Year);
            }
        }
        
        /// <summary>
        /// Returns a Tnum representing the fiscal quarter (by default, a 20-year
        /// span centered on day 1 of the fiscal year that begins in current year)
        /// </summary>
        public static Tnum TheQuarter
        {
            // TODO: Consider making a field for Q1StartMonth and Q1StartDay (so TheQuarter
            // can be used when Q1 starts sometime other than Jan. 1)
            get
            {
                return TheTime.Quarter(1, 1, 20);
            }
        }
        
        /// <summary>
        /// Returns a Tnum representing the calendar month (by default, a
        /// 10-year span centered on Jan. 1st of the current year)
        /// </summary>
        public static Tnum TheMonth
        {
            get
            {
                return TheTime.Month(5);
            }
        }
        
        /// <summary>
        /// Returns a Tnum representing the calendar week (by default, a
        /// 10-year span centered on the start of the current year).
        /// </summary>
        /// <remarks>
        /// The default TheCalendarWeek function follows the U.S. convention
        /// of starting on the Saturday on or before Jan. 1 and ending on the
        /// Sunday on or after Dec. 31.  
        /// See en.wikipedia.org/wiki/Seven-day_week#Week_numbering.
        /// Note that weeks are not numbered (because sometimes week 1 and 53 of
        /// adjacent years overlap).  Each interval has the value 0.
        /// </remarks>
        public static Tnum TheCalendarWeek
        {
            get
            {
                return TheTime.CalendarWeek(5);
            }
        }
    }
    
    /// <summary>
    /// A construct representing "the time" - as in that abstract thing we refer
    /// to when we say something like, "The time is 5 pm." 
    /// </summary>
    public partial class TheTime : Time
    {
        /// <summary>
        /// Returns a Tbool that's true at and after a specified DateTime, 
        /// and otherwise false.
        /// </summary>
        public static Tbool IsAtOrAfter(Tdate dt)
        {
            // Handle unknowns
            if (!dt.FirstValue.IsKnown)
            {
                return new Tbool(dt.FirstValue);
            }

            // Create boolean
            Tbool result = new Tbool();
            if (dt == Time.DawnOf)
            {
                result.AddState(DawnOf, true);
            }
            else
            {
                result.AddState(DawnOf, false);
                result.AddState(dt.ToDateTime, true);
            }
            return result;
        }
        
        /// <summary>
        /// Returns a Tbool that's true up to a specified DateTime, and false
        /// at and after it.
        /// </summary>
        public static Tbool IsBefore(Tdate dt)
        {
            // Handle unknowns
            if (!dt.FirstValue.IsKnown)
            {
                return new Tbool(dt.FirstValue);
            }

            // Create boolean
            Tbool result = new Tbool();

            if (dt == DawnOf)
            {
                result.AddState(DawnOf, false);
            }
            else
            {
                result.AddState(DawnOf, true);
                result.AddState(dt.ToDateTime, false);
            }
            return result;
        }
        
        /// <summary>
        /// Returns a Tbool that's true during a specified time interval (including
        /// at the "start"), and otherwise false (including at the moment represented 
        /// by the "end").
        /// </summary>
        public static Tbool IsBetween(Tdate start, Tdate end)
        {
             return IsAtOrAfter(start) && IsBefore(end);
        }
         
        /// <summary>
        /// Returns a Tnum representing the calendar year (with some
        /// span centered on the current year)
        /// </summary>
        //  TODO: Delete?
        public static Tnum Year(int halfSpanInYears)
        {
            int currentYear = DateTime.Now.Year;
            DateTime firstOfCurrentYear = new DateTime(currentYear,1,1);
            
            return IntervalsSince(firstOfCurrentYear.AddYears(halfSpanInYears * -1), 
                                  firstOfCurrentYear.AddYears(halfSpanInYears), 
                                  IntervalType.Year, 
                                  currentYear-halfSpanInYears);
        }
        
        /// <summary>
        /// Returns a Tnum representing the fiscal quarter (by default, a 20-year
        /// span centered on day 1 of the fiscal year that begins in current year)
        /// </summary>
        public static Tnum Quarter(int Q1StartMonth, int Q1StartDay)
        {
            return Quarter(Q1StartMonth, Q1StartDay, 20);
        }
        
        public static Tnum Quarter(int Q1StartMonth, int Q1StartDay, int halfSpanInYears)
        {
            DateTime Q1Start = new DateTime(DateTime.Now.Year, Q1StartMonth, Q1StartDay);
            
            return Time.Recurrence(Q1Start.AddYears(halfSpanInYears * -1),
                                   Q1Start.AddYears(halfSpanInYears),
                                   Time.IntervalType.Quarter,1,4);
        }
        
        /// <summary>
        /// Returns a Tnum representing the calendar month (by default, a
        /// 10-year span centered on Jan. 1st of the current year)
        /// </summary>
        public static Tnum Month(int halfSpanInYears)
        {
            int currentYear = DateTime.Now.Year;
            DateTime firstOfCurrentYear = new DateTime(currentYear,1,1);
            
            return Time.Recurrence(firstOfCurrentYear.AddYears(halfSpanInYears * -1),
                                   firstOfCurrentYear.AddYears(halfSpanInYears),
                                   Time.IntervalType.Month,1,12);
        }
        
        /// <summary>
        /// Returns a Tnum representing the calendar week (by default, a
        /// 10-year span centered on the start of the current year).
        /// </summary>
        /// <remarks>
        /// See remarks in the CalendarWeek method in the H class.
        /// </remarks>
        public static Tnum CalendarWeek(int halfSpanInYears)
        {
            Tnum result = new Tnum();
            result.AddState(Time.DawnOf, 0);
            
            // Get the start date for week 1, n years in the past
            DateTime d = NthDayOfWeekMonthYear(1, DayOfWeek.Saturday, 1, DateTime.Now.Year-halfSpanInYears);
            if (d.Day != 1) { d = d.AddDays(-7); }
            
            // Mark off each week
            for (int i=0; i < (halfSpanInYears*106); i++)
            {
                result.AddState(d, 0);
                d = d.AddDays(7);
            }

            // Don't apply .Lean because it would defeat the purpose of this object.
            return result;
        }
    }
}
