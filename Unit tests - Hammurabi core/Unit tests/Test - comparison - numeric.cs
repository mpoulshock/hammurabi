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
    public class NumericComparison : H
    {
        // EQUALS
        
        [Test]
        public void Test1 ()
        {
            Tbool t = new Tnum(2.0) == new Tnum(2);
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test2 ()
        {
            Tbool t = new Tnum(2.00000000000001) == new Tnum(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test3 ()
        {
            Tbool t = new Tnum(1.99999999999999) == new Tnum(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test4 ()
        {
            Tbool t = new Tnum(-2) == new Tnum(2);        
            Assert.AreEqual(false , t.Out);        
        }
        
        // TODO: Convert string currency values to Tnums
        
//        [Test]
//        public void Test5 ()
//        {
//            Tbool t = new Tnum("$3.94") == new Tnum(3.94);        
//            Assert.AreEqual(true , t.Out);        
//        }
        
        [Test]
        public void Test6 ()
        {
            Tbool t = new Tnum(3.94) == new Tnum(3.940);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tbool t = new Tnum(-3.94) == new Tnum(-3.940);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test8 ()
        {
            Tbool t = new Tnum(-3.94) == -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test9 ()
        {
            Tbool t = new Tnum(0.1) == new Tnum(0.10);        
            Assert.AreEqual(true , t.Out);        
        }

        
        // NOT EQUAL
        
        [Test]
        public void Test11 ()
        {
            Tbool t = new Tnum(2.0) != new Tnum(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test12 ()
        {
            Tbool t = new Tnum(2.00000000000001) != new Tnum(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test13 ()
        {
            Tbool t = new Tnum(1.99999999999999) != new Tnum(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test14 ()
        {
            Tbool t = new Tnum(-2) != new Tnum(2);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test16 ()
        {
            Tbool t = new Tnum(3.94) != new Tnum(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test17 ()
        {
            Tbool t = new Tnum(-3.94) != new Tnum(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test20 ()
        {
            Tbool t = new Tnum(-3.94) != -3.940;        
            Assert.AreEqual(false , t.Out);        
        }
        
        // GREATER THAN
        
        [Test]
        public void Test21 ()
        {
            Tbool t = new Tnum(2.0) > new Tnum(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test22 ()
        {
            Tbool t = new Tnum(2.00000000000001) > new Tnum(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test23 ()
        {
            Tbool t = new Tnum(1.99999999999999) > new Tnum(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test24 ()
        {
            Tbool t = new Tnum(-2) > new Tnum(2);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test26 ()
        {
            Tbool t = new Tnum(3.94) > new Tnum(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test27 ()
        {
            Tbool t = new Tnum(-3.94) > new Tnum(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test30 ()
        {
            Tbool t = new Tnum(-3.94) > -3.940;        
            Assert.AreEqual(false , t.Out);        
        }

        
        [Test]
        public void Test30_a ()
        {
            Tbool t = new Tbool(0 > 365);      
            Assert.AreEqual(false , t.Out);        
        }

        // LESS THAN
        
        [Test]
        public void Test31 ()
        {
            Tbool t = new Tnum(2.0) < new Tnum(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test32 ()
        {
            Tbool t = new Tnum(2.00000000000001) < new Tnum(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test33 ()
        {
            Tbool t = new Tnum(1.99999999999999) < new Tnum(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test34 ()
        {
            Tbool t = new Tnum(-2) < new Tnum(2);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test36 ()
        {
            Tbool t = new Tnum(3.94) < new Tnum(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test37 ()
        {
            Tbool t = new Tnum(-3.94) < new Tnum(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test40 ()
        {
            Tbool t = new Tnum(-3.94) < -3.940;        
            Assert.AreEqual(false , t.Out);        
        }
        
        // >=
        
        [Test]
        public void Test41 ()
        {
            Tbool t = new Tnum(2.0) >= new Tnum(2);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test42 ()
        {
            Tbool t = new Tnum(44) >= new Tnum(-3);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test43 ()
        {
            Tbool t = new Tnum(-44) >= new Tnum(-446);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test44 ()
        {
            Tbool t = new Tnum(-44) >= new Tnum(-4);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test45 ()
        {
            Tbool t = new Tnum(-3.94) >= -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        // <=
        
        [Test]
        public void Test51 ()
        {
            Tbool t = new Tnum(2.0) <= new Tnum(2);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test52 ()
        {
            Tbool t = new Tnum(44) <= new Tnum(-3);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test53 ()
        {
            Tbool t = new Tnum(-44) <= new Tnum(-446);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test54 ()
        {
            Tbool t = new Tnum(-44) <= new Tnum(-4);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test55 ()
        {
            Tbool t = new Tnum(-3.94) <= -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        // Temporal
        
        [Test]
        public void TemporalComparison1 ()
        {
            Tnum x = new Tnum(10);
            x.AddState(Date(2000,1,1), 1);
            Assert.AreEqual("{Dawn: false; 1/1/2000: true}", (x <= 1).Out );    
        }
        
    }
}
