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
    public class RoundingFcns : H
    {    
        // ROUND UP
        
        [Test]
        public void Up1 ()
        {
            Tnum res = new Tnum(121).RoundUp(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 130 ", res.TestOutput);    
        }
        
        [Test]
        public void Up2 ()
        {
            Tnum res = new Tnum(120).RoundUp(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 120 ", res.TestOutput);        
        }
        
        [Test]
        public void Up3 ()
        {
            Tnum res = new Tnum(7.33).RoundUp(0.25);
            Assert.AreEqual("1/1/0001 12:00:00 AM 7.50 ", res.TestOutput);            
        }
        
        [Test]
        public void Up4 ()
        {
            Tnum res = new Tnum(7.5).RoundUp(0.25);
            Assert.AreEqual("1/1/0001 12:00:00 AM 7.5 ", res.TestOutput);            
        }
        
        [Test]
        public void Up5 ()
        {
            Tnum res = new Tnum(1324103).RoundUp(50000);
            Assert.AreEqual("1/1/0001 12:00:00 AM 1350000 ", res.TestOutput);        
        }
        
        // ROUND DOWN
        
        [Test]
        public void Down1 ()
        {
            Tnum res = new Tnum(121).RoundDown(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 120 ", res.TestOutput);    
        }
        
        [Test]
        public void Down2 ()
        {
            Tnum res = new Tnum(7.33).RoundDown(0.25);
            Assert.AreEqual("1/1/0001 12:00:00 AM 7.25 ", res.TestOutput);        
        }
        
        [Test]
        public void Down3 ()
        {
            Tnum res = new Tnum(7.5).RoundDown(0.25);
            Assert.AreEqual("1/1/0001 12:00:00 AM 7.5 ", res.TestOutput);        
        }
        
        [Test]
        public void Down4 ()
        {
            Tnum res = new Tnum(1324103).RoundDown(50000);
            Assert.AreEqual("1/1/0001 12:00:00 AM 1300000 ", res.TestOutput);    
        }
        
        // ROUND TO NEAREST
        
        [Test]
        public void Near1 ()
        {
            Tnum res = new Tnum(121).RoundToNearest(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 120 ", res.TestOutput);    
        }
        
        [Test]
        public void Near2 ()
        {
            Tnum res = new Tnum(127).RoundToNearest(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 130 ", res.TestOutput);    
        }
        
        [Test]
        public void Near3 ()
        {
            Tnum res = new Tnum(125).RoundToNearest(10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 130 ", res.TestOutput);        
        }
        
        [Test]
        public void Near4 ()
        {
            Tnum res = new Tnum(121).RoundToNearest(10, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 120 ", res.TestOutput);
        }
        
        [Test]
        public void Near5 ()
        {
            Tnum res = new Tnum(127).RoundToNearest(10, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 130 ", res.TestOutput);        
        }
        
        [Test]
        public void Near6 ()
        {
            Tnum res = new Tnum(125).RoundToNearest(10, true);
            Assert.AreEqual("1/1/0001 12:00:00 AM 120 ", res.TestOutput);    
        }
        
        [Test]
        public void Near9 ()
        {
            Tnum res = new Tnum(88.34).RoundToNearest(0.10);
            Assert.AreEqual("1/1/0001 12:00:00 AM 88.30 ", res.TestOutput);    
        }
        
        [Test]
        public void Near10 ()
        {
            Tnum res = new Tnum(88.34).RoundToNearest(1);
            Assert.AreEqual("1/1/0001 12:00:00 AM 88.00 ", res.TestOutput);    
        }
        
    }
}