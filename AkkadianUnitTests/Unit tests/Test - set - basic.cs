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

using Hammurabi;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class Set : H
    {        
        // Legal entities to be used as members of the sets
        public static Thing P1 = new Thing("P1");
        public static Thing P2 = new Thing("P2");
        public static Thing P3 = new Thing("P3");
        
        
        // Construct a Tset from another Tset
        
        [Test]
        public void Constructor1 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
            Tset s2 = new Tset(s1);
            Assert.AreEqual("P1", s2.Lean.Out); 
        }
        
        // .Lean
        
        [Test]
        public void Test0_1 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
            Assert.AreEqual("P1", s1.Lean.Out);
        }
        
        // .AsOf
        
        [Test]
        public void Test_AsOf_1 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P2);
            Assert.AreEqual("P2", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        [Test]
        public void Test_AsOf_2 ()
        {
            Tset s1 = new Tset();
            s1.AddState(Time.DawnOf, P1, P2);
            s1.AddState(Time.DawnOf.AddYears(3), P2);
            Assert.AreEqual("P1, P2", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        
        // .Out
    
        [Test]
        public void Test1_1 ()
        {
            Assert.AreEqual("P1, P2", new Tset(P1,P2).Out);        
        }
        
        [Test]
        public void Test1_2 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual("", s1.Out);        
        }
        
        // .Count
        
        [Test]
        public void Count1 ()
        {
            Assert.AreEqual(2, new Tset(P1,P2).Count.Out);        
        }
        
        [Test]
        public void Count2 ()
        {
            // This is how you assert an eternally empty set
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual(0, s1.Count.Out);        
        }
        
        [Test]
        public void Count3 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual(0, s1.Count.Out);        
        }

        [Test]
        public void Count4 ()
        {
            Tset s1 = new Tset(Hstate.Stub);
            Assert.AreEqual("Stub", s1.Count.Out);        
        }

        // .IsEmpty
        
        [Test]
        public void IsEmpty1 ()
        {
            Assert.AreEqual(false, new Tset(P1,P2).IsEmpty.Out);        
        }
        
        [Test]
        public void IsEmpty2 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual(true, s1.IsEmpty.Out);        
        }

        [Test]
        public void IsEmpty3 ()
        {
            Tset s1 = new Tset(Hstate.Uncertain);
            Assert.AreEqual("Uncertain", s1.IsEmpty.Out);        
        }

        // .IsSubsetOf
        
        [Test]
        public void Test6 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P1,P3);    
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tset s1 = new Tset(P1,P2);    
            Tset s2 = new Tset(P1);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test8 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test9 ()
        {
            Tset s1 = new Tset(P1,P2,P3);    
            Tset s2 = new Tset(P1,P2);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test10 ()
        {
            Tset s1 = new Tset(P1,P2);        
            Tset s2 = new Tset(P1,P2,P3);
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test11 ()
        {
            Tset s1 = new Tset(P1,P2);        
            Tset s2 = new Tset();
            s2.SetEternally();
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test12 ()
        {
            Tset s1 = new Tset();    
            s1.SetEternally();
            Tset s2 = new Tset(P1,P2);
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        // .Contains
        
        [Test]
        public void Test20 ()
        {
            Assert.AreEqual(true, new Tset(P1,P2).Contains(P1).Out);        
        }
        
        [Test]
        public void Test21 ()
        {
            Assert.AreEqual(false, new Tset(P1,P2).Contains(P3).Out);        
        }
        
        [Test]
        public void Test22 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Assert.AreEqual(false, s1.Contains(P3).Out);        
        }
        
        // Union
        
        [Test]
        public void Union1 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("P1, P2, P3", res.Out);        
        }
        
        [Test]
        public void Union2 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("P1, P2, P3", res.Out);        
        }
        
        [Test]
        public void Union3 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("P2, P3", res.Out);        
        }

        [Test]
        public void Union4 ()
        {
            Tset s1 = new Tset(Hstate.Stub);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 | s2;
            Assert.AreEqual("Stub", res.Out);        
        }

        // Intersection
        
        [Test]
        public void Intersection1 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("P2", res.Out);        
        }
        
        [Test]
        public void Intersection2 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Intersection3 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Intersection4 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("P2, P3", res.Out);        
        }

        [Test]
        public void Intersection5 ()
        {
            Tset s1 = new Tset(Hstate.Stub);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 & s2;
            Assert.AreEqual("Stub", res.Out);        
        }

        // Relative complement
        
        [Test]
        public void Complement1 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement2 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement3 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Complement4 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3);
            Tset res = s1 - s2;
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement5 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tset res = s2 - s1;
            Assert.AreEqual("P2, P3", res.Out);        
        }

        [Test]
        public void Complement6 ()
        {
            Tset s1 = new Tset(Hstate.Unstated);
            Tset s2 = new Tset(P2,P3);
            Tset res = s2 - s1;
            Assert.AreEqual("Unstated", res.Out);        
        }

        // Equality
        
        [Test]
        public void Test60 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test61 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P1);
            Tbool res = s1 == s2;
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test62 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test63 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test64 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s2 == s1;
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test65 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Tbool res = s1 == s2;
            Assert.AreEqual(true, res.Out);        
        }
        
        // Inequality
        
        [Test]
        public void Test70 ()
        {
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test71 ()
        {
            Tset s1 = new Tset(P1);
            Tset s2 = new Tset(P1);
            Tbool res = s1 != s2;
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test72 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test73 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P2,P3,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test74 ()
        {
            Tset s1 = new Tset();
            s1.SetEternally();
            Tset s2 = new Tset(P2,P3);
            Tbool res = s2 != s1;
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test75 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Tset s2 = new Tset(P1,P2,P3);
            Tbool res = s1 != s2;
            Assert.AreEqual(false, res.Out);        
        }

        // Reverse

        [Test]
        public void Reverse1 ()
        {
            Tset s1 = new Tset(P1,P2,P3);
            Assert.AreEqual("P3, P2, P1", s1.Reverse.Out);        
        }

        [Test]
        public void Reverse2 ()
        {
            Tset s1 = new Tset(Hstate.Unstated);
            Assert.AreEqual("Unstated", s1.Reverse.Out);        
        }

        // Tset.Out

        [Test]
        public void TestOutput1 ()
        {
            string val = "ham; beans";
            string[] items = val.Split(new char[] {';'});
            List<Thing> list = new List<Thing>();

            foreach (string i in items)
            {
                list.Add(new Thing(i.Trim()));
            }

            Tset result = new Tset(list);
            Assert.AreEqual("ham, beans", result.Out);        
        }
    }
}