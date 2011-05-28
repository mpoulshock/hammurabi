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

using Hammurabi;
using System;

namespace USC.Tit26
{

    /// <summary>
    /// Returns a Tnum indicating the "taxable year,"
    /// as defined under Internal Revenue Code, Section 441.
    /// </summary>
    /// <cite>26 U.S.C. 441 (2010)</cite>    
    /// <updated>2010-11-27</updated>
    /// <remarks>
    /// Covers a 100-year span centered on current year.
    /// Represents an individual's tax year, not a corporate tax year.
    /// Does not implement "52-53 week" tax years (can be done by overloading the method).
    /// Assumes that the tax year takes the number of the year of the greatest overlap.
    /// Assumes that the start month of a person's TY never changes.
    /// </remarks>
    public class Sec441
    {        
        /// <summary>
        /// Returns an individual's tax year. 
        /// </summary>
        public static Tnum TaxYear(Person p)
        {
            // Does not handle cases where start month changes in different tax years!
            // Assumes this value is eternal
            int startMonth = Convert.ToInt32(MonthTaxYearBegins(p).AsOf(Time.DawnOf).ToDecimal);
            
            return TemporalTaxYear(startMonth);
        }
        
        /// <summary>
        /// Creates a Tnum representing the tax year. 
        /// </summary>
        private static Tnum TemporalTaxYear(int monthTaxYearBegins)
        {    
            int currentYear = DateTime.Now.Year;
            int halfSpanInYears = 50;
            
            DateTime TYStartDate = new DateTime(currentYear, monthTaxYearBegins, 1);
            
            // Assumes that TY takes number of the year of with greatest overlap 
            int offset = 0;
            if (monthTaxYearBegins > 6)
                offset = 1;
            
            // Temporal step function
            return Time.IntervalsSince(TYStartDate.AddYears(halfSpanInYears * -1),
                                       TYStartDate.AddYears(halfSpanInYears),
                                       Time.IntervalType.Year,
                                       currentYear - halfSpanInYears + offset);
        }
        
        /// <summary>
        /// Returns the month a person's tax year begins. 
        /// </summary>
        private static Tnum MonthTaxYearBegins(Person p)
        {
            // TODO: Use Switch() here? (consider short-circuiting)
            
            Tnum month = Facts.InputTnum(p, "MonthTaxYearBegins");
            
            // Default the return value to January
            if (month.IsUnknown) { return new Tnum(1); }
            
            return month;
        }
                        
    }
    
}
