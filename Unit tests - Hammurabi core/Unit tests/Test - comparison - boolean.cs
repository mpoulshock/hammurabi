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
    public class BooleanComparison : H
    {
        // EQUALS
        
        [Test]
        public void BooleanComparison1 ()
        {
            Tbool t = new Tbool(true) == new Tbool(false);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void BooleanComparison2 ()
        {
            Tbool t = new Tbool(true) == new Tbool(true);
            Assert.AreEqual(true , t.Out);            
        }
        
        [Test]
        public void BooleanComparison3 ()
        {
            Tbool t = new Tbool(false) == new Tbool(false);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison4 ()
        {
            Tbool t = new Tbool(true) == true;
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison5 ()
        {
            Tbool t = new Tbool(true) == false;
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison6 ()
        {
            Tbool t = true == new Tbool(true);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison7 ()
        {
            Tbool t = false == new Tbool(true);
            Assert.AreEqual(false , t.Out);        
        }
        
        // NOT EQUAL
        
        [Test]
        public void BooleanComparison11 ()
        {
            Tbool t = new Tbool(true) != new Tbool(false);
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void BooleanComparison12 ()
        {
            Tbool t = new Tbool(true) != new Tbool(true);
            Assert.AreEqual(false , t.Out);            
        }
    
        [Test]
        public void BooleanComparison13 ()
        {
            Tbool t = new Tbool(false) != new Tbool(false);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison14 ()
        {
            Tbool t = new Tbool(true) != true;
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison15 ()
        {
            Tbool t = new Tbool(true) != false;
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison16 ()
        {
            Tbool t = true != new Tbool(true);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison17 ()
        {
            Tbool t = false != new Tbool(true);
            Assert.AreEqual(true , t.Out);        
        }
        
    }
}
