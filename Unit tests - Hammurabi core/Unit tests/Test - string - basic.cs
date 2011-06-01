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

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class BasicString : H
    {
        [Test]
        public void Test1 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr("world");
            Tstr ts3 = ts1 + ts2;
            Assert.AreEqual("1/1/0001 12:00:00 AM hello,world ", ts3.TestOutput);            
        }
        
        [Test]
        public void Test2 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr("world");
            Tstr ts3 = ts1 + " " + ts2;
            Assert.AreEqual("1/1/0001 12:00:00 AM hello, world ", ts3.TestOutput);            
        }
        
        [Test]
        public void Test3 ()
        {
            Tstr ts1 = new Tstr("hello,") + " world";
            Assert.AreEqual("1/1/0001 12:00:00 AM hello, world ", ts1.TestOutput);            
        }
        
        [Test]
        public void Test4 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr("world");
            Tstr ts3 = ts1 + " " + ts2;
            Assert.AreEqual("1/1/0001 12:00:00 AM 12 ", ts3.Length.TestOutput);            
        }
        
        [Test]
        public void Test10 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", ts1.Contains("hello").TestOutput);            
        }
        
        [Test]
        public void Test11 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", ts1.Contains("yello").TestOutput);            
        }
        
        [Test]
        public void Test12 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", ts1.Contains("yello").TestOutput);            
        }
        
        [Test]
        public void Test13 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", ts1.StartsWith("h").TestOutput);            
        }
        
        [Test]
        public void Test14 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", ts1.StartsWith("j").TestOutput);            
        }
        
        [Test]
        public void Test15 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", ts1.EndsWith("d").TestOutput);            
        }
        
        [Test]
        public void Test16 ()
        {
            Tstr ts1 = new Tstr("hello, world");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", ts1.EndsWith("j").TestOutput);            
        }       
    }
}