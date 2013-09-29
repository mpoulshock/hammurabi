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
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    #pragma warning disable 219
    
    [TestFixture]
    public class ThingBaseTests : H
    {
        [Test]
        public void ThingBase1 ()
        {
            Facts.ClearThings();
            Facts.AddThing("thing1");
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase2 ()
        {
            Facts.ClearThings();
            Assert.AreEqual(0, Facts.ThingCount());
        }

        [Test]
        public void ThingBase3 ()
        {
            Facts.ClearThings();
            Facts.AddThing("thing1");
            Facts.AddThing("thing1");
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase4 ()
        {
            Facts.ClearThings();
            Facts.AddThing("thing1");
            Facts.AddThing(new Thing("thing1"));
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase5 ()
        {
            Facts.ClearThings();
            Facts.AddThing(new Thing("thing1"));
            Facts.AddThing("thing1");
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase6 ()
        {
            Facts.ClearThings();
            Facts.AddThing(new Thing("thing1"));
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase7 ()
        {
            Facts.ClearThings();
            Facts.AddThing(new Thing("thing1"));
            Facts.AddThing(new Thing("thing1"));
            Assert.AreEqual(1, Facts.ThingCount());
        }

        [Test]
        public void ThingBase8 ()
        {
            Facts.ClearThings();
            Facts.AddThing(new Thing("thing1"));
            Facts.AddThing(new Thing("thing2"));
            Assert.AreEqual(2, Facts.ThingCount());
        }

        [Test]
        public void ThingBase9 ()
        {
            Facts.AddThing(new Thing("thing1"));
            Facts.AddThing(new Thing("thing2"));
            Facts.ClearThings();
            Assert.AreEqual(0, Facts.ThingCount());
        }

        [Test]
        public void ThingBase10 ()
        {
            Facts.ClearThings();
            Thing t = null;
            Facts.AddThing(t);
            Assert.AreEqual(0, Facts.ThingCount());
        }

        [Test]
        public void ThingBase11 ()
        {
            Facts.ClearThings();
            Facts.AddThing(new Thing(""));
            Assert.AreEqual(0, Facts.ThingCount());
        }
    }
}
