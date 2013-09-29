// Copyright (c) 2010-2013 Hammura.bi LLC
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
    public partial class H
    {
        /// <summary>
        /// Returns a Tnum representing the calendar year, spanning all of time.
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
                return Time.Quarter(1, 1, 20);
            }
        }
        
        /// <summary>
        /// Returns a Tnum representing the calendar month (by default, a
        /// 20-year span centered on Jan. 1st of the current year)
        /// </summary>
        public static Tnum TheMonth
        {
            get
            {
                return Time.Month(10);
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
                return Time.CalendarWeek(5);
            }
        }

        /// <summary>
        /// Returns a Tnum representing the day, spanning a 200-year period.
        /// </summary>
        /// <remarks>
        /// Warning: This has over 73,000 intervals, so use judiciously.
        /// </remarks>
        public static Tnum TheDay
        {
            get
            {
                return Time.IntervalsSince(Date(1900,1,1), Date(2100,1,1), Time.IntervalType.Day, 1);
            }
        }


        /// <summary>
        /// Number of days in the calendar quarter (fiscal year assumed to start on Jan. 1) (temporal).
        /// </summary>
        public static Tnum DaysInQuarter()
        {
            return Switch<Tnum>(
                    ()=> TheQuarter == 1 && IsLeapYear(), ()=> 91,
                    ()=> TheQuarter == 1, ()=> 90,
                    ()=> TheQuarter == 2, ()=> 91,
                    ()=> TheQuarter == 3, ()=> 92,
                    ()=> TheQuarter == 4, ()=> 92,
                    ()=> new Tnum(Hstate.Stub));
        }

        /// <summary>
        /// Number of days in the calendar month (temporal).
        /// </summary>
        public static Tnum DaysInMonth()
        {
            return Switch<Tnum>(
                    ()=> TheMonth ==  1, ()=> 31,
                    ()=> TheMonth ==  2 && IsLeapYear(), ()=> 29,
                    ()=> TheMonth ==  2, ()=> 28,
                    ()=> TheMonth ==  3, ()=> 31,
                    ()=> TheMonth ==  4, ()=> 30,
                    ()=> TheMonth ==  5, ()=> 31,
                    ()=> TheMonth ==  6, ()=> 30,
                    ()=> TheMonth ==  7, ()=> 31,
                    ()=> TheMonth ==  8, ()=> 31,
                    ()=> TheMonth ==  9, ()=> 30,
                    ()=> TheMonth == 10, ()=> 31,
                    ()=> TheMonth == 11, ()=> 30,
                    ()=> TheMonth == 12, ()=> 31,
                    ()=> new Tnum(Hstate.Stub));
        }

        /// <summary>
        /// Number of days in the calendar year (temporal).
        /// </summary>
        public static Tnum DaysInYear()
        {
            return Switch<Tnum>(
                    ()=> IsLeapYear(), ()=> 366,
                    ()=> 365);
        }

        /// <summary>
        /// True when the year is a leap year (temporal)
        /// </summary>
        public static Tbool IsLeapYear()
        {
            return Switch<Tbool>(
                    ()=> TheYear == 2100 , ()=> false,
                    ()=> TheYear % 4 == 0, ()=> true,
                    ()=> false);
        }
    }
        

    /// <summary>
    /// A construct representing "the time" - as in that thing we refer
    /// to when we say, "The time is 5 pm." 
    /// </summary>
    public partial class Time
    {
        /// <summary>
        /// Returns the date the universe was created.
        /// (Well, you get the point.)
        /// </summary>
        public static DateTime DawnOf
        {
            get
            {
                return new DateTime(1800,1,1);
            }
        }

        /// <summary>
        /// Returns the date the universe will end.
        /// </summary>
        public static DateTime EndOf
        {
            get
            {
                return new DateTime(2200,12,31);
            }
        }

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
