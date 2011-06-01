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
using USC.Tit5;

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
            bool result = Sec6103.IsLegalHoliday(Date(2011,1,1));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test2 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2010,12,31));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test3 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2011,1,2));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test4 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,1,1));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test5 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,1,2));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test6 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2013,1,1));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test7 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2011,1,17));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test8 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2011,2,21));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test9 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2011,2,17));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test10 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2011,5,30));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test11 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2015,7,4));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test12 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2015,7,3));
            Assert.AreEqual(true, result);       
        }

        [Test]
        public void Test13 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2013,9,2));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test14 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2013,10,14));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test15 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,11,11));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test16 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,11,10));
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Test17 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,11,12));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test18 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,11,22));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test19 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,12,25));
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Test20 ()
        {
            bool result = Sec6103.IsLegalHoliday(Date(2012,12,26));
            Assert.AreEqual(false, result);       
        }
    }
}
