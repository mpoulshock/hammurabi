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
	public class TemporalDate : H
	{
		// .Lean
		
		[Test]
		public void Tdate_Lean_1 ()
		{
			Tdate td = new Tdate();
			td.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td.AddState(Time.DawnOf.AddYears(2), new DateTime(2011,01,01));
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/1/2011 12:00:00 AM ", td.Lean.TestOutput);		
		}
		
		// .AsOf
		
		[Test]
		public void Tdate_AsOf_1 ()
		{
			Tdate td = new Tdate();
			td.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td.AddState(Time.DawnOf.AddYears(2), new DateTime(2012,01,01));
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/1/2012 12:00:00 AM ", td.AsOf(Time.DawnOf.AddYears(3)).TestOutput);		
		}
		
		[Test]
		public void Tdate_AsOf_2 ()
		{
			Tdate td = new Tdate();
			td.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td.AddState(Time.DawnOf.AddYears(2), new DateTime(2012,01,01));
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/1/2011 12:00:00 AM ", td.AsOf(Time.DawnOf.AddYears(1)).TestOutput);		
		}
		
		// .IsEver
		
		[Test]
		public void Tdate_IsEver_1 ()
		{
			Tdate td = new Tdate();
			td.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td.AddState(Time.DawnOf.AddYears(2), new DateTime(2012,01,01));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", td.IsEver(new DateTime(2012,01,01)).TestOutput);		
		}
		
		[Test]
		public void Tdate_IsEver_2 ()
		{
			Tdate td = new Tdate();
			td.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td.AddState(Time.DawnOf.AddYears(2), new DateTime(2012,01,01));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", td.IsEver(new DateTime(2021,01,01)).TestOutput);		
		}
		
		// Equals
		
		[Test]
		public void Tdate_Equals_1 ()
		{
			Tdate td1 = new Tdate(2010,5,13);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2010,5,13));
			Tbool result = td1 == td2;
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/0003 12:00:00 AM True ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_Equals_2 ()
		{
			Tdate td1 = new Tdate(2010,5,13);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(2011,01,01));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2010,5,13));
			Tbool result = td1 != td2;
			Assert.AreEqual("1/1/0001 12:00:00 AM True 1/1/0003 12:00:00 AM False ", result.TestOutput);		
		}
		
		// IsBefore / IsAfter
		
		[Test]
		public void Tdate_IsAfter_1 ()
		{
			Tdate td1 = new Tdate(2010,1,1);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(2009,1,1));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2011,1,1));
			Tbool result = td1 > td2;
			Assert.AreEqual("1/1/0001 12:00:00 AM True 1/1/0003 12:00:00 AM False ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_IsBefore_1 ()
		{
			Tdate td1 = new Tdate(2010,1,1);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(2009,1,1));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2011,1,1));
			Tbool result = td1 < td2;
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/0003 12:00:00 AM True ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_IsAtOrAfter_1 ()
		{
			Tdate td1 = new Tdate(2008,1,1);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(2009,1,1));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2008,1,1));
			Tbool result = td1 >= td2;
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/0003 12:00:00 AM True ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_IsAtOrBefore_1 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate();
			td2.AddState(Time.DawnOf, new DateTime(1999,1,1));
			td2.AddState(Time.DawnOf.AddYears(2), new DateTime(2000,1,1));
			td2.AddState(Time.DawnOf.AddYears(5), new DateTime(2008,1,1));
			Tbool result = td2 <= td1;
			Assert.AreEqual("1/1/0001 12:00:00 AM True 1/1/0006 12:00:00 AM False ", result.TestOutput);		
		}
		
		// .AddDays
		
		[Test]
		public void Tdate_AddDays_1 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddDays(3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/4/2000 12:00:00 AM ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_AddDays_2 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddDays(-3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 12/29/1999 12:00:00 AM ", result.TestOutput);		
		}
		
		// .AddMonths
		
		[Test]
		public void Tdate_AddMonths_1 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddMonths(3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 4/1/2000 12:00:00 AM ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_AddMonths_2 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddMonths(-3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 10/1/1999 12:00:00 AM ", result.TestOutput);		
		}
		
		// .AddYears
		
		[Test]
		public void Tdate_AddYears_1 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddYears(3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/1/2003 12:00:00 AM ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_AddYears_2 ()
		{
			Tdate td = new Tdate(2000,1,1);
			Tdate result = td.AddYears(-3);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1/1/1997 12:00:00 AM ", result.TestOutput);		
		}
		
		// DayDiff
		
		[Test]
		public void Tdate_DayDiff_1 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2010,1,1);
			Tnum result = Tdate.DayDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 3653 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_DayDiff_2 ()
		{
			Tdate td1 = new Tdate(2010,1,1);
			Tdate td2 = new Tdate(2000,1,1);
			Tnum result = Tdate.DayDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 3653 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_DayDiff_3 ()
		{
			Tdate td1 = new Tdate(2010,1,1);
			Tdate td2 = new Tdate();
			Tnum result = Tdate.DayDiff(td1,td2);
			Assert.AreEqual("Unknown", result.TestOutput);		
		}
		
		// WeekDiff
		
		[Test]
		public void Tdate_WeekDiff_1 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2000,1,8);
			Tnum result = Tdate.WeekDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_WeekDiff_2 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2000,1,7);
			Tnum result = Tdate.WeekDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_WeekDiff_3 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2000,1,15);
			Tnum result = Tdate.WeekDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 2 ", result.TestOutput);		
		}
		
		// YearDiff
		
		[Test]
		public void Tdate_YearDiff_1 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2010,1,1);
			Tnum result = Tdate.YearDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 10 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_YearDiff_2 ()
		{
			Tdate td1 = new Tdate(2000,1,2);
			Tdate td2 = new Tdate(2010,1,1);
			Tnum result = Tdate.YearDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 9 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_YearDiff_3 ()
		{
			Tdate td1 = new Tdate(2000,1,1);
			Tdate td2 = new Tdate(2000,1,2);
			Tnum result = Tdate.YearDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_YearDiff_4 ()
		{
			Tdate td1 = new Tdate(2000,2,29);
			Tdate td2 = new Tdate(2004,2,29);
			Tnum result = Tdate.YearDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 4 ", result.TestOutput);		
		}
		
		[Test]
		public void Tdate_YearDiff_5 ()
		{
			Tdate td1 = new Tdate(2000,2,29);
			Tdate td2 = new Tdate(2004,2,28);
			Tnum result = Tdate.YearDiff(td1,td2);
			Assert.AreEqual("1/1/0001 12:00:00 AM 3 ", result.TestOutput);		
		}
		
		
	}
}
	
	
	