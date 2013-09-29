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

    public abstract partial class Time : H
    {
        /// <summary>
        /// Enumeration of common time intervals 
        /// </summary>
        public enum IntervalType
        {
            Day,                // DateTime.AddDays(1)
            Week,               // DateTime.AddDays(7)
            Month,              // DateTime.AddMonths(1)
            Quarter,            // DateTime.AddYears(3)
            Year                // DateTime.AddYears(1)
        }

        // Constants

        public const double DaysPerYear = 365.242;

        public const double DaysPerQuarer = DaysPerYear / 4;

        public const double DaysPerMonth = DaysPerYear / 12;

        
        //*********************************************************************
        // TEMPORAL "STEP" FUNCTIONS
        //*********************************************************************
        
        /// <summary>
        /// Returns the number of intervals since a given date (step up function)
        /// (e.g. 1 2 3 4 5 6 7 8 9 ...)
        /// </summary>
        public static Tnum IntervalsSince(Tdate start, Tdate end, IntervalType interval)
        {
            return IntervalsSince(start, end, interval, 0);
        }
        
        public static Tnum IntervalsSince(Tdate startDate, Tdate endDate, IntervalType interval, int? startAt)
        {
            // Handle unknowns
            Hstate top = PrecedingState(startDate.FirstValue, endDate.FirstValue);
            if (top != Hstate.Known) return new Tnum(new Hval(null,top));

            DateTime start = startDate.ToDateTime;
            DateTime end = endDate.ToDateTime;

            Tnum result = new Tnum();
            
            if (start != Time.DawnOf)
            {
                result.AddState(Time.DawnOf,0);
            }
            
            DateTime indexDate = start;
            int? indexNumber = startAt;
    
            while (indexDate < end) 
            {
                result.AddState(indexDate,Convert.ToDecimal(indexNumber));
                indexNumber++;
                indexDate = indexDate.AddInterval(interval, 1);
            }

            if (end < Time.EndOf) result.AddState(end, 0);
            return result;
        }

        /// <summary>
        /// Returns the number of intervals until a date (step down function)
        /// (e.g. ... 7 6 5 4 3 2 1)
        /// </summary>
        public static Tnum IntervalsUntil(Tdate start, Tdate end, IntervalType interval)
        {
            return IntervalsUntil(start, end, interval, 0);
        }
        
        public static Tnum IntervalsUntil(Tdate startDate, Tdate endDate, IntervalType interval, int startAt)
        {
            // Handle unknowns
            Hstate top = PrecedingState(startDate.FirstValue, endDate.FirstValue);
            if (top != Hstate.Known) return new Tnum(new Hval(null,top));

            DateTime start = startDate.ToDateTime;
            DateTime end = endDate.ToDateTime;
            
            Tnum result = new Tnum();
            
            DateTime indexDate = end;
            int indexNumber = startAt-1;
    
            while (indexDate > start) 
            {
                if (indexNumber != startAt-1)
                {
                    result.AddState(indexDate,Convert.ToString(indexNumber));
                }
                indexNumber++;
                indexDate = indexDate.SubtractInterval(interval);
            }
            
            result.AddState(Time.DawnOf, 0);
            result.AddState(start,Convert.ToString(indexNumber));
            result.AddState(end, 0);
            
            return result;
        }
        
        //*********************************************************************
        // TEMPORAL "RECURRENCE" FUNCTIONS
        //*********************************************************************
        
        /// <summary>
        /// Loops (cycles) through numbers over time (e.g. 1 2 3 4 1 2 3 4 ...)
        /// </summary>
        public static Tnum Recurrence(Tdate startDate, Tdate endDate, IntervalType interval, int min, int max)
        {    
            // Handle unknowns
            Hstate top = PrecedingState(startDate.FirstValue, endDate.FirstValue);
            if (top != Hstate.Known) return new Tnum(new Hval(null,top));

            DateTime start = startDate.ToDateTime;
            DateTime end = endDate.ToDateTime;
            
            Tnum result = new Tnum();
            
            if (start != Time.DawnOf)
            {
                result.AddState(Time.DawnOf, 0);
            }
            
            DateTime indexDate = start;
            int indexNumber = min;
    
            while (indexDate < end) 
            {
                result.AddState(indexDate,Convert.ToString(indexNumber));
                indexDate = indexDate.AddInterval(interval, 1);
                
                // Reset sequence
                indexNumber++;
                if (indexNumber > max)
                {
                    indexNumber = min;
                }
            }
            
            result.AddState(end, 0);
            return result;
        }      

        /// <summary>
        /// Maps a function to a timeline, starting on a given date.
        /// </summary>
        //  TODO: Get rid of Time.IntervalType
        public static Tnum TemporalMap(Func<Tnum,Tnum> fcn, Tdate startDate, Tnum intervalCount, Time.IntervalType intervalType)
        {
            return fcn(Time.IntervalsSince(startDate, startDate.AddYears(intervalCount), intervalType));
        }
    }
}