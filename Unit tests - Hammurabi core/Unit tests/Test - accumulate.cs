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
    public class Accumulate : H
    {   
        // ACCUMULATED

        [Test]
        public void Accumulate1()
        {
            Tnum t = new Tnum(0);
            t.AddState(Date(2010, 1, 1), 1000);
            t.AddState(Date(2010, 3, 1), 0);
            Tnum r = t.Accumulated(TheMonth).AsOf(Date(2012,1,1));           
            Assert.AreEqual(2000, r.Out);    
        }

        [Test]
        public void Accumulate2()       // Fail: not sure what's happening here.  Decimal comparison issues?
        {
            Tnum t = new Tnum(0);
            Tnum r = t.Accumulated(TheMonth);           
            Assert.AreEqual(0, r.Out);    
        }
        
        [Test]
        public void Accumulate3()
        {
            Tnum t = new Tnum(new Hval());
            Tnum r = t.Accumulated(TheMonth);           
            Assert.AreEqual("Unstated", r.Out);    
        }
        
        [Test]
        public void Accumulate4()
        {
            Tnum t = new Tnum(Hstate.Stub);
            Tnum r = t.Accumulated(TheMonth);           
            Assert.AreEqual("Stub", r.Out);    
        }

        // ACCUMULATED OVER

        [Test]
        public void AccumulateOver1()
        {
            Tnum t = new Tnum(1000);
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf 2000 ", r.TestOutput);    
        }

        [Test]
        public void AccumulateOver2()
        {
            Tnum t = new Tnum(new Hval());
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf Unstated ", r.TestOutput);    
        }

        [Test]
        public void AccumulateOver3()
        {
            Tnum t = new Tnum(Hstate.Stub);
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf Stub ", r.TestOutput);    
        }

        [Test]
        public void AccumulateOver4()
        {
            Tnum t = new Tnum(1000);
            t.AddState(Date(2013, 1, 1), 2000);

            Tnum r = t.AccumulatedOver(2, TheYear);           
            Assert.AreEqual("Time.DawnOf 2000 1/1/2012 12:00:00 AM 3000 1/1/2013 12:00:00 AM 4000 ", r.TestOutput);    
        }
    }
}