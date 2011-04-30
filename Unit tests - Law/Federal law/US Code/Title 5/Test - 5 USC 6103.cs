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
//
// NO REPRESENTATION OR WARRANTY IS MADE THAT THIS PROGRAM ACCURATELY REFLECTS
// OR EMBODIES ANY APPLICABLE LAWS, REGULATIONS, RULES OR EXECUTIVE ORDERS 
// ("LAWS"). YOU SHOULD RELY ONLY ON OFFICIAL VERSIONS OF LAWS PUBLISHED BY THE 
// RELEVANT GOVERNMENT AUTHORITY, AND YOU ASSUME THE RESPONSIBILITY OF 
// INDEPENDENTLY VERIFYING SUCH LAWS. THE USE OF THIS PROGRAM IS NOT A 
// SUBSTITUTE FOR THE ADVICE OF AN ATTORNEY.

using Hammurabi;
using NUnit.Framework;
using System;

namespace Hammurabi.UnitTests
{
    /// <summary>
    /// 5 U.S.C. 6103 - Federal holidays.
    /// </summary>
    [TestFixture]
    public class USC_Tit5_Sec6103 : H
    {
        [Test]
        public void Test1 ()
        {
            DateTime date = new DateTime(2011,1,1);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test2 ()
        {
            DateTime date = new DateTime(2010,12,31);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test3 ()
        {
            DateTime date = new DateTime(2011,1,2);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test4 ()
        {
            DateTime date = new DateTime(2012,1,1);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test5 ()
        {
            DateTime date = new DateTime(2012,1,2);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test6 ()
        {
            DateTime date = new DateTime(2013,1,1);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test7 ()
        {
            DateTime date = new DateTime(2011,1,17);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test8 ()
        {
            DateTime date = new DateTime(2011,2,21);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test9 ()
        {
            DateTime date = new DateTime(2011,2,17);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test10 ()
        {
            DateTime date = new DateTime(2011,5,30);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test11 ()
        {
            DateTime date = new DateTime(2015,7,4);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test12 ()
        {
            DateTime date = new DateTime(2015,7,3);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }

        [Test]
        public void Test13 ()
        {
            DateTime date = new DateTime(2013,9,2);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test14 ()
        {
            DateTime date = new DateTime(2013,10,14);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test15 ()
        {
            DateTime date = new DateTime(2012,11,11);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test16 ()
        {
            DateTime date = new DateTime(2012,11,10);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test17 ()
        {
            DateTime date = new DateTime(2012,11,12);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test18 ()
        {
            DateTime date = new DateTime(2012,11,22);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test19 ()
        {
            DateTime date = new DateTime(2012,12,25);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test20 ()
        {
            DateTime date = new DateTime(2012,12,26);
            bool result = USC.Tit5.Sec6103.IsLegalHoliday(date);
            Assert.AreEqual(false, result);       
        }
        
    }
}
