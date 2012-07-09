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
    public class FundamentalTypes : H
    {
        // .Lean
        
        [Test]
        public void FT_Lean_1 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), true);
            Tbool res = t.Lean;
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        // .AsOf
        
        [Test]
        public void FT_AsOf_1 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_2 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_3 ()
        {
            Tbool t = new Tbool(true);
            Tbool res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_4 ()
        {
            Tnum t = new Tnum(4);
            t.AddState(Time.DawnOf.AddYears(5), 44);
            Tnum res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("Time.DawnOf 4 ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_5 ()
        {
            Tnum t = new Tnum(4);
            t.AddState(Time.DawnOf.AddYears(5), 44);
            Tnum res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual("Time.DawnOf 44 ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_6 ()
        {
            Tstr t = new Tstr("ham");
            t.AddState(Time.DawnOf.AddYears(5), "sam");
            Tstr res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual("Time.DawnOf sam ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_7 ()
        {
            Tstr t = new Tstr("ham");
            t.AddState(Time.DawnOf.AddYears(5), "sam");
            Tstr res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("Time.DawnOf ham ", res.TestOutput);        
        }
        
        [Test]
        public void FT_AsOf_8 ()
        {
            Tbool res = new Tbool(Hstate.Uncertain).AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("Time.DawnOf Uncertain ", res.TestOutput);       
        }

        [Test]
        public void FT_AsOf_9 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(new DateTime(2000,1,1), false);

            Tdate time = new Tdate(1999,1,1);

            Assert.AreEqual("Time.DawnOf True ", t.AsOf(time).TestOutput);        
        }

        [Test]
        public void FT_AsOf_10 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);

            Tdate time = new Tdate(Hstate.Stub);

            Assert.AreEqual("Time.DawnOf Stub ", t.AsOf(time).TestOutput);        
        }

        [Test]
        public void FT_AsOf_11 ()
        {
            Tbool t = new Tbool(Hstate.Stub);
            Tdate time = new Tdate(Hstate.Unstated);
            Assert.AreEqual("Time.DawnOf Stub ", t.AsOf(time).TestOutput);        
        }

        [Test]
        public void FT_AsOf_12 ()
        {
            // Tdate varies, but base Tvar is eternal, so .AsOf should return that eternal value
            Tbool t = new Tbool(true);
            Tdate time = new Tdate(Date(2000,1,1));
            time.AddState(Date(2010,1,1),Date(2010,1,1));
            Assert.AreEqual("Time.DawnOf True ", t.AsOf(time).TestOutput);        
        }

        [Test]
        public void FT_AsOf_13 ()
        {
            // Tdate unknown, but base Tvar is eternal, so .AsOf should return that eternal value
            Tbool t = new Tbool(true);
            Tdate time = new Tdate(Hstate.Stub);
            Assert.AreEqual("Time.DawnOf True ", t.AsOf(time).TestOutput);        
        }

        [Test]
        public void FT_AsOf_14 ()
        {
            // Both Tdate and Tvar vary
            Tbool t = new Tbool(true);
            t.AddState(Date(2000,1,1),false);

            // When Tdate varies, the FirstValue is used...
            Tdate time = new Tdate(Date(1999,1,1));
            time.AddState(Date(2010,1,1),Date(2010,1,1));

            Assert.AreEqual("Time.DawnOf True ", t.AsOf(time).TestOutput);        
        }

        // .IsAlways
        
        [Test]
        public void FT_IsAlways_1 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.IsAlwaysTrue();
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_2 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = (!t).IsAlwaysTrue();
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_3 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            Tbool res = t.IsAlwaysTrue();
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_4 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.IsAlwaysTrue(Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_5 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.IsAlwaysTrue(Time.DawnOf.AddYears(2), Time.DawnOf.AddYears(3));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_6 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_7 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = (!t).IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_8 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            Tbool res = t.IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_9 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            Tbool res = (!t).IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_10 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, true);
            Tbool res = (!t).IsAlwaysTrue();
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsAlways_11 ()
        {
            Tbool t = new Tnum(3.4) == 3.4;
            Tbool res = t.IsAlwaysTrue();
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);       
        }

        // .IsEver
        
        [Test]
        public void FT_IsEver_1 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tbool res = t.IsEverTrue();
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);        
        }
        
        [Test]
        public void FT_IsEver_2 ()
        {
            Tbool t = new Tbool(false);
            Tbool res = t.IsEverTrue();
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);        
        }

        [Test]
        public void FT_IsEver_8 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2012,11,8), true);
            Tbool res = t.IsEverTrue();
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);            
        }
        
        [Test]
        public void FT_IsEver_9 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2012,11,8), true);
            Tbool res = t.IsEverTrue(Date(2013,1,1), Date(2014,1,1));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);            
        }
        
        [Test]
        public void FT_IsEver_10 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2012,11,8), true);
            Tbool res = t.IsEverTrue(Date(2012,1,1), Date(2013,1,1));
            Assert.AreEqual("Time.DawnOf True ", res.TestOutput);            
        }
        
        [Test]
        public void FT_IsEver_11 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2012,11,8), true);
            Tbool res = t.IsEverTrue(Date(2011,1,1), Date(2012,1,1));
            Assert.AreEqual("Time.DawnOf False ", res.TestOutput);            
        }

        [Test]
        public void FT_IsEver_13 ()
        {
            Tbool t = new Tbool(Hstate.Unstated);
            Tbool res = t.IsEverTrue(Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Time.DawnOf Unstated ", res.TestOutput);        
        }

        [Test]
        public void FT_IsEver_14 ()
        {
            Tbool t = new Tbool(Hstate.Unstated);
            Tbool res = t.IsEverTrue();
            Assert.AreEqual("Time.DawnOf Unstated ", res.TestOutput);        
        }

        [Test]
        public void FT_IsEver_15 ()
        {
            Tbool t = new Tbool(Hstate.Stub);
            Tbool res = t.IsEverTrue();
            Assert.AreEqual("Time.DawnOf Stub ", res.TestOutput);        
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
            Tbool t = new Tbool(false);
            t.AddState(Time.DawnOf.AddYears(5), true);
            Assert.AreEqual(null, t.ToBool);        
        }
        
        [Test]
        public void FT_ToBool_4 ()
        {
            // If value is unknown, .ToBool should return null
            Tbool t = new Tbool(Hstate.Uncertain);
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
            t.AddState(Date(2012,11,8), true);
            
            Tbool result = t.EverPer(theYear).Lean;
            Assert.AreEqual("Time.DawnOf False 1/1/2012 12:00:00 AM True ", result.TestOutput);        
        }
        
        [Test]
        public void FT_EverPerInterval_2 ()
        {
            Tnum theYear = TheTime.Year(5);
            Tbool t = new Tbool(Hstate.Unstated);
            Tbool result = t.EverPer(theYear);
            Assert.AreEqual("Time.DawnOf Unstated ", result.Lean.TestOutput);        
        }

        [Test]
        public void FT_EverPerInterval_3 ()
        {
            Tnum theYear = new Tnum(Hstate.Stub);
            Tbool t = new Tbool(Hstate.Unstated);
            Tbool result = t.EverPer(theYear);
            Assert.AreEqual("Time.DawnOf Stub ", result.Lean.TestOutput);        
        }

        
        // .AlwaysPerInterval
        
        [Test]
        public void FT_AlwaysPerInterval_1 ()
        {
            // This will break annually b/c Year is determined by the system clock
            Tnum theYear = TheTime.Year(5);
            
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf, false);
            t.AddState(Date(2012,11,8), true);
            
            Tbool result = t.AlwaysPer(theYear).Lean;
            Assert.AreEqual("Time.DawnOf False 1/1/2013 12:00:00 AM True ", result.TestOutput);        
        }
        
        [Test]
        public void FT_AlwaysPerInterval_2 ()
        {
            Tnum theYear = TheTime.Year(5);
            Tbool result = new Tbool(Hstate.Stub).AlwaysPer(theYear);
            Assert.AreEqual("Time.DawnOf Stub ", result.Lean.TestOutput);        
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
            Tnum t = new Tnum(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToInt);        
        }
        
        [Test]
        public void FT_ToInt_3 ()
        {
            Tnum t = new Tnum(42);
            t.AddState(Date(1912,5,3), 43);
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
            Tnum t = new Tnum(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToDecimal);        
        }
        
        [Test]
        public void FT_ToDecimal_3 ()
        {
            Tnum t = new Tnum(42.13);
            t.AddState(Date(1912,5,3), 43.13);
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
            Tstr t = new Tstr(Hstate.Stub);
            Assert.AreEqual(null, t.ToString);        
        }
        
        [Test]
        public void FT_ToString_3 ()
        {
            Tstr t = new Tstr("42");
            t.AddState(Date(2004,2,21), "43");
            Assert.AreEqual(null, t.ToString);        
        }
        
        // Tdate.ToDateTime
        
        [Test]
        public void FT_ToDateTime_1 ()
        {
            Tdate t = new Tdate(2000,1,1);
            Assert.AreEqual(Date(2000,1,1), t.ToNullDateTime);        
        }
        
        [Test]
        public void FT_ToDateTime_2 ()
        {
            Tdate t = new Tdate(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToNullDateTime);        
        }
        
        [Test]
        public void FT_ToDateTime_3 ()
        {
            Tdate t = new Tdate(Date(1000,1,1));
            t.AddState(Date(1000,1,1), Date(2000,1,1));
            Assert.AreEqual(null, t.ToNullDateTime);        
        }
        
        // Tvar.ElapsedTime
        
        [Test]
        public void FT_ElapsedTime_1 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);      
            Tnum result = t.TotalElapsedDays(Date(1999,1,1), Date(2000,1,6));
            Assert.AreEqual("Time.DawnOf 2 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_2 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);
            Tnum result = t.TotalElapsedDays(Date(2000,1,2), Date(2000,1,6));
            Assert.AreEqual("Time.DawnOf 1 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedTime_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);
            Tnum result = t.TotalElapsedDays(Date(2010,1,2), Date(2010,1,6));
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedTime_4 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            Tnum result = t.TotalElapsedDays(Date(1999,1,2), Date(1999,1,6));
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_5 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,2,1), false);
            Tnum result = t.TotalElapsedDays(Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual("Time.DawnOf 5 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_6 ()
        {
            Tbool t = new Tbool(true);
            Tnum result = t.TotalElapsedDays(Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual("Time.DawnOf 5 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_7 ()
        {
            Tbool t = new Tbool(true);
            Tnum result = (!t).TotalElapsedDays(Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_8 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            Tnum result = t.TotalElapsedDays(Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual("Time.DawnOf 5 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_9 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,5), false);
            Tnum result = t.TotalElapsedDays(Date(2000,1,2), Date(2000,1,6));
            Assert.AreEqual("Time.DawnOf 3 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_10 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.TotalElapsedDays(Date(2000,1,1), Date(2010,1,1));
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_11 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.TotalElapsedDays(Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);
        }
        
        [Test]
        public void FT_ElapsedTime_12 ()
        {
            Tbool t = new Tbool(Hstate.Uncertain);
            Tnum result = t.TotalElapsedDays(Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Time.DawnOf Uncertain ", result.TestOutput);
        }

        // Tvar.ElapsedDaysPerInterval
        
        [Test]
        public void FT_ElapsedDaysPerInterval_1 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2001,1,1), false);
            t.AddState(Date(2002,1,1), true);
            t.AddState(Date(2003,1,1), false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("Time.DawnOf 0 1/1/2000 12:00:00 AM 366 1/1/2001 12:00:00 AM 0 1/1/2002 12:00:00 AM 365 1/1/2003 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedDaysPerInterval_2 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,6,1), true);
            t.AddState(Date(2001,1,1), false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("Time.DawnOf 0 1/1/2000 12:00:00 AM 214 1/1/2001 12:00:00 AM 0 ", result.TestOutput);      
        }
        
        [Test]
        public void FT_ElapsedDaysPerInterval_3 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("Time.DawnOf 0 ", result.TestOutput);      
        }

        [Test]
        public void FT_ElapsedDaysPerInterval_4 ()
        {
            Tbool t = new Tbool(Hstate.Unstated);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("Time.DawnOf Unstated ", result.TestOutput);      
        }

        [Test]
        public void FT_ElapsedDaysPerInterval_5 ()
        {
            Tbool t = new Tbool(false);
            Tnum n = new Tnum(Hstate.Unstated);
            Tnum result = t.TotalElapsedDaysPer(n);
            Assert.AreEqual("Time.DawnOf Unstated ", result.TestOutput);      
        }

        // Tbool.CountPer
        
        [Test]
        public void FT_CountPer_1 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), true);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tnum actual = t.CountPer(TheYear);
            string expected = "Time.DawnOf 0 1/1/2010 12:00:00 AM 3 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_2 ()
        {
            Tbool t = new Tbool(false);
            Tnum actual = t.CountPer(TheYear);
            string expected = "Time.DawnOf 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tnum actual = t.CountPer(TheYear);
            string expected = "Time.DawnOf 0 1/1/2010 12:00:00 AM 2 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_4 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2010,11,1), true);
            t.AddState(Date(2010,12,1), true);
            t.AddState(Date(2011,1,1), true);
            t.AddState(Date(2011,2,1), false);
            Tnum actual = t.CountPer(TheYear);
            string expected = "Time.DawnOf 0 1/1/2010 12:00:00 AM 2 1/1/2011 12:00:00 AM 1 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPer_5 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2009,12,15), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tnum actual = t.CountPer(TheYear);
            string expected = "Time.DawnOf 0 1/1/2010 12:00:00 AM 1 1/1/2011 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.RunningCountPer
        
        [Test]
        public void FT_RunningCountPer_1 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), true);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tnum actual = t.RunningCountPer(TheYear);
            string expected = "Time.DawnOf 0 2/1/2010 12:00:00 AM 1 3/1/2010 12:00:00 AM 2 4/1/2010 12:00:00 AM 3 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_RunningCountPer_2 ()
        {
            Tbool t = new Tbool(false);
            Tnum actual = t.RunningCountPer(TheYear);
            string expected = "Time.DawnOf 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_RunningCountPer_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tnum actual = t.RunningCountPer(TheYear);
            string expected = "Time.DawnOf 0 2/1/2010 12:00:00 AM 1 4/1/2010 12:00:00 AM 2 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.CountPastNIntervals
        
        [Test]
        public void FT_CountPastNIntervals_1 ()
        {
            // This test case will break in 2012 due to the use of TheTime.Year
            Tbool t = new Tbool(true);
            t.AddState(Date(2011,1,1), false);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "Time.DawnOf 1 1/1/2010 12:00:00 AM 2 1/1/2011 12:00:00 AM 1 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPastNIntervals_2 ()
        {
            // This test case will break in 2012 due to the use of TheTime.Year
            Tbool t = new Tbool(true);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "Time.DawnOf 1 1/1/2010 12:00:00 AM 2 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        [Test]
        public void FT_CountPastNIntervals_3 ()
        {
            Tbool t = new Tbool(false);
            Tnum actual = t.CountPastNIntervals(TheTime.Year(2), 2);
            string expected = "Time.DawnOf 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
        
        // Tbool.ForConsecutiveMonths
        
        [Test]
        public void ForConsecutiveMonths_1 ()
        {
            Tbool t = new Tbool(false);
            Tbool r = t.ConsecutiveMonths(12);
            Assert.AreEqual("Time.DawnOf False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_2 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(new DateTime(2000,1,1), true);

            Tbool r = t.ConsecutiveMonths(12);
            Assert.AreEqual("Time.DawnOf False 1/1/2001 12:00:00 AM True ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecMonths_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1),true);
            t.AddState(Date(2001,1,1),false);
            Tbool r = t.ConsecutiveMonths(6);
            Assert.AreEqual("Time.DawnOf False 7/1/2000 12:00:00 AM True 1/1/2001 12:00:00 AM False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_4 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1),true);
            t.AddState(Date(2001,1,1),false);
            Tbool r = t.ConsecutiveMonths(18);
            Assert.AreEqual("Time.DawnOf False ", r.TestOutput);      
        }
        
        [Test]
        public void ForConsecutiveMonths_5 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1),true);
            t.AddState(Date(2001,1,1),false);
            t.AddState(Date(2001,6,1),true);
            Tbool r = t.ConsecutiveMonths(20);
            Assert.AreEqual("Time.DawnOf False 2/1/2003 12:00:00 AM True ", r.TestOutput);      
        }

        [Test]
        public void ForConsecutiveMonths_6 ()
        {
            Tbool t = new Tbool(Hstate.Uncertain);
            Tbool r = t.ConsecutiveMonths(20);
            Assert.AreEqual("Time.DawnOf Uncertain ", r.TestOutput);      
        }

        // Tbool.DateFirst

        [Test]
        public void DateFirst_1 ()
        {
            // Base Tvar never meets the specified condition
            Tbool t = new Tbool(false);
            Assert.AreEqual("Time.DawnOf Stub ", t.DateFirstTrue.TestOutput);      
        }
        
        [Test]
        public void DateFirst_2 ()
        {
            Tbool t = new Tbool(true);
            Assert.AreEqual(Time.DawnOf, t.DateFirstTrue.ToDateTime);   
        }
        
        [Test]
        public void DateFirst_3 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Date(2000,1,1), t.DateFirstTrue.ToDateTime);      
        }

        [Test]
        public void DateFirst_4 ()
        {
            // Base Tvar is eternally unknown; that state must percolate up
            Tbool t = new Tbool(Hstate.Unstated);
            Assert.AreEqual("Time.DawnOf Unstated ", t.DateFirstTrue.TestOutput);      
        }

        [Test]
        public void DateFirst_5 ()
        {
            // Base Tvar is unknown, then the required value
            Tbool t = new Tbool(Hstate.Unstated);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Date(2000,1,1), t.DateFirstTrue.ToDateTime);      
        }

        // Type identification
        
        [Test]
        public void TnumTypeTest1 ()
        {
            bool isTnum = new Tnum().GetType().ToString() == "Hammurabi.Tnum";
            Assert.AreEqual(true, isTnum);        
        }
        
        [Test]
        public void TnumTypeTest2 ()
        {
            bool isTnum = new Tbool().GetType().ToString() == "Hammurabi.Tnum";
            Assert.AreEqual(false, isTnum);        
        }
        
        [Test]
        public void TvarTypeTest1 ()
        {
            Tnum item = new Tnum(3);
            bool istype = Auxiliary.IsType<Tnum>(item);
            Assert.AreEqual(true, istype);        
        }
        
        [Test]
        public void TvarTypeTest2 ()
        {
            Tnum item = new Tnum(3);
            bool istype = Auxiliary.IsType<Tbool>(item);
            Assert.AreEqual(false, istype);        
        }

        // ObjectAsOf

        [Test]
        public void ObjectAsOf1 ()
        {
            Tnum item = new Tnum(3);
            Hval h = item.ObjectAsOf(DateTime.Now);
            Assert.AreEqual(3, h.Val);        
        }

        [Test]
        public void ObjectAsOf2 ()
        {
            Tnum item = new Tnum(Hstate.Stub);
            Hval h = item.ObjectAsOf(DateTime.Now);
            Assert.AreEqual("Stub", h.ToString);        
        }

        // Value re-assignment

        [Test]
        public void ValueReassignment1 ()
        {
            Tnum t = new Tnum(3);
            t = new Tnum(4);
            Assert.AreEqual("Time.DawnOf 4 ", t.TestOutput);        
        }

        // .IsUnstated

        [Test]
        public void IsUnstated1 ()
        {
            Hval unst = new Hval(null,Hstate.Unstated);

            Tbool tb1 = new Tbool(false);
            tb1.AddState(Date(2000,1,1), unst);
            tb1.AddState(Date(2001,1,1), true);

            Assert.AreEqual("Time.DawnOf False 1/1/2000 12:00:00 AM True 1/1/2001 12:00:00 AM False ", tb1.IsUnstated.TestOutput);        
        }

    }    
}