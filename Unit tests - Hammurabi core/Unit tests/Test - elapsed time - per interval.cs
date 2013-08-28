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

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public partial class ElapsedTime : H
    {   
        // ElapsedDaysPerInterval

        [Test]
        public void ElapsedDaysPerInterval_1 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2001,1,1), false);
            t.AddState(Date(2002,1,1), true);
            t.AddState(Date(2003,1,1), false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("{Dawn: 0; 1/1/2000: 366; 1/1/2001: 0; 1/1/2002: 365; 1/1/2003: 0}", result.Out);      
        }

        [Test]
        public void ElapsedDaysPerInterval_2 ()
        {
            Tbool t = new Tbool(false);
            t.AddState(Date(2000,6,1), true);
            t.AddState(Date(2001,1,1), false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("{Dawn: 0; 1/1/2000: 214; 1/1/2001: 0}", result.Out);      
        }

        [Test]
        public void ElapsedDaysPerInterval_3 ()
        {
            Tbool t = new Tbool(false);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual(0, result.Out);      
        }

        [Test]
        public void ElapsedDaysPerInterval_4 ()
        {
            Tbool t = new Tbool(Hstate.Unstated);
            Tnum result = t.TotalElapsedDaysPer(TheYear);
            Assert.AreEqual("Unstated", result.Out);      
        }

        [Test]
        public void ElapsedDaysPerInterval_5 ()
        {
            Tbool t = new Tbool(false);
            Tnum n = new Tnum(Hstate.Unstated);
            Tnum result = t.TotalElapsedDaysPer(n);
            Assert.AreEqual("Unstated", result.Out);      
        }
    }
}
