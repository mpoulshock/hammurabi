// Copyright (c) 2013 Hammura.bi LLC
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
    public class SummedIntervals : H
    {   
        // RUNNING SUMMED INTERVALS

        [Test]
        public void RunningSummedIntervals_1()
        {
            Tnum t = new Tnum(0);
            t.AddState(Date(2010, 1, 1), 1000);
            t.AddState(Date(2010, 3, 1), 0);
            Tnum r = t.RunningSummedIntervals(TheMonth).AsOf(Date(2012,1,1));           
            Assert.AreEqual(2000, r.Out);    
        }

        [Test]
        public void RunningSummedIntervals_2()       // Fail: not sure what's happening here.  Decimal comparison issues?
        {
            Tnum t = new Tnum(0);
            Tnum r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual(0, r.Out);    
        }
        
        [Test]
        public void RunningSummedIntervals_3()
        {
            Tnum t = new Tnum(new Hval());
            Tnum r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual("Unstated", r.Out);    
        }
        
        [Test]
        public void RunningSummedIntervals_4()
        {
            Tnum t = new Tnum(Hstate.Stub);
            Tnum r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual("Stub", r.Out);    
        }

        // SLIDING SUMMED INTERVALS

        [Test]
        public void SlidingSummedIntervals_1()
        {
            Tnum t = new Tnum(1000);
            Tnum r = t.SlidingSummedIntervals(TheMonth, 2);           
            Assert.AreEqual(2000, r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_2()
        {
            Tnum t = new Tnum(new Hval());
            Tnum r = t.SlidingSummedIntervals(TheMonth, 2);            
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_3()
        {
            Tnum t = new Tnum(Hstate.Stub);
            Tnum r = t.SlidingSummedIntervals(TheMonth, 2);            
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_4()
        {
            Tnum t = new Tnum(1000);
            t.AddState(Date(2013, 1, 1), 2000);
            Tnum r = t.SlidingSummedIntervals(TheYear, 2);           
            Assert.AreEqual("{Dawn: 2000; 1/1/2012: 3000; 1/1/2013: 4000}", r.Out);    
        }

        // TOTAL SUMMED INTERVALS

        [Test]
        public void TotalSummedIntervals_1()
        {
            Tnum t = new Tnum(1000);
            Tnum r = t.TotalSummedIntervals(TheMonth, Date(2015,1,1), Date(2015,12,31));           
            Assert.AreEqual(12000, r.Out);    
        }
    }
}