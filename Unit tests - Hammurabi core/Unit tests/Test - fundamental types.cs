
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
	public class FundamentalTypes
	{
		// .Lean
		
		[Test]
		public void FT_Lean_1 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), true);
			Tbool res = t.Lean;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		// .AsOf
		
		[Test]
		public void FT_AsOf_1 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.AsOf(Time.DawnOf.AddYears(2));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_2 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.AsOf(Time.DawnOf.AddYears(12));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_3 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			Tbool res = t.AsOf(Time.DawnOf.AddYears(12));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_4 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 4);
			t.AddState(Time.DawnOf.AddYears(5), 44);
			Tnum res = t.AsOf(Time.DawnOf.AddYears(2));
			Assert.AreEqual("1/1/0001 12:00:00 AM 4 ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_5 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 4);
			t.AddState(Time.DawnOf.AddYears(5), 44);
			Tnum res = t.AsOf(Time.DawnOf.AddYears(12));
			Assert.AreEqual("1/1/0001 12:00:00 AM 44 ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_6 ()
		{
			Tstr t = new Tstr();
			t.AddState(Time.DawnOf, "ham");
			t.AddState(Time.DawnOf.AddYears(5), "sam");
			Tstr res = t.AsOf(Time.DawnOf.AddYears(12));
			Assert.AreEqual("1/1/0001 12:00:00 AM sam ", res.TestOutput);		
		}
		
		[Test]
		public void FT_AsOf_7 ()
		{
			Tstr t = new Tstr();
			t.AddState(Time.DawnOf, "ham");
			t.AddState(Time.DawnOf.AddYears(5), "sam");
			Tstr res = t.AsOf(Time.DawnOf.AddYears(2));
			Assert.AreEqual("1/1/0001 12:00:00 AM ham ", res.TestOutput);		
		}
		
		// .IsAlways
		
		[Test]
		public void FT_IsAlways_1 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(true);
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_2 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(false);
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_3 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			Tbool res = t.IsAlways(true);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_4 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(true, Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_5 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(true, Time.DawnOf.AddYears(2), Time.DawnOf.AddYears(3));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_6 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(true, Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_7 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsAlways(false, Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_8 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			Tbool res = t.IsAlways(true, Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_9 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			Tbool res = t.IsAlways(false, Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_10 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			Tbool res = t.IsAlways(false);
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_11 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 3.4);
			Tbool res = t.IsAlways(3.4);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsAlways_12 ()
		{
			Tstr t = new Tstr("jello");
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.IsAlways("jello").TestOutput);		
		}
		
		// .IsEver
		
		[Test]
		public void FT_IsEver_1 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, true);
			t.AddState(Time.DawnOf.AddYears(5), false);
			Tbool res = t.IsEver(true);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_2 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			Tbool res = t.IsEver(true);
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_3 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 3);
			Tbool res = t.IsEver(3);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_4 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 3.3);
			Tbool res = t.IsEver(3.3);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_5 ()
		{
			Tstr t = new Tstr("jello");
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.IsEver("jello").TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_6 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(Time.DawnOf.AddYears(5), true);
			Tbool res = t.IsEver(false, Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_7 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			Tbool res = t.IsEver(false);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);			
		}
		
		[Test]
		public void FT_IsEver_8 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			Tbool res = t.IsEver(true);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);			
		}
		
		[Test]
		public void FT_IsEver_9 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			Tbool res = t.IsEver(true, new DateTime(2013,1,1), new DateTime(2014,1,1));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);			
		}
		
		[Test]
		public void FT_IsEver_10 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			Tbool res = t.IsEver(true, new DateTime(2012,1,1), new DateTime(2013,1,1));
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);			
		}
		
		[Test]
		public void FT_IsEver_11 ()
		{
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			Tbool res = t.IsEver(true, new DateTime(2011,1,1), new DateTime(2012,1,1));
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);			
		}
		
		[Test]
		public void FT_IsEver_12 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 3.3);
			Tbool res = t.IsEver(3.4);
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);		
		}
		
		[Test]
		public void FT_IsEver_13 ()
		{
			Tbool t = new Tbool();
			Tbool res = t.IsEver(false, Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
			Assert.AreEqual("Unknown", res.TestOutput);		
		}
		
		// .ToBool
		
		[Test]
		public void FT_ToBool_1 ()
		{
			Tbool t = new Tbool(true);
			Assert.AreEqual(true, t.ToBool);		
		}
		
		[Test]
		public void FT_ToBool_2 ()
		{
			Tbool t = new Tbool(false);
			Assert.AreEqual(false, t.ToBool);		
		}
		
		[Test]
		public void FT_ToBool_3 ()
		{
			// If value is not eternal, .ToBool should return null
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(Time.DawnOf.AddYears(5), true);
			Assert.AreEqual(null, t.ToBool);		
		}
		
		[Test]
		public void FT_ToBool_4 ()
		{
			// If value is unknown, .ToBool should return null
			Tbool t = new Tbool();
			Assert.AreEqual(null, t.ToBool);		
		}
		
		// .EverPerInterval
		
		[Test]
		public void FT_EverPerInterval_1 ()
		{
			// This will break annually b/c Year is determined by the system clock
			Tnum theYear = TheTime.Year(5);
			
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			
			Tbool result = t.EverPer(theYear).Lean;
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/2012 12:00:00 AM True ", result.TestOutput);		
		}
		
		[Test]
		public void FT_EverPerInterval_2 ()
		{
			Tnum theYear = TheTime.Year(5);
			Tbool t = new Tbool();
			Tbool result = t.EverPer(theYear);
			Assert.AreEqual("Unknown", result.TestOutput);		
		}
		
		// .AlwaysPerInterval
		
		[Test]
		public void FT_AlwaysPerInterval_1 ()
		{
			// This will break annually b/c Year is determined by the system clock
			Tnum theYear = TheTime.Year(5);
			
			Tbool t = new Tbool();
			t.AddState(Time.DawnOf, false);
			t.AddState(new DateTime(2012,11,8), true);
			
			Tbool result = t.AlwaysPer(theYear).Lean;
			Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/2013 12:00:00 AM True ", result.TestOutput);		
		}
		
		[Test]
		public void FT_AlwaysPerInterval_2 ()
		{
			Tnum theYear = TheTime.Year(5);
			Tbool t = new Tbool();
			Tbool result = t.AlwaysPer(theYear);
			Assert.AreEqual("Unknown", result.TestOutput);		
		}
		
		// .ToInt
		
		[Test]
		public void FT_ToInt_1 ()
		{
			Tnum t = new Tnum(42);
			Assert.AreEqual(42, t.ToInt);		
		}
		
		[Test]
		public void FT_ToInt_2 ()
		{
			Tnum t = new Tnum();
			Assert.AreEqual(null, t.ToInt);		
		}
		
		[Test]
		public void FT_ToInt_3 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 42);
			t.AddState(new DateTime(1912,5,3), 43);
			Assert.AreEqual(null, t.ToInt);		
		}
		
		// Tnum.ToDecimal
		
		[Test]
		public void FT_ToDecimal_1 ()
		{
			Tnum t = new Tnum(42.1);
			Assert.AreEqual(42.1, t.ToDecimal);		
		}
		
		[Test]
		public void FT_ToDecimal_2 ()
		{
			Tnum t = new Tnum();
			Assert.AreEqual(null, t.ToDecimal);		
		}
		
		[Test]
		public void FT_ToDecimal_3 ()
		{
			Tnum t = new Tnum();
			t.AddState(Time.DawnOf, 42.13);
			t.AddState(new DateTime(1912,5,3), 43.13);
			Assert.AreEqual(null, t.ToDecimal);		
		}
		
		// Tstr.ToString
		
		[Test]
		public void FT_ToString_1 ()
		{
			Tstr t = new Tstr("42");
			Assert.AreEqual("42", t.ToString);		
		}
		
		[Test]
		public void FT_ToString_2 ()
		{
			Tstr t = new Tstr();
			Assert.AreEqual(null, t.ToString);		
		}
		
		[Test]
		public void FT_ToString_3 ()
		{
			Tstr t = new Tstr();
			t.AddState(Time.DawnOf, "42");
			t.AddState(new DateTime(2004,2,21), "43");
			Assert.AreEqual(null, t.ToString);		
		}
		
		// Tdate.ToDateTime
		
		[Test]
		public void FT_ToDateTime_1 ()
		{
			Tdate t = new Tdate(2000,1,1);
			Assert.AreEqual(new DateTime(2000,1,1), t.ToDateTime);		
		}
		
		[Test]
		public void FT_ToDateTime_2 ()
		{
			Tdate t = new Tdate();
			Assert.AreEqual(null, t.ToDateTime);		
		}
		
		[Test]
		public void FT_ToDateTime_3 ()
		{
			Tdate t = new Tdate();
			t.AddState(Time.DawnOf, new DateTime(1000,1,1));
			t.AddState(new DateTime(1000,1,1), new DateTime(2000,1,1));
			Assert.AreEqual(null, t.ToDateTime);		
		}
        
        // Tvar.ElapsedTime
        
        [Test]
        public void FT_ElapsedTime_1 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,1,2), false);
            t.AddState(new DateTime(2000,1,3), true);
            t.AddState(new DateTime(2000,1,4), false);      
            Tnum result = t.ElapsedDays(new DateTime(1999,1,1), new DateTime(2000,1,6));
            Assert.AreEqual("1/1/0001 12:00:00 AM 2 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_2 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,1,2), false);
            t.AddState(new DateTime(2000,1,3), true);
            t.AddState(new DateTime(2000,1,4), false);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,2), new DateTime(2000,1,6));
            Assert.AreEqual("1/1/0001 12:00:00 AM 1 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedTime_3 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,1,2), false);
            t.AddState(new DateTime(2000,1,3), true);
            t.AddState(new DateTime(2000,1,4), false);
            Tnum result = t.ElapsedDays(new DateTime(2010,1,2), new DateTime(2010,1,6));
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedTime_4 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,1,2), false);
            Tnum result = t.ElapsedDays(new DateTime(1999,1,2), new DateTime(1999,1,6));
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);
        }
		
        [Test]
        public void FT_ElapsedTime_5 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,2,1), false);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,15), new DateTime(2000,1,20));
            Assert.AreEqual("1/1/0001 12:00:00 AM 5 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_6 ()
        {
            Tbool t = new Tbool(true);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,15), new DateTime(2000,1,20));
            Assert.AreEqual("1/1/0001 12:00:00 AM 5 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_7 ()
        {
            Tbool t = new Tbool(true);
            Tnum result = t.ElapsedDays(false, new DateTime(2000,1,15), new DateTime(2000,1,20));
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_8 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,15), new DateTime(2000,1,20));
            Assert.AreEqual("1/1/0001 12:00:00 AM 5 ", result.TestOutput);
        }
		
        [Test]
        public void FT_ElapsedTime_9 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2000,1,5), false);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,2), new DateTime(2000,1,6));
            Assert.AreEqual("1/1/0001 12:00:00 AM 3 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_10 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.ElapsedDays(new DateTime(2000,1,1), new DateTime(2010,1,1));
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_11 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.ElapsedDays(Time.DawnOf, Time.EndOf);
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_12 ()
        {
            Tbool t = new Tbool();
            Tnum result = t.ElapsedDays(Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_HowToHandleEternals ()
        {
            Tbool t = new Tbool(true);
            Tnum result = t.ElapsedDays(true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 3652058 ", result.TestOutput);   // Is this the desired result?
        }
        
        // Tvar.ElapsedDaysPerInterval
        
        [Test]
        public void FT_ElapsedDaysPerInterval_1 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,1,1), true);
            t.AddState(new DateTime(2001,1,1), false);
            t.AddState(new DateTime(2002,1,1), true);
            t.AddState(new DateTime(2003,1,1), false);
            Tnum result = t.ElapsedDaysPer(TheTime.TheYear, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 1/1/2000 12:00:00 AM 366 1/1/2001 12:00:00 AM 0 1/1/2002 12:00:00 AM 365 1/1/2003 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedDaysPerInterval_2 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2000,6,1), true);
            t.AddState(new DateTime(2001,1,1), false);
            Tnum result = t.ElapsedDaysPer(TheTime.TheYear, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 1/1/2000 12:00:00 AM 214 1/1/2001 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedDaysPerInterval_3 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.ElapsedDaysPer(TheTime.TheYear, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        // Tbool.CountPer
        
        [Test]
        public void FT_CountPer_1 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,1,1), true);
            t.AddState(new DateTime(2010,2,1), true);
            t.AddState(new DateTime(2010,3,1), true);
            t.AddState(new DateTime(2010,4,1), false);
            Tnum actual = t.CountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 1/1/2010 12:00:00 AM 3 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_2 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            Tnum actual = t.CountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_3 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,1,1), true);
            t.AddState(new DateTime(2010,2,1), false);
            t.AddState(new DateTime(2010,3,1), true);
            t.AddState(new DateTime(2010,4,1), false);
            Tnum actual = t.CountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 1/1/2010 12:00:00 AM 2 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_4 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,11,1), true);
            t.AddState(new DateTime(2010,12,1), true);
            t.AddState(new DateTime(2011,1,1), true);
            t.AddState(new DateTime(2011,2,1), false);
            Tnum actual = t.CountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 1/1/2010 12:00:00 AM 2 1/1/2011 12:00:00 AM 1 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_5 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2009,12,15), true);
            t.AddState(new DateTime(2010,2,1), false);
            t.AddState(new DateTime(2010,3,1), true);
            t.AddState(new DateTime(2010,4,1), false);
            Tnum actual = t.CountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 1/1/2010 12:00:00 AM 1 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.RunningCountPer
        
        [Test]
        public void FT_RunningCountPer_1 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,1,1), true);
            t.AddState(new DateTime(2010,2,1), true);
            t.AddState(new DateTime(2010,3,1), true);
            t.AddState(new DateTime(2010,4,1), false);
            Tnum actual = t.RunningCountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 2/1/2010 12:00:00 AM 1 3/1/2010 12:00:00 AM 2 4/1/2010 12:00:00 AM 3 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_RunningCountPer_2 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            Tnum actual = t.RunningCountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_RunningCountPer_3 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,1,1), true);
            t.AddState(new DateTime(2010,2,1), false);
            t.AddState(new DateTime(2010,3,1), true);
            t.AddState(new DateTime(2010,4,1), false);
            Tnum actual = t.RunningCountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 2/1/2010 12:00:00 AM 1 4/1/2010 12:00:00 AM 2 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_RunningCountPer_4 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(new DateTime(2010,11,1), true);
            t.AddState(new DateTime(2010,12,1), true);
            t.AddState(new DateTime(2011,1,1), true);
            t.AddState(new DateTime(2011,2,1), false);
            Tnum actual = t.RunningCountPer(TheTime.TheYear);
            string expected = "1/1/0001 12:00:00 AM 0 12/1/2010 12:00:00 AM 1 1/1/2011 12:00:00 AM 0 2/1/2011 12:00:00 AM 1 3/1/2011 12:00:00 AM 2 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.CountPastNIntervals
        
        [Test]
        public void FT_CountPastNIntervals_1 ()
        {
            // This test case will break in 2012 due to the use of TheTime.Year
            Tbool t = new Tbool(true);
            t.AddState(new DateTime(2011,1,1), false);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "1/1/0001 12:00:00 AM 1 1/1/2009 12:00:00 AM 2 1/1/2011 12:00:00 AM 1 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPastNIntervals_2 ()
        {
            // This test case will break in 2012 due to the use of TheTime.Year
            Tbool t = new Tbool(true);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "1/1/0001 12:00:00 AM 1 1/1/2009 12:00:00 AM 2 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPastNIntervals_3 ()
        {
            Tbool t = new Tbool(false);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "1/1/0001 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.ForConsecutiveMonths
        
        [Test]
        public void ForConsecutiveMonths_1 ()
        {
            Tbool t = new Tbool(false);
            Tbool r = t.ForConsecutiveMonths(12);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_2 ()
        {
            Tbool t = new Tbool(true);
            Tbool r = t.ForConsecutiveMonths(12);
            Assert.AreEqual("1/1/0001 12:00:00 AM False 1/1/0002 12:00:00 AM True ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(new DateTime(2000,1,1),true);
            t.AddState(new DateTime(2001,1,1),false);
            Tbool r = t.ForConsecutiveMonths(6);
            Assert.AreEqual("1/1/0001 12:00:00 AM False 7/1/2000 12:00:00 AM True 1/1/2001 12:00:00 AM False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_4 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(new DateTime(2000,1,1),true);
            t.AddState(new DateTime(2001,1,1),false);
            Tbool r = t.ForConsecutiveMonths(18);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_5 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(new DateTime(2000,1,1),true);
            t.AddState(new DateTime(2001,1,1),false);
            t.AddState(new DateTime(2001,6,1),true);
            Tbool r = t.ForConsecutiveMonths(20);
            Assert.AreEqual("1/1/0001 12:00:00 AM False 2/1/2003 12:00:00 AM True ", r.TestOutput);      
        }
        
        // Tbool.DateFirstTrue
        
        [Test]
        public void DateFirstTrue_1 ()
        {
            Tbool t = new Tbool(false);
            Assert.AreEqual(Time.EndOf, t.DateFirstTrue);      
        }
        
        [Test]
        public void DateFirstTrue_2 ()
        {
            Tbool t = new Tbool(true);
            Assert.AreEqual(Time.DawnOf, t.DateFirstTrue);      
        }
        
        [Test]
        public void DateFirstTrue_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(new DateTime(2000,1,1), true);
            Assert.AreEqual(new DateTime(2000,1,1), t.DateFirstTrue);      
        }
        
	}
	
}