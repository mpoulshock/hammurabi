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
    public partial class ElapsedTime : H
    {   
        // SlidingElapsedIntervals

        [Test]
        public void SlidingElapsedIntervals1 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);

            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);

            Assert.AreEqual("{Dawn: 0; 1/2/2015: 1; 1/3/2015: 2; 1/4/2015: 1; 1/5/2015: 0}", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals2 ()
        {
            Tbool tb = new Tbool(false);
            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);

            Assert.AreEqual(0, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals3 () 
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Unstated);
            tb.AddState(new DateTime(2015,3,1), false);

            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);

            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals4 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Stub);
            tb.AddState(new DateTime(2015,3,1), false);

            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);

            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals5 ()
        {
            Tbool tb = new Tbool(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);
            tb.AddState(new DateTime(2015,1,10),true);
            tb.AddState(new DateTime(2015,2,18),false);

            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);
            string tline = "{Dawn: 0; 1/2/2015: 1; 1/3/2015: 2; 1/4/2015: 1; 1/5/2015: 0; " +
                "1/11/2015: 1; 1/12/2015: 2; 2/19/2015: 1; 2/20/2015: 0}"; 

            Assert.AreEqual(tline, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals6 ()
        {
            Tbool tb = new Tbool(true);
            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);
            Assert.AreEqual(2, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals7 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(Date(2012,1,1), false);
            Tnum actual = t.SlidingElapsedIntervals(TheYear, 2);
            Assert.AreEqual("{Dawn: 0; 1/1/1801: 1; 1/1/1802: 2; 1/1/2013: 1; 1/1/2014: 0}", actual.Out);
        }

        [Test]
        public void SlidingElapsedIntervals8 ()
        {
            Tbool t = new Tbool(true);
            Tnum actual = t.SlidingElapsedIntervals(TheYear, 2);
            Assert.AreEqual(2, actual.Out);      
        }

        [Test]
        public void SlidingElapsedIntervals9 () 
        {
            Tbool tb = new Tbool(Hstate.Unstated);
            Tnum r = tb.SlidingElapsedIntervals(TheDay, 2);
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals10 () 
        {
            // Window size is unstated
            Tbool tb = new Tbool(true);
            Tnum r = tb.SlidingElapsedIntervals(TheDay, new Tnum(Hstate.Unstated));
            Assert.AreEqual("Unstated", r.Out);    
        }
    }
}
