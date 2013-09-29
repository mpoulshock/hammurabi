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

using System;
using Hammurabi;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class MiscFcns : H
    {    
        // Recursion using Tvars - non-temporal
        
        [Test]
        public void Misc_Recursion1 ()
        {
            Assert.AreEqual(89, Fibonacci(10).Out);    
        }
        
        [Test]
        public void Misc_Recursion2 ()
        {
            Assert.AreNotEqual(89, Fibonacci(11).Out);    
        }
        
        [Test]
        public void Misc_Recursion3 ()
        {
            Assert.AreEqual(1, Fibonacci(1).Out);    
        }
        
        private static Tnum Fibonacci(Tnum x)
        {
           if (x <= 1)      // only works for values that do not change over time
               return 1;
           return Fibonacci(x - 1) + Fibonacci(x - 2);
        }
  
        // Recursion using Tvars - temporal

        [Test]
        public void Misc_TemporalRecursion1 ()
        {
            Assert.AreEqual(89, TemporalFibonacci(10).Out);    
        }
        
        [Test]
        public void Misc_TemporalRecursion2 ()
        {
            Assert.AreNotEqual(89, TemporalFibonacci(11).Out);    
        }
        
        [Test]
        public void Misc_TemporalRecursion3 ()
        {
            Assert.AreEqual(1, TemporalFibonacci(1).Out);    
        }
        
        [Test]
        public void Misc_TemporalRecursion4 ()
        {
            Tnum x = new Tnum();
            x.AddState(Time.DawnOf, 10);
            x.AddState(new DateTime(2000,1,1), 1);
            Assert.AreEqual("{Dawn: 89; 1/1/2000: 1}", TemporalFibonacci(x).Out);    
        }
        
        private static Tnum TemporalFibonacci(Tnum x)
        {
            return Switch<Tnum>(()=> x <= 1, ()=> new Tnum(1), 
                          ()=> TemporalFibonacci(x - 1) + TemporalFibonacci(x - 2));
        }
    }
}