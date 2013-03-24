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
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2011,01,01));
            Assert.AreEqual(Date(2011,1,1), td.Lean.Out);        
        }
        
        // .AsOf
        
        [Test]
        public void Tdate_AsOf_1 ()
        {
            Tdate td = new Tdate();
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2012,01,01));
            Assert.AreEqual(Date(2012,1,1), td.AsOf(Time.DawnOf.AddYears(3)).Out);        
        }
        
        [Test]
        public void Tdate_AsOf_2 ()
        {
            Tdate td = new Tdate();
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2012,01,01));
            Assert.AreEqual(Date(2011,1,1), td.AsOf(Time.DawnOf.AddYears(1)).Out);        
        }
                
        // Equals
        
        [Test]
        public void Tdate_Equals_1 ()
        {
            Tdate td1 = new Tdate(2010,5,13);

            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(2011,01,01));
            td2.AddState(Date(2000,1,1), Date(2010,5,13));

            Tbool result = td1 == td2;
            Assert.AreEqual("{Dawn: false; 1/1/2000: true}", result.Out);        
        }
        
        [Test]
        public void Tdate_Equals_2 ()
        {
            Tdate td1 = new Tdate(2010,5,13);
            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(2011,01,01));
            td2.AddState(Date(2000,1,1), Date(2010,5,13));
            Tbool result = td1 != td2;
            Assert.AreEqual("{Dawn: true; 1/1/2000: false}", result.Out);        
        }
        
        // IsBefore / IsAfter
        
        [Test]
        public void Tdate_IsAfter_1 ()
        {
            Tdate td1 = new Tdate(2010,1,1);
            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2011,1,1));
            Tbool result = td1 > td2;
            Assert.AreEqual("{Dawn: true; 1/1/2000: false}", result.Out);        
        }
        
        [Test]
        public void Tdate_IsBefore_1 ()
        {
            Tdate td1 = new Tdate(2010,1,1);
            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2011,1,1));
            Tbool result = td1 < td2;
            Assert.AreEqual("{Dawn: false; 1/1/2000: true}", result.Out);        
        }
        
        [Test]
        public void Tdate_IsAtOrAfter_1 ()
        {
            Tdate td1 = new Tdate(2008,1,1);
            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2008,1,1));
            Tbool result = td1 >= td2;
            Assert.AreEqual("{Dawn: false; 1/1/2000: true}", result.Out);        
        }
        
        [Test]
        public void Tdate_IsAtOrBefore_1 ()
        {
            Tdate td1 = new Tdate(2000,1,1);
            Tdate td2 = new Tdate();
            td2.AddState(Time.DawnOf, Date(1999,1,1));
            td2.AddState(Date(2000,1,1), Date(2000,1,1));
            td2.AddState(Date(2001,1,1), Date(2008,1,1));
            Tbool result = td2 <= td1;
            Assert.AreEqual("{Dawn: true; 1/1/2001: false}", result.Out);        
        }
        
        // .AddDays
        
        [Test]
        public void Tdate_AddDays_1 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddDays(3);
            Assert.AreEqual(Date(2000,1,4), result.Out);        
        }
        
        [Test]
        public void Tdate_AddDays_2 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddDays(-3);
            Assert.AreEqual(Date(1999,12,29), result.Out);        
        }
        
        // .AddMonths
        
        [Test]
        public void Tdate_AddMonths_1 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddMonths(3);
            Assert.AreEqual(Date(2000,4,1), result.Out);        
        }
        
        [Test]
        public void Tdate_AddMonths_2 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddMonths(-3);
            Assert.AreEqual(Date(1999,10,1), result.Out);        
        }
        
        // .AddYears
        
        [Test]
        public void Tdate_AddYears_1 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddYears(3);
            Assert.AreEqual(Date(2003,1,1), result.Out);        
        }
        
        [Test]
        public void Tdate_AddYears_2 ()
        {
            Tdate td = new Tdate(2000,1,1);
            Tdate result = td.AddYears(-3);
            Assert.AreEqual(Date(1997,1,1), result.Out);        
        }

        // DayDiff
        // When the earlier date is put after the prior date, DayDiff returns a negative number.
        
        [Test]
        public void Tdate_DayDiff_2 ()
        {
            Tdate td1 = new Tdate(2010,1,1);
            Tdate td2 = new Tdate(2000,1,1);
            Tnum result = Tdate.DayDiff(td1,td2);
            Assert.AreEqual(-3653, result.Out);        
        }
    }
}   