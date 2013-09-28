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

namespace Akkadian
{
    //*************************************************************************
    // DateTime extension methods
    //*************************************************************************
    
    public static class DateTimeExtensionMethods
    {

//        public static implicit operator DateTime(Tdate d) 
//        {
//            DateTime r = d.ToDateTime ?? DateTime.Now;
//            return r;
//        }
        
        /// <summary>
        /// Determines whether a DateTime is a weekday.
        /// </summary>
        public static bool IsWeekday(this DateTime dt)
        {
            return dt.DayOfWeek != DayOfWeek.Saturday && 
                   dt.DayOfWeek != DayOfWeek.Sunday;
        }
        
        /// <summary>
        /// Returns the current day if it's not a weekend or U.S. federal holiday;
        /// Otherwise returns the next day that is not a weekend or holiday.
        /// </summary>
        public static DateTime CurrentOrNextBusinessDay(this DateTime dt)
        {
            if (dt.IsWeekday())
            {
                return dt;
            }
            
            return CurrentOrNextBusinessDay(dt.AddDays(1));
        }
        
        /// <summary>
        /// Returns the subsequent Monday if a date is on a weekend;
        /// otherwise, returns the date itself.
        /// </summary>
        public static DateTime CurrentOrNextWeekday(this DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                return dt.AddDays(2);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return dt.AddDays(1);
            }
            
            return dt;
        }
        
        /// <summary>
        /// Returns the previous Friday if a date is on a Saturday,
        /// the subsequent Monday if it's on a Sunday, 
        /// otherwise, the date itself.
        /// </summary>
        public static DateTime CurrentOrAdjacentWeekday(this DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                return dt.AddDays(-1);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return dt.AddDays(1);
            }
            
            return dt;
        }
        
        /// <summary>
        /// Add an interval of time (or some multiple thereof) to a DateTime.
        /// </summary>
        public static DateTime AddInterval(this DateTime dt, Time.IntervalType interval, int numberOfIntervals)
        {
            if (interval == Time.IntervalType.Day)
                return dt.AddDays(numberOfIntervals);
            else if (interval == Time.IntervalType.Week)
                return dt.AddDays(7 * numberOfIntervals);
            else if (interval == Time.IntervalType.Month)
                return dt.AddMonths(numberOfIntervals);
            else if (interval == Time.IntervalType.Quarter) // won't necessarily work for qtr end dates
                return dt.AddMonths(3 * numberOfIntervals);
            else if (interval == Time.IntervalType.Year)
                return dt.AddYears(numberOfIntervals);
            else
                return dt;
        }
        
        /// <summary>
        /// Subtract an interval of time from a DateTime.
        /// </summary>
        public static DateTime SubtractInterval(this DateTime dt, Time.IntervalType interval)
        {
            if (interval == Time.IntervalType.Day)
                return dt.AddDays(-1);
            else if (interval == Time.IntervalType.Week)
                return dt.AddDays(-7);
            else if (interval == Time.IntervalType.Month)  // not reversible version of AddInterval
                return dt.AddMonths(-1);
            else if (interval == Time.IntervalType.Quarter) // won't necessarily work for qtr end dates
                return dt.AddMonths(-3);
            else if (interval == Time.IntervalType.Year)
                return dt.AddYears(-1);
            else
                return dt;
        }
        
        /// <summary>
        /// Returns the date after n full calendar months have elapsed, starting
        /// from a given date (this).
        /// </summary>
        /// <remarks>
        /// This only works when adding, not subtracting, calendar months.
        /// </remarks>
        public static DateTime AddCalendarMonths(this DateTime dt, int n)
        {
            // Advance n months...
            int y = dt.AddMonths(n).Year;
            int m = dt.AddMonths(n).Month;
            
            // ...then get (the day after) the last day in that month
            if (dt.Day != 1)
                return new DateTime(y, m, DateTime.DaysInMonth(y,m)).AddDays(1);
            // unless it's the first of the month (special case)
            else
                return dt.AddMonths(n);
        }

        #region Friendly reminders
        
        // C# methods that apply to a DateTime instance:
        
            // DateTime.Now
            // DateTime.AddYears, AddMonths, AddDays, AddHours, etc.
            // DateTime.DayOfWeek - returns Monday, Tuesday, etc. from DayOfWeek enum
            // DateTime.ToString
        
        // C# methods that exist in the DateTime class:
        
            // int DateTime.DaysInMonth(year, month)
            // bool IsLeapYear(year)
                
        // See: http://msdn.microsoft.com/en-us/library/497a406b(v=VS.100).aspx
        
        #endregion
    } 
    
    
    //*************************************************************************
    // Hammurabi Time class
    //*************************************************************************
    
    public abstract partial class Time : H
    {
        /// <summary>
        /// Returns the current date, reset to midnight.
        /// </summary>
        public DateTime Today
        {
            get
            {
                return DateTime.Now.Date;
            }
        }
        
        /// <summary>
        /// Returns the last given day of the week of a given month.
        /// Example: the last Monday in January 2011 = 
        /// LastDayOfTheNthMonth(DayOfWeek.Monday, 1, 2011)
        /// </summary>    
        public static DateTime LastDayOfWeekMonthYear(DayOfWeek dow, int month, int year)
        {
            try
            {
                return Time.NthDayOfWeekMonthYear(5, dow, month, year);
            }
            catch
            {
                return Time.NthDayOfWeekMonthYear(4, dow, month, year);
            }
        }
        
        /// <summary>
        /// Returns the Nth day of the week of a given month.
        /// Example: the 3rd Monday in January 2011 = 
        /// NthDayOfTheNthMonth(3, DayOfWeek.Monday, 1, 2011)
        /// </summary>
        public static DateTime NthDayOfWeekMonthYear(int num, DayOfWeek dow, int month, int year)
        {
            DateTime FirstDayOfMonth = new DateTime(year, month, 1);
            DayOfWeek DoWFirstDayOfMonth = FirstDayOfMonth.DayOfWeek;
            
            // First day that is the target DayOfWeek
            int FirstTargetDayOfWeek = dow - DoWFirstDayOfMonth + 1;
            if (FirstTargetDayOfWeek <= 0)
            {
                FirstTargetDayOfWeek += 7;
            }
            
            // Target day
            int day = FirstTargetDayOfWeek + ((num-1) * 7);
            
            DateTime result = new DateTime(year, month, day);
            return result; 
        }
        
        /// <summary>
        /// The next given day of the week on or after a particular date
        /// Example: the next Monday after 2010-14-12 = 2010-12-20
        /// </summary>
        public static DateTime DoWOnOrAfter(DayOfWeek dow, DateTime dt)
        {
            DayOfWeek inputDoW = dt.DayOfWeek;
            int diff = dow - inputDoW;
            if (diff < 0) { diff += 7; }
            return dt.AddDays(diff);
        }
        
        /// <summary>
        /// The next given day of the week on or before a particular date
        /// Example: the next Monday after 2010-14-12 = 2010-12-13
        /// </summary>
        public static DateTime DoWOnOrBefore(DayOfWeek dow, DateTime dt)
        {
            DayOfWeek inputDoW = dt.DayOfWeek;
            int diff = dow - inputDoW;
            if (diff > 0) { diff -= 7; }
            return dt.AddDays(diff);
        }
        
        /// <summary>
        /// Returns the number of weekdays (M-F) between two dates.  The result
        /// includes the start and end dates.
        /// </summary>
        public static int WeekdayCount(DateTime start, DateTime end)
        {
            TimeSpan timeDiff = end - start;
            int diff = Convert.ToInt32(timeDiff.TotalDays);
            int remainder = Convert.ToInt32(diff % 7);    
            int weeks = diff / 7;
            
            // Remove Saturdays and Sundays from count
            for(int i=1; i<remainder+1; i++)
            {
                DateTime theDay = start.AddDays(i);
                
                if (theDay.DayOfWeek == DayOfWeek.Saturday || 
                    theDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    remainder--;
                }
            }
            
            return (weeks * 5) + remainder;
        }
            
        /// <summary>
        /// Returns the earliest of the input DateTimes.
        /// </summary>
        public static DateTime Earliest (params DateTime[] dates)
        {
            DateTime first = dates[0];
            foreach (DateTime dt in dates) 
            {
                if (dt < first)
                    first = dt;
            }
            return first;
        }
        
        /// <summary>
        /// Returns the latest of the input DateTimes.
        /// </summary>
        public static DateTime Latest (params DateTime[] dates)
        {
            DateTime last = dates[0];
            foreach (DateTime dt in dates) 
            {
                if (dt > last)
                    last = dt; 
            }
            return last;
        }
    }
}