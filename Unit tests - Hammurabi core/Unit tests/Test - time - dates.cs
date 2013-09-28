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

using Hammurabi;
using NUnit.Framework;
using System;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class TestDates : H
    {    
        
        // CurrentOrNextBusinessDay
        
        [Test]
        public void CurrentOrNextBusinessDay1 ()
        {
            Assert.AreEqual(Date(2011,2,18), Date(2011,2,18).CurrentOrNextBusinessDay());    
        }
        
        [Test]
        public void CurrentOrNextBusinessDay2 ()
        {
            Assert.AreEqual(Date(2011,2,22), Date(2011,2,19).CurrentOrNextBusinessDay());    
        }
        
        [Test]
        public void CurrentOrNextBusinessDay3 ()
        {
            Assert.AreEqual(Date(2011,2,22), Date(2011,2,20).CurrentOrNextBusinessDay());    
        }
        
        [Test]
        public void CurrentOrNextBusinessDay4 ()
        {
            Assert.AreEqual(Date(2011,2,22), Date(2011,2,21).CurrentOrNextBusinessDay());    
        }
        
        // CurrentOrNextWeekday

        [Test]
        public void CurrentOrNextWeekday1 ()
        {
            Assert.AreEqual(Date(2011,2,18), Date(2011,2,18).CurrentOrNextWeekday());    
        }
        
        [Test]
        public void CurrentOrNextWeekday2 ()
        {
            Assert.AreEqual(Date(2011,2,21), Date(2011,2,19).CurrentOrNextWeekday());    
        }
        
        [Test]
        public void CurrentOrNextWeekday3 ()
        {
            Assert.AreEqual(Date(2011,2,21), Date(2011,2,20).CurrentOrNextWeekday());    
        }
        
        [Test]
        public void CurrentOrNextWeekday4 ()
        {
            Assert.AreEqual(Date(2011,2,21), Date(2011,2,21).CurrentOrNextWeekday());    
        }
        
        // CurrentOrAdjacentWeekday
        
        [Test]
        public void CurrentOrAdjacentWeekday1 ()
        {
            Assert.AreEqual(Date(2011,2,18), Date(2011,2,18).CurrentOrAdjacentWeekday());    
        }
        
        [Test]
        public void CurrentOrAdjacentWeekday2 ()
        {
            Assert.AreEqual(Date(2011,2,18), Date(2011,2,19).CurrentOrAdjacentWeekday());    
        }
        
        [Test]
        public void CurrentOrAdjacentWeekday3 ()
        {
            Assert.AreEqual(Date(2011,2,21), Date(2011,2,20).CurrentOrAdjacentWeekday());    
        }
        
        [Test]
        public void CurrentOrAdjacentWeekday4 ()
        {
            Assert.AreEqual(Date(2011,2,21), Date(2011,2,21).CurrentOrAdjacentWeekday());    
        }
        
        // NthDayOfWeekMonthYear
            
        [Test]
        public void NthDayOfWeekMonthYear1 ()
        {
            DateTime dt = Time.NthDayOfWeekMonthYear(3, DayOfWeek.Friday, 2, 2011);
            Assert.AreEqual(Date(2011,2,18), dt);    
        }
            
        [Test]
        public void NthDayOfWeekMonthYear2 ()
        {
            DateTime dt = Time.NthDayOfWeekMonthYear(1, DayOfWeek.Tuesday, 2, 2011);
            Assert.AreEqual(Date(2011,2,1), dt);    
        }
        
        [Test]
        public void NthDayOfWeekMonthYear3 ()
        {
            DateTime dt = Time.NthDayOfWeekMonthYear(5, DayOfWeek.Tuesday, 3, 2011);
            Assert.AreEqual(Date(2011,3,29), dt);    
        }
            
        // LastDayOfWeekMonthYear
        
        [Test]
        public void LastDayOfWeekMonthYear1 ()
        {
            DateTime dt = Time.LastDayOfWeekMonthYear(DayOfWeek.Friday, 2, 2011);
            Assert.AreEqual(Date(2011,2,25), dt);    
        }
        
        [Test]
        public void LastDayOfWeekMonthYear2 ()
        {
            DateTime dt = Time.LastDayOfWeekMonthYear(DayOfWeek.Tuesday, 4, 2013);
            Assert.AreEqual(Date(2013,4,30), dt);    
        }
        
        // DoWOnOrAfter
        
        [Test]
        public void DoWOnOrAfter1 ()
        {
            DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, Date(2011,2,20));
            Assert.AreEqual(Date(2011,2,25), dt);    
        }
        
        [Test]
        public void DoWOnOrAfter2 ()
        {
            DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, Date(2011,2,25));
            Assert.AreEqual(Date(2011,2,25), dt);    
        }
        
        [Test]
        public void DoWOnOrAfter3 ()
        {
            DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, Date(2011,2,26));
            Assert.AreEqual(Date(2011,3,4), dt);    
        }
        
        // DoWOnOrBefore
        
        [Test]
        public void DoWOnOrBefore1 ()
        {
            DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, Date(2011,2,20));
            Assert.AreEqual(Date(2011,2,18), dt);    
        }
        
        [Test]
        public void DoWOnOrBefore2 ()
        {
            DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, Date(2011,2,18));
            Assert.AreEqual(Date(2011,2,18), dt);    
        }
        
        [Test]
        public void DoWOnOrBefore3 ()
        {
            DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, Date(2011,2,17));
            Assert.AreEqual(Date(2011,2,11), dt);    
        }
        
        // WeekdayCount
        
        [Test]
        public void WeekdayCount1 ()
        {
            int count = Time.WeekdayCount(Date(2011,1,30), Date(2011,3,3));
            Assert.AreEqual(24, count);    
        }
        
        [Test]
        public void WeekdayCount2 ()
        {
            int count = Time.WeekdayCount(Date(2011,2,1), Date(2011,2,1));
            Assert.AreEqual(0, count);    
        }
        
        [Test]
        public void WeekdayCount3 ()
        {
            int count = Time.WeekdayCount(Date(2011,2,1), Date(2011,2,2));
            Assert.AreEqual(1, count);    
        }
        
        // Earliest
        
        [Test]
        public void Earliest1 ()
        {
            DateTime dt = Time.Earliest(Date(2011,1,30), Date(2011,3,3), Date(2013,3,3));
            Assert.AreEqual(Date(2011,1,30), dt);    
        }
        
        [Test]
        public void Earliest2 ()
        {
            DateTime dt = Time.Earliest(Date(2014,1,30), Date(2011,3,3), Date(2013,3,3));
            Assert.AreEqual(Date(2011,3,3), dt);    
        }
        
        // Latest
        
        [Test]
        public void Latest1 ()
        {
            DateTime dt = Time.Latest(Date(2011,1,30), Date(2011,3,3), Date(2013,3,3));
            Assert.AreEqual(Date(2013,3,3), dt);    
        }
        
        [Test]
        public void Latest2 ()
        {
            DateTime dt = Time.Latest(Date(2014,1,30), Date(2011,3,3), Date(2013,3,3));
            Assert.AreEqual(Date(2014,1,30), dt);    
        }
                        
        // .AddCalendarMonths
        
        [Test]
        public void AddCalendarMonths1 ()
        {
            Assert.AreEqual(Date(2011,6,1), Date(2011,4,15).AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths2 ()
        {
            Assert.AreEqual(Date(2011,6,1), Date(2011,5,1).AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths3 ()
        {
            Assert.AreEqual(Date(2011,6,1), Date(2011,4,30).AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths4 ()
        {
            Assert.AreEqual(Date(2011,7,1), Date(2011,4,15).AddCalendarMonths(2));  
        }
    }
}
