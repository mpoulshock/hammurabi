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
    public class UnknownNess : H
    {
        // IsUnknown
        
        [Test]
        public void IsUnknown_1 ()
        {
            Tbool tbu = new Tbool();
            Assert.AreEqual(true, tbu.IsUnknown);        
        }
        
        [Test]
        public void IsUnknown_2 ()
        {
            Tbool tbu = new Tbool(true);
            Assert.AreEqual(false, tbu.IsUnknown);        
        }
        
        [Test]
        public void IsUnknown_3 ()
        {
            Tbool tbu = new Tbool();
            Assert.AreEqual(true, tbu.IsUnknown);        
        }
        
        [Test]
        public void IsUnknown_4 ()
        {
            Tbool tbu = new Tbool();
            Assert.AreEqual("Unknown", tbu.TestOutput);     
        }
        
        [Test]
        public void IsUnknown_5 ()
        {
            Facts.Clear();
            Tnum month = Facts.InputTnum(new Person("p"), "MonthTaxYearBegins");
            bool result = month.IsUnknown;
            Assert.AreEqual(true, result);     
        }
        
        // HasUnknown
        
        [Test]
        public void HasUnknown_1 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Assert.AreEqual(true, AnyAreUnknown(tbu, tbt, tbf));        
        }
        
        [Test]
        public void HasUnknown_2 ()
        {
            Tbool tbu = new Tbool();
            Assert.AreEqual(true, AnyAreUnknown(tbu));        
        }
        
        // Basic logic - And
        
        [Test]
        public void Unknown_Logic_And_1 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbt & tbf & tbu;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_And_2 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbt & tbu & tbf;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_And_3 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbu = new Tbool();
            Tbool t1 = tbt & tbu;
            Assert.AreEqual("Unknown", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_And_4 ()
        {
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbf & tbu;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);        
        }
        
        // Basic logic - Or
        
        [Test]
        public void Unknown_Logic_Or_1 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbu | tbf | tbt;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_Or_2 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbt | tbu | tbf;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_Or_3 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbu = new Tbool();
            Tbool t1 = tbt | tbu;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);        
        }
        
        [Test]
        public void Unknown_Logic_Or_4 ()
        {
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbf | tbu;
            Assert.AreEqual("Unknown", t1.TestOutput);        
        }
        
        // Basic logic - not
        
        [Test]
        public void Unknown_Logic_Not_1 ()
        {
            Tbool tbu = new Tbool();
            Tbool t1 = !tbu;
            Assert.AreEqual("Unknown", t1.TestOutput);        
        }
        
        // Basic logic - nested and/or
        
        [Test]
        public void Unknown_Logic_AndOr_1 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            Tbool tbu = new Tbool();
            Tbool t1 = tbf | ( tbu & tbt );
            Assert.AreEqual("Unknown", t1.TestOutput);        
        }
        
        // Math - multiplication
        
        [Test]
        public void Unknown_Mult_1 ()
        {
            Tnum n1 = new Tnum();
            Tnum n2 = new Tnum(4);
            Tnum result = n1 * n2;
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        [Test]
        public void Unknown_Mult_2 ()
        {
            Tnum n1 = new Tnum();
            Tnum n2 = new Tnum(0);
            Tnum result = n1 * n2;
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);        
        }
        
        [Test]
        public void Unknown_Mult_3 ()
        {
            Tnum n1 = new Tnum();
            Tnum result = n1 * 0;
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);        
        }
        
        // Math - abs
        
        [Test]
        public void Unknown_Abs_1 ()
        {
            Tnum n = new Tnum();
            Tnum result = n.Abs;
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // Math - round
        
        [Test]
        public void Unknown_Round_1 ()
        {
            Tnum n = new Tnum();
            Tnum result = n.RoundUp(2);
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // Math - min
        
        [Test]
        public void Unknown_Min_1 ()
        {
            Tnum n1 = new Tnum();
            Tnum n2 = new Tnum(4);
            Tnum result = Min(n1, n2);
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // Tvar.Lean
        
        [Test]
        public void Unknown_Lean_1 ()
        {
            Tbool b = new Tbool();
            Tbool result = b.Lean;
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // Tvar.AsOf
        
        [Test]
        public void Unknown_AsOf_1 ()
        {
            Tbool b = new Tbool();
            Tbool result = b.AsOf(DateTime.Now);
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // Tvar.IsAlways / IsEver
        
        [Test]
        public void Unknown_IsAlways_1 ()
        {
            Tbool b = new Tbool();
            Tbool result = b.IsAlways(true);
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        [Test]
        public void Unknown_IsEver_1 ()
        {
            Tbool b = new Tbool();
            Tbool result = b.IsEver(true);
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        // String concatenation
        
        [Test]
        public void Unknown_Concat_1 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr();
            Tstr ts3 = ts1 + " " + ts2;
            Assert.AreEqual("Unknown", ts3.Length.TestOutput);            
        }
        
        // Set.AsOf
        
        [Test]
        public void Unknown_SetAsOf_1 ()
        {
            Tset s1 = new Tset();
            Assert.AreEqual("Unknown", s1.AsOf(Time.DawnOf).TestOutput);        // Lean not working        
        }
        
        // Set.IsSubsetOf
        
        [Test]
        public void Unknown_Subset_1 ()
        {
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Tset s1 = new Tset(P1,P2);    
            Tset s2 = new Tset();
            Assert.AreEqual("Unknown", s2.IsSubsetOf(s1).TestOutput);        
        }
        
        // Set.Contains
        
        [Test]
        public void Unknown_SetContains_1 ()
        {
            Person P1 = new Person("P1");
            Tset s1 = new Tset();
            Assert.AreEqual("Unknown", s1.Contains(P1).TestOutput);        
        }
        
        // Set equality
        
        [Test]
        public void Unknown_SetEquality_1 ()
        {
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset();
            Tbool res = s1 == s2;
            Assert.AreEqual("Unknown", res.TestOutput);        
        }
        
        [Test]
        public void Unknown_SetEquality_2 ()
        {
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Tset s1 = new Tset(P1,P2);
            Tset s2 = new Tset();
            Tbool res = s1 != s2;
            Assert.AreEqual("Unknown", res.TestOutput);        
        }
        
        // IsAtOrBefore
        
        [Test]
        public void Unknown_IsAtOrBefore_1 ()
        {
            Tdate td1 = new Tdate(2000,1,1);
            Tdate td2 = new Tdate();
            Tbool result = td2 <= td1;
            Assert.AreEqual("Unknown", result.TestOutput);    
        }
    }
}