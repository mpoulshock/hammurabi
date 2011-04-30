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

namespace USC.Tit26
{
    /// <summary>
    /// Definitions relevant to the Federal Unemployment Tax Act (FUTA).
    /// </summary>
    /// <cite>26 U.S.C. 3306 (2011)</cite>   
    /// <updated>2011-04-10</updated>
    public class Sec3306 : H
    {
        /// <summary>
        /// Determines whether a corporate entity is an "employer" under FUTA.
        /// </summary>
        public static Tbool IsEmployer(CorporateEntity c)
        {
            return a1(c) | a2(c) | a3(c);
        }
        
        /// <summary>
        /// Employer - general, defined
        /// </summary>
        private static Tbool a1(CorporateEntity c)
        {
            // Paid $1,500 or more in wages in any calendar quarter in current or the preceding calendar year 
            Tbool QWages1500 = (QuarterlyGeneralWagesPaid(c) >= 1500).EverPer(TheTime.TheQuarter);
            Tbool wageTest = QWages1500.CountPastNIntervals(TheTime.TheYear,2) > 0;
            
            // Employed at least one individual...on each of some 20 days during the calendar year or 
            // during the preceding calendar year, each day being in a different calendar week
            Tbool hasEmp = (NumberOfGeneralEmployees(c) >= 1).EverPer(TheTime.TheCalendarWeek);
            Tbool weekTest = (hasEmp.CountPer(TheTime.TheYear) >= 20).CountPastNIntervals(TheTime.TheYear,2) > 0;
            
            return wageTest | weekTest;
        }
        
        /// <summary>
        /// Employer - agricultural, defined
        /// </summary>
        private static Tbool a2(CorporateEntity c)
        {   
            // Paid $20,000 or more in wages in any calendar quarter in current or the preceding calendar year 
            Tbool QWages20K = (QuarterlyAgWagesPaid(c) >= 20000).EverPer(TheTime.TheQuarter);
            Tbool wageTest = QWages20K.CountPastNIntervals(TheTime.TheYear,2) > 0;
            
            // Employed at least 10 individuals...on each of some 20 days during the calendar year or 
            // during the preceding calendar year, each day being in a different calendar week
            Tbool hasEmp = (NumberOfAgEmployees(c) >= 10).EverPer(TheTime.TheCalendarWeek);
            Tbool weekTest = (hasEmp.CountPer(TheTime.TheYear) >= 20).CountPastNIntervals(TheTime.TheYear,2) > 0;
            
            return wageTest | weekTest;
        }
        
        /// <summary>
        /// Employer - domestic, defined
        /// </summary>
        private static Tbool a3(CorporateEntity c)
        {
            // Paid $1,000 or more in wages in any calendar quarter in current or the preceding calendar year 
            Tbool QWages1K = (QuarterlyDomesticWagesPaid(c) >= 1000).EverPer(TheTime.TheQuarter);
            Tbool wageTest = QWages1K.CountPastNIntervals(TheTime.TheYear,2) > 0;
            
            return wageTest;
        }
        
        
        // ********************************************************************
        // Inputs - not sure I like these...refactor?
        // ********************************************************************
        
        /// <summary>
        /// Number of general, not agricultural or domestic, employees
        /// </summary>
        private static Tnum NumberOfGeneralEmployees(CorporateEntity c)
        {
            return Facts.InputTnum(c,"NumberOfGeneralEmployees");
        }
        
        /// <summary>
        /// Number of agricultural employees.
        /// </summary>
        private static Tnum NumberOfAgEmployees(CorporateEntity c)
        {
            return Facts.InputTnum(c,"NumberOfAgriculturalEmployees");
        }
        
        /// <summary>
        /// Wages paid for general, not agricultural or domestic, employment.
        /// This is a Tnum that varies quarterly.
        /// </summary>
        private static Tnum QuarterlyGeneralWagesPaid(CorporateEntity c)
        {
            return Facts.InputTnum(c,"QuarterlyGeneralWagesPaid"); 
        }
        
        /// <summary>
        /// Wages paid for agricultural employment.
        /// This is a Tnum that varies quarterly.
        /// </summary>
        private static Tnum QuarterlyAgWagesPaid(CorporateEntity c)
        {
            return Facts.InputTnum(c,"QuarterlyAgriculturalWagesPaid"); 
        }
        
        /// <summary>
        /// Wages paid for domestic employment.
        /// This is a Tnum that varies quarterly.
        /// </summary>
        private static Tnum QuarterlyDomesticWagesPaid(CorporateEntity c)
        {
            return Facts.InputTnum(c,"QuarterlyDomesticWagesPaid"); 
        }
        
        
    }
}