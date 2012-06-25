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
    public partial class Facts
    {
        /*
         * The "window of concern" is the range of time that a user is interested
         * in getting a determination about.  
         * 
         * For example, if the user wants to know their tax liability for 2016,
         * the window of concern might start on 2016-01-01 and end on 2016-12-31.
         * 
         * The significance of the window of concern is that it allows boolean
         * expressions to short-circuit if they are true or false (depending on
         * whether it is a conjunction or disjunction) within the specified
         * time period.
         * 
         * Without a window of concern (or when the window encompasses all of time), 
         * some expressions would never short-circuit because variables within
         * that expression have values that change over irrelevant spans of time.
         * 
         * Setting the window to something other than the default is really only
         * necessary when calling legal rules in an interactive interview.  
         * (This is when short-circuit evaluation matters.)  
         * 
         * Setting the window too narrowly could cause erroneous determinations, as
         * it could override the temporal logic of some rules.
         * 
         * Since the bounds of the window must be checked every time && or || are
         * called, boolean fields are used below to improve performance.  (They
         * reduce the number of date comparisons that must be made.)
         */
        
        /// <summary>
        /// Default point in time in which the window of concern starts.
        /// </summary>
        private static DateTime WindowOfConcernStartDate = Time.DawnOf;
        
        /// <summary>
        /// Returns the point in time in which the window of concern starts.
        /// </summary>
        public static DateTime WindowOfConcernStart
        {
            get { return WindowOfConcernStartDate; }
        }
        
        /// <summary>
        /// Default point in time in which the window of concern ends.
        /// </summary>
        private static DateTime WindowOfConcernEndDate = Time.EndOf;
        
        /// <summary>
        /// Returns the point in time in which the window of concern ends.
        /// </summary>
        public static DateTime WindowOfConcernEnd
        {
            get { return WindowOfConcernEndDate; }
        }
        
        /// <summary>
        /// Indicates whether the window of concern is the default (all of time).
        /// </summary>
        private static bool WoCIsDefault = true;
        
        /// <summary>
        /// Returns the point in time in which the "window of concern" ends.
        /// </summary>
        public static bool WindowOfConcernIsDefault
        {
            get { return WoCIsDefault; }
        }
        
        /// <summary>
        /// By default, the window of concern is not a single point in time.
        /// </summary>
        private static bool WoCIsPoint = false;
        
        /// <summary>
        /// Indicates whether the window of concern is a single point in time.
        /// </summary>
        public static bool WindowOfConcernIsPoint
        {
            get { return WoCIsPoint; }
        }
        
        /// <summary>
        /// Sets the window of time in which facts are relevant.
        /// </summary>
        public static void SetWindowOfConcern(DateTime start, DateTime end)
        {
            WindowOfConcernStartDate = start;
            WindowOfConcernEndDate = end;
            
            WoCIsDefault = false;
            WoCIsPoint = false;
        }
        
        /// <summary>
        /// Sets the window of time in which facts are relevant to a given
        /// date.
        /// </summary>
        public static void SetWindowOfConcern(DateTime dateOfConcern)
        {
            WindowOfConcernStartDate = dateOfConcern;
            WindowOfConcernEndDate = dateOfConcern;
            
            WoCIsDefault = false;
            WoCIsPoint = true;
        }
        
        /// <summary>
        /// Sets the window of time in which facts are relevant to a span
        /// of 2*n years centered on the current date
        /// </summary>
        public static void SetWindowOfConcern(int halfSpanInYears)
        {
            WindowOfConcernStartDate = DateTime.Now.AddYears(halfSpanInYears * -1);
            WindowOfConcernEndDate = DateTime.Now.AddYears(halfSpanInYears);
            
            WoCIsDefault = false;
            WoCIsPoint = false;
        }
    }
}

