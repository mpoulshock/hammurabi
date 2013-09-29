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

using System;
using Hammurabi;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class Utility : H
    {
        // NextChangeDate

        [Test]
        public void NextChangeDate1 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.NextChangeDate(new DateTime(2015, 1, 2));
            Assert.AreEqual(new DateTime(2015, 2, 1), d);    
        }

        [Test]
        public void NextChangeDate2 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.NextChangeDate(new DateTime(2010, 1, 1));
            Assert.AreEqual(new DateTime(2015, 1, 1), d);    
        }

        [Test]
        public void NextChangeDate3 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.NextChangeDate(Time.DawnOf);
            Assert.AreEqual(Time.DawnOf, d);    
        }

        [Test]
        public void NextChangeDate4 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.NextChangeDate(Time.EndOf);
            Assert.AreEqual(Time.EndOf, d);    
        }

        [Test]
        public void NextChangeDate5 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.NextChangeDate(new DateTime(2015, 8, 15));
            Assert.AreEqual(new DateTime(2015, 9, 1), d);    
        }

        // DateNextTrue

        [Test]
        public void DateNextTrue1 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.DateNextTrue(Time.DawnOf);
            Assert.AreEqual(new DateTime(2015, 1, 1), d);    
        }

        [Test]
        public void DateNextTrue2 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.DateNextTrue(Date(2015,5,5));
            Assert.AreEqual(Time.EndOf, d);    
        }

        [Test]
        public void DateNextTrue3 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,1),false);
            tb.AddState(new DateTime(2015,3,1),true);
            tb.AddState(new DateTime(2015,9,1),false);

            DateTime d = tb.DateNextTrue(Date(2015,2,15));
            Assert.AreEqual(new DateTime(2015,3,1), d);    
        }

        // Minimum

        [Test]
        public void Minimum_1()
        {
            Facts.Clear();
            Assert.AreEqual(99, Min(100, 99).Out);
        }

        [Test]
        public void Minimum_2()
        {
            Facts.Clear();
            Assert.AreEqual(0, Min(100, 0).Out);
        }

        [Test]
        public void Minimum_3()
        {
            Facts.Clear();
            Assert.AreEqual(-100, Min(100, -100).Out);
        }

        [Test]
        public void Minimum_4()
        {
            Facts.Clear();
            Assert.AreEqual("Unstated", Min(100, new Tnum(Hstate.Unstated)).Out);
        }

        [Test]
        public void Minimum_5()
        {
            Facts.Clear();
            Assert.AreEqual("Uncertain", Min(100, new Tnum(Hstate.Uncertain)).Out);
        }

        [Test]
        public void Minimum_6()
        {
            Facts.Clear();
            Assert.AreEqual("Uncertain", Min(new Tnum(Hstate.Unstated), new Tnum(Hstate.Uncertain)).Out);
        }

        [Test]
        public void Minimum_7()
        {
            Facts.Clear();
            Assert.AreEqual("Stub", Min(0, new Tnum(Hstate.Stub)).Out);
        }

        [Test]
        public void Minimum_8()
        {
            Facts.Clear();
            Assert.AreEqual("Stub", Min(new Tnum(Hstate.Unstated), new Tnum(Hstate.Uncertain), new Tnum(Hstate.Stub)).Out);
        }

        [Test]
        public void Minimum_9()
        {
            Facts.Clear();
            Assert.AreEqual(1, Min(1, 2).Out);
        }

        // Maximum

        [Test]
        public void Maximum_1()
        {
            Facts.Clear();
            Assert.AreEqual(100, Max(100, 99).Out);
        }

        [Test]
        public void Maximum_2()
        {
            Facts.Clear();
            Assert.AreEqual(100, Max(100, 0).Out);
        }

        [Test]
        public void Maximum_3()
        {
            Facts.Clear();
            Assert.AreEqual(100, Max(100, -100).Out);
        }

        [Test]
        public void Maximum_4()
        {
            Facts.Clear();
            Assert.AreEqual("Unstated", Max(100, new Tnum(Hstate.Unstated)).Out);
        }

        [Test]
        public void Maximum_5()
        {
            Facts.Clear();
            Assert.AreEqual("Uncertain", Max(100, new Tnum(Hstate.Uncertain)).Out);
        }

        [Test]
        public void Maximum_6()
        {
            Facts.Clear();
            Assert.AreEqual("Uncertain", Max(new Tnum(Hstate.Unstated), new Tnum(Hstate.Uncertain)).Out);
        }

        [Test]
        public void Maximum_7()
        {
            Facts.Clear();
            Assert.AreEqual("Stub", Max(0, new Tnum(Hstate.Stub)).Out);
        }

        [Test]
        public void Maximum_8()
        {
            Facts.Clear();
            Assert.AreEqual("Stub", Max(new Tnum(Hstate.Unstated), new Tnum(Hstate.Uncertain), new Tnum(Hstate.Stub)).Out);
        }

        [Test]
        public void Maximum_9()
        {
            Facts.Clear();
            Assert.AreEqual(2, Max(1, 2).Out);
        }
    }
}