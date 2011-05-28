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

namespace KSA.Tit35
{
    /// <summary>
    /// Determines whether a date is a Kansas legal holiday.
    /// </summary>
    /// <cite>K.S.A. 35-107 (2009)</cite>    
    /// <updated>2011-03-05</updated>        
    public static class Sec107
    {
        /// <summary>
        /// Determines whether a date is a Kansas legal holiday.
        /// </summary>
        public static bool IsLegalHoliday(DateTime d)
        {
            // Kansas law does not refer to 5 U.S.C. 6103, 
            // but the logic is the same.
            return USC.Tit5.Sec6103.IsNewYearsDayObserved(d) || 
                   d == USC.Tit5.Sec6103.MLKDay(d.Year) ||
                   d == PresidentsDay(d.Year) ||
                   d == USC.Tit5.Sec6103.MemorialDay(d.Year) ||
                   d == USC.Tit5.Sec6103.IndependenceDayObserved(d.Year) ||
                   d == USC.Tit5.Sec6103.LaborDay(d.Year) ||
                   d == USC.Tit5.Sec6103.ColumbusDay(d.Year) ||
                   d == USC.Tit5.Sec6103.VeteransDayObserved(d.Year)    ||
                   d == USC.Tit5.Sec6103.ThanksgivingDay(d.Year) ||
                   d == USC.Tit5.Sec6103.ChristmasDayObserved(d.Year);
        }
        
        /// <summary>
        /// President's Day - third Monday in February.
        /// </summary>
        public static DateTime PresidentsDay(int year)
        {
            // Kansas law calls it "President's Day"; the U.S. Code 
            // calls it "Washington's Birthday."
            return USC.Tit5.Sec6103.WashingtonsBirthday(year);
        }

    }
}