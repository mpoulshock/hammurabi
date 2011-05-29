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

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class TestSwitch : H
    {

        [Test]
        public void TSwitch1 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(tbf, new Tnum(41),
                                 tbt, new Tnum(42) );
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }
        
        [Test]
        public void TSwitch2 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(tbt, new Tnum(41),
                                 tbf, new Tnum(42) );
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 41 ", result.TestOutput);        
        }
        
        [Test]
        public void TSwitch3 ()
        {
            Tbool tbt = new Tbool(true);
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(tbf, 41,
                                 tbt, 42 );
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }
        
        [Test]
        public void TSwitch4 ()
        {
            Tnum result = Switch(false, 41,
                                 true, 42 );
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }
        
        
        [Test]
        public void TSwitch_Exhaustion_ShouldDefaultToNull ()
        {
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(tbf, new Tnum(41),
                                 tbf, new Tnum(42) );

            // Should default to "null"
            Assert.AreEqual("1/1/0001 12:00:00 AM Null ", result.TestOutput);        
        }
        
        [Test]
        public void TSwitch_Malformed1 ()
        {
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(new Tnum(41), tbf,
                                 tbf, new Tnum(42) );
            
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        [Test]
        public void TSwitch_Malformed2 ()
        {
            Tbool tbf = new Tbool(false);
            
            Tnum result = Switch(tbf, tbf, new Tnum(42));
            Assert.AreEqual("Unknown", result.TestOutput);        
        }
        
        
    }
}