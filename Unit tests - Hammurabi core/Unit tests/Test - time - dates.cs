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

using Hammurabi;
using NUnit.Framework;
using System;

namespace Hammurabi.UnitTests.CoreFcns
{
	[TestFixture]
	public class TestDates : H
	{	
		
		// CurrentOrNextBusinessDay
		
		[Test]
		public void CurrentOrNextBusinessDay1 ()
		{
			DateTime dt = new DateTime(2011,2,18).CurrentOrNextBusinessDay();
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void CurrentOrNextBusinessDay2 ()
		{
			DateTime dt = new DateTime(2011,2,19).CurrentOrNextBusinessDay();
			Assert.AreEqual(new DateTime(2011,2,22), dt);	
		}
		
		[Test]
		public void CurrentOrNextBusinessDay3 ()
		{
			DateTime dt = new DateTime(2011,2,20).CurrentOrNextBusinessDay();
			Assert.AreEqual(new DateTime(2011,2,22), dt);	
		}
		
		[Test]
		public void CurrentOrNextBusinessDay4 ()
		{
			DateTime dt = new DateTime(2011,2,21).CurrentOrNextBusinessDay();
			Assert.AreEqual(new DateTime(2011,2,22), dt);	
		}
		
		// CurrentOrNextWeekday

		[Test]
		public void CurrentOrNextWeekday1 ()
		{
			DateTime dt = new DateTime(2011,2,18).CurrentOrNextWeekday();
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void CurrentOrNextWeekday2 ()
		{
			DateTime dt = new DateTime(2011,2,19).CurrentOrNextWeekday();
			Assert.AreEqual(new DateTime(2011,2,21), dt);	
		}
		
		[Test]
		public void CurrentOrNextWeekday3 ()
		{
			DateTime dt = new DateTime(2011,2,20).CurrentOrNextWeekday();
			Assert.AreEqual(new DateTime(2011,2,21), dt);	
		}
		
		[Test]
		public void CurrentOrNextWeekday4 ()
		{
			DateTime dt = new DateTime(2011,2,21).CurrentOrNextWeekday();
			Assert.AreEqual(new DateTime(2011,2,21), dt);	
		}
		
		// CurrentOrAdjacentWeekday
		
		[Test]
		public void CurrentOrAdjacentWeekday1 ()
		{
			DateTime dt = new DateTime(2011,2,18).CurrentOrAdjacentWeekday();
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void CurrentOrAdjacentWeekday2 ()
		{
			DateTime dt = new DateTime(2011,2,19).CurrentOrAdjacentWeekday();
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void CurrentOrAdjacentWeekday3 ()
		{
			DateTime dt = new DateTime(2011,2,20).CurrentOrAdjacentWeekday();
			Assert.AreEqual(new DateTime(2011,2,21), dt);	
		}
		
		[Test]
		public void CurrentOrAdjacentWeekday4 ()
		{
			DateTime dt = new DateTime(2011,2,21).CurrentOrAdjacentWeekday();
			Assert.AreEqual(new DateTime(2011,2,21), dt);	
		}
		
		// NthDayOfWeekMonthYear
			
		[Test]
		public void NthDayOfWeekMonthYear1 ()
		{
			DateTime dt = Time.NthDayOfWeekMonthYear(3, DayOfWeek.Friday, 2, 2011);
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
			
		[Test]
		public void NthDayOfWeekMonthYear2 ()
		{
			DateTime dt = Time.NthDayOfWeekMonthYear(1, DayOfWeek.Tuesday, 2, 2011);
			Assert.AreEqual(new DateTime(2011,2,1), dt);	
		}
		
		[Test]
		public void NthDayOfWeekMonthYear3 ()
		{
			DateTime dt = Time.NthDayOfWeekMonthYear(5, DayOfWeek.Tuesday, 3, 2011);
			Assert.AreEqual(new DateTime(2011,3,29), dt);	
		}
			
		// LastDayOfWeekMonthYear
		
		[Test]
		public void LastDayOfWeekMonthYear1 ()
		{
			DateTime dt = Time.LastDayOfWeekMonthYear(DayOfWeek.Friday, 2, 2011);
			Assert.AreEqual(new DateTime(2011,2,25), dt);	
		}
		
		[Test]
		public void LastDayOfWeekMonthYear2 ()
		{
			DateTime dt = Time.LastDayOfWeekMonthYear(DayOfWeek.Tuesday, 4, 2013);
			Assert.AreEqual(new DateTime(2013,4,30), dt);	
		}
		
		// DoWOnOrAfter
		
		[Test]
		public void DoWOnOrAfter1 ()
		{
			DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, new DateTime(2011,2,20));
			Assert.AreEqual(new DateTime(2011,2,25), dt);	
		}
		
		[Test]
		public void DoWOnOrAfter2 ()
		{
			DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, new DateTime(2011,2,25));
			Assert.AreEqual(new DateTime(2011,2,25), dt);	
		}
		
		[Test]
		public void DoWOnOrAfter3 ()
		{
			DateTime dt = Time.DoWOnOrAfter(DayOfWeek.Friday, new DateTime(2011,2,26));
			Assert.AreEqual(new DateTime(2011,3,4), dt);	
		}
		
		// DoWOnOrBefore
		
		[Test]
		public void DoWOnOrBefore1 ()
		{
			DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, new DateTime(2011,2,20));
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void DoWOnOrBefore2 ()
		{
			DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, new DateTime(2011,2,18));
			Assert.AreEqual(new DateTime(2011,2,18), dt);	
		}
		
		[Test]
		public void DoWOnOrBefore3 ()
		{
			DateTime dt = Time.DoWOnOrBefore(DayOfWeek.Friday, new DateTime(2011,2,17));
			Assert.AreEqual(new DateTime(2011,2,11), dt);	
		}
		
		// WeekdayCount
		
		[Test]
		public void WeekdayCount1 ()
		{
			int ct = Time.WeekdayCount(new DateTime(2011,1,30), new DateTime(2011,3,3));
			Assert.AreEqual(24, ct);	
		}
		
		[Test]
		public void WeekdayCount2 ()
		{
			int ct = Time.WeekdayCount(new DateTime(2011,2,1), new DateTime(2011,2,1));
			Assert.AreEqual(0, ct);	
		}
		
		[Test]
		public void WeekdayCount3 ()
		{
			int ct = Time.WeekdayCount(new DateTime(2011,2,1), new DateTime(2011,2,2));
			Assert.AreEqual(1, ct);	
		}
		
		// Earliest
		
		[Test]
		public void Earliest1 ()
		{
			DateTime dt = Time.Earliest(new DateTime(2011,1,30), new DateTime(2011,3,3), new DateTime(2013,3,3));
			Assert.AreEqual(new DateTime(2011,1,30), dt);	
		}
		
		[Test]
		public void Earliest2 ()
		{
			DateTime dt = Time.Earliest(new DateTime(2014,1,30), new DateTime(2011,3,3), new DateTime(2013,3,3));
			Assert.AreEqual(new DateTime(2011,3,3), dt);	
		}
		
		// Latest
		
		[Test]
		public void Latest1 ()
		{
			DateTime dt = Time.Latest(new DateTime(2011,1,30), new DateTime(2011,3,3), new DateTime(2013,3,3));
			Assert.AreEqual(new DateTime(2013,3,3), dt);	
		}
		
		[Test]
		public void Latest2 ()
		{
			DateTime dt = Time.Latest(new DateTime(2014,1,30), new DateTime(2011,3,3), new DateTime(2013,3,3));
			Assert.AreEqual(new DateTime(2014,1,30), dt);	
		}
		
		// .ToMidnight
		
		[Test]
		public void ToMidnight1 ()
		{
			DateTime dt = new DateTime(2014,3,15,6,6,6);
			Assert.AreEqual(new DateTime(2014,3,15), dt.ToMidnight());	
		}
		
		// .FirstDayOfYear
		
		[Test]
		public void FirstDayOfYear1 ()
		{
			DateTime dt = new DateTime(2014,3,15,6,6,6);
			Assert.AreEqual(new DateTime(2014,1,1), dt.FirstDayOfYear());	
		}
		
		// .LastDayOfYear
		
		[Test]
		public void LastDayOfYear1 ()
		{
			DateTime dt = new DateTime(2014,3,15,6,6,6);
			Assert.AreEqual(new DateTime(2014,12,31), dt.LastDayOfYear());	
		}
		
        // .AddCalendarMonths
        
        [Test]
        public void AddCalendarMonths1 ()
        {
            DateTime dt = new DateTime(2011,4,15);
            Assert.AreEqual(new DateTime(2011,6,1), dt.AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths2 ()
        {
            DateTime dt = new DateTime(2011,5,1);
            Assert.AreEqual(new DateTime(2011,6,1), dt.AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths3 ()
        {
            DateTime dt = new DateTime(2011,4,30);
            Assert.AreEqual(new DateTime(2011,6,1), dt.AddCalendarMonths(1));  
        }
        
        [Test]
        public void AddCalendarMonths4 ()
        {
            DateTime dt = new DateTime(2011,4,15);
            Assert.AreEqual(new DateTime(2011,7,1), dt.AddCalendarMonths(2));  
        }
        
	}
}
