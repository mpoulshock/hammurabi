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
    public class SymmetricalFacts : H
    {
        private static Person p1 = new Person("P1");
        private static Person p2 = new Person("P2");
        
        [Test]
        public void SymTT ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2);
            Facts.Assert(p2, "IsMarriedTo", p1);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);       
        }
        
        [Test]
        public void SymTF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2);
            Facts.Assert(p2, "IsMarriedTo", p1, false);                         // contradictory assertion
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("1/1/0001 12:00:00 AM ??? ", result.TestOutput);    // what is desired here? 
        }
        
        [Test]
        public void SymTU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);       
        }
        
        [Test]
        public void SymFF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Facts.Assert(p2, "IsMarriedTo", p1, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);       
        }  
        
        [Test]
        public void SymFU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);       
        }
        
        [Test]
        public void SymUU ()
        {
            Facts.Clear();
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("Unknown", result.TestOutput);       
        }
        
    }
}
