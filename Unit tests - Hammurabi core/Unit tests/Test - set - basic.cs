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
using System;

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class Set : H
    {        
        // Legal entities to be used as members of the sets
        public static Person P1 = new Person("P1");
        public static Person P2 = new Person("P2");
        public static Person P3 = new Person("P3");
        
        
        // .Lean
        
        [Test]
        public void Test0_1 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
            Assert.AreEqual("1/1/0001 12:00:00 AM P1 ", s1.Lean.TestOutput);        // Lean not working
        }
        
        // .AsOf
        
        [Test]
        public void Test_AsOf_1 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P2);
            Assert.AreEqual("1/1/0001 12:00:00 AM P2 ", s1.AsOf(Time.DawnOf.AddYears(2)).TestOutput);        // Lean not working
        }
        
        [Test]
        public void Test_AsOf_2 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1, P2);
            s1.AddState(Time.DawnOf.AddYears(3), P2);
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P2 ", s1.AsOf(Time.DawnOf.AddYears(2)).TestOutput);        // Lean not working
        }
        
        
        // .TestOutput
    
        [Test]
        public void Test1_1 ()
        {
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P2 ", new Tset(P1,P2).TestOutput);        
        }
        
        [Test]
        public void Test1_2 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM ", s1.TestOutput);        
        }
        
        // .Count
        
        [Test]
        public void Test2 ()
        {
            Assert.AreEqual("1/1/0001 12:00:00 AM 2 ", new Tset(P1,P2).Count.TestOutput);        
        }
        
        [Test]
        public void Test3_1 ()
        {
            // This is how you assert an eternally empty set
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", s1.Count.TestOutput);        
        }
        
        [Test]
        public void Test3_2 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", s1.Count.TestOutput);        
        }
        
        // .IsEmpty
        
        [Test]
        public void Test4 ()
        {
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", new Tset(P1,P2).IsEmpty.TestOutput);        
        }
        
        [Test]
        public void Test5 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", s1.IsEmpty.TestOutput);        
        }
        
        // .IsSubsetOf
        
        [Test]
        public void Test6 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P1,P3);    
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tset s1 = new Tset(P1,P2);    
            Tset s2 = new Tset(P1);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test8 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test9 ()
        {
            Tset s1 = new Tset(P1,P2,P3);    
            Tset s2 = new Tset(P1,P2);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test10 ()
        {
            Tset s1 = new Tset(P1,P2);        
            Tset s2 = new Tset(P1,P2,P3);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test11 ()
        {
            Tset s1 = new Tset(P1,P2);        
            Tset s2 = new Tset();
            s2.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        [Test]
        public void Test12 ()
        {
            Tset s1 = new Tset();    
            s1.SetEternally();
            Tset s2 = new Tset(P1,P2);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        // .Contains
        
        [Test]
        public void Test20 ()
        {
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", new Tset(P1,P2).Contains(P1).TestOutput);        
        }
        
        [Test]
        public void Test21 ()
        {
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", new Tset(P1,P2).Contains(P3).TestOutput);        
        }
        
        [Test]
        public void Test22 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", s1.Contains(P3).TestOutput);        
        }
        
        // Union
        
        [Test]
        public void Test30 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P2, P3 ", res.TestOutput);        
        }
        
        [Test]
        public void Test31 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P2, P3 ", res.TestOutput);        
        }
        
        [Test]
        public void Test32 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P2, P3 ", res.TestOutput);        
        }
        
        // Intersection
        
        [Test]
        public void Test40 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P2 ", res.TestOutput);        
        }
        
        [Test]
        public void Test41 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM ", res.TestOutput);        
        }
        
        [Test]
        public void Test42 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM ", res.TestOutput);        
        }
        
        [Test]
        public void Test43 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P2, P3 ", res.TestOutput);        
        }
        
        // Relative complement
        
        [Test]
        public void Test50 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P1 ", res.TestOutput);        
        }
        
        [Test]
        public void Test51 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P1 ", res.TestOutput);        
        }
        
        [Test]
        public void Test52 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM ", res.TestOutput);        
        }
        
        [Test]
        public void Test53 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM P1 ", res.TestOutput);        
        }
        
        [Test]
        public void Test54 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s2 - s1;
            Assert.AreEqual("1/1/0001 12:00:00 AM P2, P3 ", res.TestOutput);        
        }
        
        // Equality
        
        [Test]
        public void Test60 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
        
        [Test]
        public void Test61 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P1);
            Tbool res = s1 == s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        [Test]
        public void Test62 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
        
        [Test]
        public void Test63 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
        
        [Test]
        public void Test64 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s2 == s1;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
        
        [Test]
        public void Test65 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        // Inequality
        
        [Test]
        public void Test70 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        [Test]
        public void Test71 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P1);
            Tbool res = s1 != s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
        
        [Test]
        public void Test72 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        [Test]
        public void Test73 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        [Test]
        public void Test74 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s2 != s1;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", res.TestOutput);        
        }
        
        [Test]
        public void Test75 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", res.TestOutput);        
        }
    }
}