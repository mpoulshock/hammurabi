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
    public class StringComparison : H
    {
        // EQUALS
        
        [Test]
        public void StringComparison1 ()
        {
            Tbool t = new Tstr("Hello, world!") == new Tstr("Hello, world!");
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);        
        }

        [Test]
        public void StringComparison2 ()
        {
            Tbool t = new Tstr("Hello, world!") == new Tstr("Jello, world!");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);                
        }
        
        [Test]
        public void StringComparison3 ()
        {
            Tbool t = new Tstr("Hello, world!") == "Hello, world!";
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);                
        }
        
        [Test]
        public void StringComparison4 ()
        {
            Tbool t = new Tstr("Hello, world!") == "Hello, world";
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);                
        }
        
        [Test]
        public void StringComparison5 ()
        {
            Tbool t = new Tstr(Hstate.Unstated) == "Hello, world";
            Assert.AreEqual("1/1/0001 12:00:00 AM Unstated ", t.TestOutput);                
        }
        
        // NOT EQUAL
        
        [Test]
        public void StringComparison11 ()
        {
            Tbool t = new Tstr("Hello, world!") != new Tstr("Hello, world!");
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);        
        }

        [Test]
        public void StringComparison12 ()
        {
            Tbool t = new Tstr("Hello, world!") != new Tstr("Jello, world!");
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);                
        }
        
        [Test]
        public void StringComparison13 ()
        {
            Tbool t = new Tstr("Hello, world!") != "Hello, world!";
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);                
        }
        
        [Test]
        public void StringComparison14 ()
        {
            Tbool t = new Tstr("Hello, world!") != "Hello, world";
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);                
        }
        
    }
}
