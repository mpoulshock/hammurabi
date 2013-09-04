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

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class Performance : H
    {
        // The sole purpose of this set of tests is to verify that these Akkadian functions
        // stay within reasonable performance tolerances.  They may not all pass all the time,
        // but they should pass almost all of the time.
        // Obviously, different machines will execute these functions at different speeds.

        /// <summary>
        /// A Tbool used to test the Akkadian functions.
        /// </summary>
        private static Tbool Tb1()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,2,2),false);
            tb.AddState(new DateTime(2015,3,3),true);
            tb.AddState(new DateTime(2015,4,4),false);
            return tb;
        }

        private static Tnum Tn1()
        {
            Tnum tn = new Tnum(0);
            tn.AddState(Date(2010, 01, 01), 7000);
            tn.AddState(Date(2013, 01, 01), 6000);
            tn.AddState(Date(2015, 01, 01), 5090);
            tn.AddState(Date(2017, 01, 01), 4470);
            return tn;
        }

        [Test]
        public void Performance_And ()
        {
            DateTime startTime = DateTime.Now;
            Tbool t = Tb1() && Tb1();
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 30);    
        }

        [Test]
        public void Performance_RunningElapsedIntervals ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tb1().RunningElapsedIntervals(TheDay);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 60);    
        }

        [Test]
        public void Performance_ContinuousElapsedIntervals ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tb1().ContinuousElapsedIntervals(TheDay);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 70);    
        }

        [Test]
        public void Performance_SlidingElapsedIntervals1 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tb1().SlidingElapsedIntervals(TheDay, 10);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 115);    
        }

        [Test]
        public void Performance_SlidingElapsedIntervals2 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = (new Tbool(true)).SlidingElapsedIntervals(TheDay, 10);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 105);    
        }

        [Test]
        public void Performance_TotalElapsedIntervals ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tb1().TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 70);    
        }

        [Test]
        public void Performance_Shift ()
        {
            DateTime startTime = DateTime.Now;
            Tbool t = Tb1().Shift(10, TheDay);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 60);    
        }

        [Test]
        public void Performance_RunningSummedIntervals_1 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = new Tnum(9.99).RunningSummedIntervals(TheDay);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 85);    
        }

        [Test]
        public void Performance_RunningSummedIntervals_2 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tn1().RunningSummedIntervals(TheDay);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 85);    
        }

        [Test]
        public void Performance_SlidingSummedIntervals_1 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = new Tnum(9.99).SlidingSummedIntervals(TheDay, 90);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 70);     
        }

        [Test]
        public void Performance_SlidingSummedIntervals_2 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tn1().SlidingSummedIntervals(TheDay, 90);
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 70); 
        }

        [Test]
        public void Performance_TotalSummedIntervals_2 ()
        {
            DateTime startTime = DateTime.Now;
            Tnum t = Tn1().TotalSummedIntervals(TheDay, Date(2012,1,1), Date(2016,1,1));
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 80);   
        }

        [Test]
        public void Performance_Subtraction ()
        {
            // ~10ms 
            DateTime startTime = DateTime.Now;
            Tnum t = new Tnum(10) + Tn1();
            int ResponseTimeInMs = Convert.ToInt32((DateTime.Now - startTime).TotalMilliseconds);
            Assert.IsTrue(ResponseTimeInMs < 10);    
        }
    }
}