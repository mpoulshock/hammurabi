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

using Hammurabi;
using NUnit.Framework;
using System;

namespace Hammurabi.UnitTests.CoreFcns
{
	[TestFixture]
	public class TestTheTime : H
	{
		// IsAtOrAfter
		
		[Test]
		public void IsAtOrAfter1 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf True ", afterY2K.AsOf(Date(2012,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsAtOrAfter2 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf False ", afterY2K.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsAtOrAfter3 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf True ", afterY2K.AsOf(Date(2000,1,1)).TestOutput);		
		}
		
        [Test]
        public void IsAtOrAfter4 ()
        {
            Tdate d = new Tdate(Hstate.Uncertain);
            Tbool afterY2K = TheTime.IsAtOrAfter(d);   
            Assert.AreEqual("Time.DawnOf Uncertain ", afterY2K.TestOutput);        
        }

		// IsBefore
		
		[Test]
		public void IsBefore1 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf False ", beforeY2K.AsOf(Date(2012,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBefore2 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf True ", beforeY2K.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBefore3 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("Time.DawnOf False ", beforeY2K.AsOf(Date(2000,1,1)).TestOutput);		
		}
		
		// IsBetween
		
		[Test]
		public void IsBetween0 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf False 1/20/2001 12:00:00 AM True 1/20/2009 12:00:00 AM False ", isDuringTheBushYears.TestOutput);		
		}
		
		[Test]
		public void IsBetween1 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf False ", isDuringTheBushYears.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBetween2 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf True ", isDuringTheBushYears.AsOf(Date(2001,1,20)).TestOutput);			
		}
		
		[Test]
		public void IsBetween3 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf True ", isDuringTheBushYears.AsOf(Date(2008,1,1)).TestOutput);			
		}
		
		[Test]
		public void IsBetween4 () 
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf False ", isDuringTheBushYears.AsOf(Date(2009,1,20)).TestOutput);			
		}
		
		[Test]
		public void IsBetween5 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("Time.DawnOf False ", isDuringTheBushYears.AsOf(Date(2012,1,1)).TestOutput);			
		}
		
        [Test]
        public void IsBetween6 ()
        {
            Tdate d = new Tdate(Hstate.Uncertain);
            Tbool isDuringTheBushYears = TheTime.IsBetween(d, Date(2009,1,20));   
            Assert.AreEqual("Time.DawnOf Uncertain 1/20/2009 12:00:00 AM False ", isDuringTheBushYears.TestOutput);           
        }

		// TheYear
		
		[Test]
		public void TheYear1 ()
		{	
			// This test will break every new calendar year (b/c the time frame of TheYear is determined by the system clock)
			// Last updated: 4/28/12
            Assert.AreEqual("Time.DawnOf 0 1/1/2010 12:00:00 AM 2010 1/1/2011 12:00:00 AM 2011 1/1/2012 12:00:00 AM 2012 1/1/2013 12:00:00 AM 2013 1/1/2014 12:00:00 AM 0 ", TheTime.Year(2).TestOutput);		
		}
		
		[Test]
		public void TheYear2 ()
		{
			Tbool isAfterY2K = TheYear > 2000; 	
			Assert.AreEqual("Time.DawnOf True ", isAfterY2K.AsOf(DateTime.Now).TestOutput);		
		}
		
		public void TheYear3 ()
		{
			Tbool isAfterY2K = TheYear > 2000; 	
			Assert.AreEqual("Time.DawnOf True ", isAfterY2K.AsOf(Date(1999,12,31)).TestOutput);		
		}
		
		// TheQuarter
		
		[Test]
		public void TheQuarter1 ()
		{
			Tbool is4thQtr = TheQuarter == 4; 	
			Assert.AreEqual("Time.DawnOf True ", is4thQtr.AsOf(Date(2015,11,15)).TestOutput);		
		}
		
		[Test]
		public void TheQuarter2 ()
		{
			Tbool is4thQtr = TheQuarter == 4; 	
			Assert.AreEqual("Time.DawnOf False ", is4thQtr.AsOf(Date(2015,3,15)).TestOutput);		
		}
		
		[Test]
		public void TheQuarter3 ()
		{
			// It should never be the 5th quarter
			Tbool is5thQtr = TheQuarter == 5; 	
			Assert.AreEqual("Time.DawnOf False ", is5thQtr.IsEverTrue().TestOutput);		
		}
		
		[Test]
		public void TheQuarter4 ()
		{
			// The quarter is numbered 0 outside of the default 20 year time span
			Tbool is0thQtr = TheQuarter == 0; 	
			Assert.AreEqual("Time.DawnOf True ", is0thQtr.IsEverTrue().TestOutput);		
		}
		
		[Test]
		public void TheQuarter5 ()
		{
			// It should be the 2nd quarter sometime
			Tbool is2ndQtr = TheQuarter == 2; 	
			Assert.AreEqual("Time.DawnOf True ", is2ndQtr.IsEverTrue().TestOutput);		
		}
		
		// TheMonth
		
		[Test]
		public void TheMonth1 ()
		{
			Tbool isApril = TheMonth == 4; 	
			Assert.AreEqual("Time.DawnOf False ", isApril.AsOf(Date(2015,3,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth2 ()
		{
			Tbool isApril = TheMonth == 4; 	
			Assert.AreEqual("Time.DawnOf True ", isApril.AsOf(Date(2015,4,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth3 ()
		{
			Tbool isAfterJuly = TheMonth > 7; 	
			Assert.AreEqual("Time.DawnOf False ", isAfterJuly.AsOf(Date(2015,4,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth4 ()
		{
			Tbool isAfterJuly = TheMonth > 7; 	
			Assert.AreEqual("Time.DawnOf True ", isAfterJuly.AsOf(Date(2015,8,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth5 ()
		{
			// It should never be month 13
			Tbool isMonth13 = TheMonth == 13; 	
			Assert.AreEqual("Time.DawnOf False ", isMonth13.IsEverTrue().TestOutput);		
		}
		
		[Test]
		public void TheMonth6 ()
		{
			// The month is numbered 0 outside of the default 10 year time span
			Tbool isMonth0 = TheMonth == 0; 	
			Assert.AreEqual("Time.DawnOf True ", isMonth0.IsEverTrue().TestOutput);		
		}
        
		// Tnum.Max
		
		[Test]
		public void Max1 ()
		{
			Assert.AreEqual("Time.DawnOf 4 ", TheQuarter.Max().TestOutput);		
		}
		
		[Test]
		public void Max2 ()
		{
			Assert.AreEqual("Time.DawnOf 12 ", TheMonth.Max().TestOutput);		
		}
		
		// Tnum.Min
		
		[Test]
		public void Min1 ()
		{
			Assert.AreEqual("Time.DawnOf 0 ", TheTime.TheQuarter.Min().TestOutput);		
		}
		
		[Test]
		public void Min2 ()
		{
			Assert.AreEqual("Time.DawnOf 0 ", TheMonth.Min().TestOutput);		
		}
	}
}