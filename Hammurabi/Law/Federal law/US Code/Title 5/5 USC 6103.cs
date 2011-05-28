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
//
// NO REPRESENTATION OR WARRANTY IS MADE THAT THIS PROGRAM ACCURATELY REFLECTS
// OR EMBODIES ANY APPLICABLE LAWS, REGULATIONS, RULES OR EXECUTIVE ORDERS 
// ("LAWS"). YOU SHOULD RELY ONLY ON OFFICIAL VERSIONS OF LAWS PUBLISHED BY THE 
// RELEVANT GOVERNMENT AUTHORITY, AND YOU ASSUME THE RESPONSIBILITY OF 
// INDEPENDENTLY VERIFYING SUCH LAWS. THE USE OF THIS PROGRAM IS NOT A 
// SUBSTITUTE FOR THE ADVICE OF AN ATTORNEY.

using System;
using Hammurabi;

namespace USC.Tit5
{
    /// <summary>
    /// Determines whether a date is a U.S. federal holiday.
    /// </summary>
    /// <cite>5 U.S.C. 6103 (2010)</cite>    
    /// <updated>2010-11-11</updated>        
    /// <remarks>
    /// Does not implement portions of sec. 6103 relating to the schedules of 
    /// federal employees.
    /// Does not implement Inauguration Day.
    /// </remarks>    
    public class Sec6103
    {
         
        /// <summary>
        /// Determines whether a date is a U.S. federal holiday.
        /// </summary>
        public static bool IsLegalHoliday(DateTime d)
        {
            return IsNewYearsDayObserved(d) || 
                   d == MLKDay(d.Year) ||
                   d == WashingtonsBirthday(d.Year) ||
                   d == MemorialDay(d.Year) ||
                   d == IndependenceDayObserved(d.Year) ||
                   d == LaborDay(d.Year) ||
                   d == ColumbusDay(d.Year) ||
                   d == VeteransDayObserved(d.Year)    ||
                   d == ThanksgivingDay(d.Year) ||
                   d == ChristmasDayObserved(d.Year);
        }
        
        /// <summary>
        /// New Year's - January 1st.
        /// </summary>
        public static DateTime NewYearsDay(int year)
        {
            return new DateTime(year,1,1);
        }
        
        public static DateTime NewYearsDayObserved(int year)
        {
            return NewYearsDay(year).CurrentOrAdjacentWeekday();
        }    
        
        public static bool IsNewYearsDayObserved(DateTime d)
        {    
            // New years day can be observed in the previous year,
            // for example, if it falls on a Saturday
            return d == NewYearsDayObserved(d.Year) ||
                   d == NewYearsDayObserved(d.Year+1);
        }    
        
        /// <summary>
        /// MLK Day - third Monday in January.
        /// </summary>
        public static DateTime MLKDay(int year)
        {
            return Time.NthDayOfWeekMonthYear(3,DayOfWeek.Monday,1,year);
        }
        
        /// <summary>
        /// Washington's Birthday - third Monday in February.
        /// </summary>
        public static DateTime WashingtonsBirthday(int year)
        {
            return Time.NthDayOfWeekMonthYear(3,DayOfWeek.Monday,2,year);
        }

        /// <summary>
        /// Memorial Day - last Monday in May.
        /// </summary>
        public static DateTime MemorialDay(int year)
        {
            return Time.LastDayOfWeekMonthYear(DayOfWeek.Monday, 5, year);
        }

        /// <summary>
        /// July 4th.
        /// </summary>
        public static DateTime IndependenceDay(int year)
        {
            return new DateTime(year,7,4);
        }
        
        public static DateTime IndependenceDayObserved(int year)
        {
            return IndependenceDay(year).CurrentOrAdjacentWeekday();
        }

        /// <summary>
        /// Labor Day - first Monday in September.
        /// </summary>
        public static DateTime LaborDay(int year)
        {
            return Time.NthDayOfWeekMonthYear(1,DayOfWeek.Monday,9,year);
        }
        
        /// <summary>
        /// Columbus Day - second Monday in October.
        /// </summary>
        public static DateTime ColumbusDay(int year)
        {
            return Time.NthDayOfWeekMonthYear(2,DayOfWeek.Monday,10,year);
        }
        
        /// <summary>
        /// Veteran's Day - November 11th.
        /// </summary>
        public static DateTime VeteransDay(int year)
        {
            return new DateTime(year,11,11);
        }
        
        public static DateTime VeteransDayObserved(int year)
        {
            return VeteransDay(year).CurrentOrAdjacentWeekday();
        }
        
        /// <summary>
        /// Thanksgiving - fourth Thursday in November.
        /// </summary>
        public static DateTime ThanksgivingDay(int year)
        {
            return Time.NthDayOfWeekMonthYear(4,DayOfWeek.Thursday,11,year);
        }
        
        /// <summary>
        /// Christmas - December 25th.
        /// </summary>
        public static DateTime ChristmasDay(int year)
        {
            return new DateTime(year,12,25);
        }
        
        public static DateTime ChristmasDayObserved(int year)
        {
            return ChristmasDay(year).CurrentOrAdjacentWeekday();
        }
    
    }
    
}
