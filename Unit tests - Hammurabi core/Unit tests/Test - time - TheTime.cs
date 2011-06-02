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
	public class TestTheTime : H
	{
		// IsAtOrAfter
		
		[Test]
		public void IsAtOrAfter1 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", afterY2K.AsOf(Date(2012,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsAtOrAfter2 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", afterY2K.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsAtOrAfter3 ()
		{
			Tbool afterY2K = TheTime.IsAtOrAfter(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", afterY2K.AsOf(Date(2000,1,1)).TestOutput);		
		}
		
		// IsBefore
		
		[Test]
		public void IsBefore1 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", beforeY2K.AsOf(Date(2012,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBefore2 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", beforeY2K.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBefore3 ()
		{
			Tbool beforeY2K = TheTime.IsBefore(Date(2000,1,1));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", beforeY2K.AsOf(Date(2000,1,1)).TestOutput);		
		}
		
		// IsBetween
		
		[Test]
		public void IsBetween0 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/20/2001 12:00:00 AM True 1/20/2009 12:00:00 AM False ", isDuringTheBushYears.TestOutput);		
		}
		
		[Test]
		public void IsBetween1 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", isDuringTheBushYears.AsOf(Date(1999,1,1)).TestOutput);		
		}
		
		[Test]
		public void IsBetween2 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isDuringTheBushYears.AsOf(Date(2001,1,20)).TestOutput);			
		}
		
		[Test]
		public void IsBetween3 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isDuringTheBushYears.AsOf(Date(2008,1,1)).TestOutput);			
		}
		
		[Test]
		public void IsBetween4 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isDuringTheBushYears.AsOf(Date(2009,1,20)).TestOutput);			
		}
		
		[Test]
		public void IsBetween5 ()
		{
			Tbool isDuringTheBushYears = TheTime.IsBetween(Date(2001,1,20), Date(2009,1,20));	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", isDuringTheBushYears.AsOf(Date(2012,1,1)).TestOutput);			
		}
		
		// TheYear
		
		[Test]
		public void TheYear1 ()
		{	
			// This test will break every new calendar year (b/c the time frame of TheYear is determined by the system clock)
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 1/1/2006 12:00:00 AM 2006 1/1/2007 12:00:00 AM 2007 1/1/2008 12:00:00 AM 2008 1/1/2009 12:00:00 AM 2009 1/1/2010 12:00:00 AM 2010 1/1/2011 12:00:00 AM 2011 1/1/2012 12:00:00 AM 2012 1/1/2013 12:00:00 AM 2013 1/1/2014 12:00:00 AM 2014 1/1/2015 12:00:00 AM 2015 1/1/2016 12:00:00 AM 0 ", TheTime.Year(5).TestOutput);		
		}
		
		[Test]
		public void TheYear2 ()
		{
			Tbool isAfterY2K = TheYear > 2000; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isAfterY2K.AsOf(DateTime.Now).TestOutput);		
		}
		
		public void TheYear3 ()
		{
			Tbool isAfterY2K = TheYear > 2000; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isAfterY2K.AsOf(Date(1999,12,31)).TestOutput);		
		}
		
		// TheQuarter
		
		[Test]
		public void TheQuarter1 ()
		{
			Tbool is4thQtr = TheQuarter == 4; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", is4thQtr.AsOf(Date(2015,11,15)).TestOutput);		
		}
		
		[Test]
		public void TheQuarter2 ()
		{
			Tbool is4thQtr = TheQuarter == 4; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", is4thQtr.AsOf(Date(2015,3,15)).TestOutput);		
		}
		
		[Test]
		public void TheQuarter3 ()
		{
			// It should never be the 5th quarter
			Tbool is5thQtr = TheQuarter == 5; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", is5thQtr.IsEver(true).TestOutput);		
		}
		
		[Test]
		public void TheQuarter4 ()
		{
			// The quarter is numbered 0 outside of the default 20 year time span
			Tbool is0thQtr = TheQuarter == 0; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", is0thQtr.IsEver(true).TestOutput);		
		}
		
		[Test]
		public void TheQuarter5 ()
		{
			// It should be the 2nd quarter sometime
			Tbool is2ndQtr = TheQuarter == 2; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", is2ndQtr.IsEver(true).TestOutput);		
		}
		
		// TheMonth
		
		[Test]
		public void TheMonth1 ()
		{
			Tbool isApril = TheMonth == 4; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", isApril.AsOf(Date(2015,3,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth2 ()
		{
			Tbool isApril = TheMonth == 4; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isApril.AsOf(Date(2015,4,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth3 ()
		{
			Tbool isAfterJuly = TheMonth > 7; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", isAfterJuly.AsOf(Date(2015,4,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth4 ()
		{
			Tbool isAfterJuly = TheMonth > 7; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isAfterJuly.AsOf(Date(2015,8,15)).TestOutput);		
		}
		
		[Test]
		public void TheMonth5 ()
		{
			// It should never be month 13
			Tbool isMonth13 = TheMonth == 13; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", isMonth13.IsEver(true).TestOutput);		
		}
		
		[Test]
		public void TheMonth6 ()
		{
			// The month is numbered 0 outside of the default 10 year time span
			Tbool isMonth0 = TheMonth == 0; 	
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", isMonth0.IsEver(true).TestOutput);		
		}
        
		// Tnum.Max
		
		[Test]
		public void Max1 ()
		{
			Assert.AreEqual("1/1/0001 12:00:00 AM 4 ", TheQuarter.Max().TestOutput);		
		}
		
		[Test]
		public void Max2 ()
		{
			Assert.AreEqual("1/1/0001 12:00:00 AM 12 ", TheMonth.Max().TestOutput);		
		}
		
		// Tnum.Min
		
		[Test]
		public void Min1 ()
		{
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", TheTime.TheQuarter.Min().TestOutput);		
		}
		
		[Test]
		public void Min2 ()
		{
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", TheMonth.Min().TestOutput);		
		}
	}
}