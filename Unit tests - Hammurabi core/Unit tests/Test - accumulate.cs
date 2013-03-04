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
        [Test]
        public void Accumulate1()
        {
            Tnum t = new Tnum(1000);
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf 2000 ", r.TestOutput);    
        }

        [Test]
        public void Accumulate2()
        {
            Tnum t = new Tnum(new Hval());
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf Unstated ", r.TestOutput);    
        }

        [Test]
        public void Accumulate3()
        {
            Tnum t = new Tnum(Hstate.Stub);
            Tnum r = t.AccumulatedOver(2, TheMonth);           
            Assert.AreEqual("Time.DawnOf Stub ", r.TestOutput);    
        }

        [Test]
        public void Accumulate4()
        {
            Tnum t = new Tnum(1000);
            t.AddState(Date(2013, 1, 1), 2000);

            Tnum r = t.AccumulatedOver(2, TheYear);           
            Assert.AreEqual("Time.DawnOf 2000 1/1/2012 12:00:00 AM 3000 1/1/2013 12:00:00 AM 4000 1/1/2199 12:00:00 AM 2000 ", r.TestOutput);    
        }
    }
}