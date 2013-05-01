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
    public partial class H
    {        
        /// <summary>
        /// Represents a nested if-then statement within in a boolean expression.
        /// </summary>
        public static Tbool IfThen(Tbool tb1, Tbool tb2)
        {       
            return !tb1 || tb2;
        }
        
        /// <summary>
        /// Counts the number of boolean inputs that have the same value
        /// value as the first (test) argument
        /// </summary>
        // TODO: Generalize BoolCount to ValCount(val, Tvar[] list)?
        public static Tnum BoolCount(bool test, params Tbool[] list)
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(list))
            {    
                result.AddState(slice.Key, BoolCountK(test, slice.Value));
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Private non-temporal BOOL COUNT function.
        /// </summary>
        private static Hval BoolCountK(bool test, List<Hval> list)
        {
            Hstate top = PrecedingState(list);
            if (top != Hstate.Known) 
            {
                return new Hval(null,top);
            }

            int count = 0;
            foreach (Hval v in list)
            {
                if (Convert.ToBoolean(v.Val) == test)
                {
                    count++;
                }
            }
            
            return new Hval(count);
        }

        /// <summary>
        /// Returns the minimum value of the given inputs.  Accepts Tnums, ints,
        /// doubles, and decimals.
        /// </summary>
        public static Tnum Min(params Tnum[] list)
        {
            return ApplyFcnToTimeline(x => Util.Minimum(x), list);
        }
        
        /// <summary>
        /// Returns the maximum value of the given inputs.  Accepts Tnums, ints,
        /// doubles, and decimals.
        /// </summary>
        public static Tnum Max(params Tnum[] list)
        {
            return ApplyFcnToTimeline(x => Util.Maximum(x), list);
        }
        
        /// <summary>
        /// A shorter way of instantiating a DateTime.
        /// </summary>
        public static DateTime Date(int year, int mo, int day)
        {
            return new DateTime(year, mo, day);
        }
        
        /// <summary>
        /// Converts a yyyy-mm-dd string into a DateTime.
        /// </summary>
        public static DateTime Date(string date)
        {
            return DateTime.Parse(date);
        }

        /// <summary>
        /// Returns the number of days between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum DayDiff(Tdate td1, Tdate td2)
        {
            return Tdate.DayDifference(td1,td2);
        }
        
        /// <summary>
        /// Returns the number of weeks (accurate to three decimal places) between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum WeekDiff(Tdate td1, Tdate td2)
        {
            return Tdate.WeekDifference(td1,td2);
        }

        /// <summary>
        /// Returns the number of years (accurate to three decimal places) between the DateTimes in two Tdates.
        /// </summary>
        public static Tnum YearDiff(Tdate td1, Tdate td2)
        {
            return Tdate.YearDifference(td1,td2);
        }

        /// <summary>
        /// Creates a string of N blank spaces.
        /// </summary>
        public static string NSpaces(int count)
        {
            string result = "";
            for (int i=0; i<count; i++)
                result += " ";
            return result;
        }
    }
}