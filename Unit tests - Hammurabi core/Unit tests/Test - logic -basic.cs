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
	public class BasicLogic : H
	{
		// AND
		
		[Test]
		public void LogicAnd1 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt & tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);		
		}
		
		[Test]
		public void LogicAnd2 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t2 = tbt & tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t2.TestOutput);		
		}
		
		[Test]
		public void LogicAnd3 ()
		{
			Tbool tbf = new Tbool(false);
			Tbool t7 = tbf & tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t7.TestOutput);		
		}
		
		[Test]
		public void LogicAnd4 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt & tbt & tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);			
		}
		
        [Test]
        public void LogicAnd5 ()
        {
            Tbool t1 = new Tbool(true);
            Tbool t2 = new Tbool(null);
            Tbool r = t1 & t2;
            Assert.AreEqual("1/1/0001 12:00:00 AM Null ", r.TestOutput);          
        }
        
        [Test]
        public void LogicAnd6 ()
        {
            Tbool t1 = new Tbool(false);
            Tbool t2 = new Tbool(null);
            Tbool r = t1 & t2;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", r.TestOutput);          
        }
        
        [Test]
        public void LogicAnd7 ()
        {
            Tbool t1 = new Tbool(null);
            Tbool t2 = new Tbool(null);
            Tbool r = t1 & t2;
            Assert.AreEqual("1/1/0001 12:00:00 AM Null ", r.TestOutput);          
        }
        
        [Test]
        public void LogicAnd8 ()
        {
            Tbool t1 = new Tbool();
            Tbool t2 = new Tbool(null);
            Tbool r = t1 & t2;
            Assert.AreEqual("Unknown", r.TestOutput);          
        }
        
		[Test]
		public void LogicAnd9 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t1 = tbt & false;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);			
		}
		
		[Test]
		public void LogicAnd10 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t1 = false & tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);			
		}
		
		
		// OR
		
		[Test]
		public void LogicOr1 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt | tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);		
		}
		
		[Test]
		public void LogicOr2 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t2 = tbt | tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t2.TestOutput);		
		}
		
		[Test]
		public void LogicOr3 ()
		{
			Tbool tbf = new Tbool(false);
			Tbool t7 = tbf | tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t7.TestOutput);		
		}
		
		[Test]
		public void LogicOr4 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbf | tbf | tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);			
		}
		
		[Test]
		public void LogicOr5 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbf | (tbf | tbt);
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);			
		}
		
		// NOT
		
		[Test]
		public void LogicNot1 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t1 = !tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);	
		}
		
		[Test]
		public void LogicNot2 ()
		{
			Tbool tbf = new Tbool(false);
			Tbool t2 = !tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t2.TestOutput);		
		}
		
		// XOR
		
		[Test]
		public void LogicXor1 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt ^ tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);		
		}
		
		[Test]
		public void LogicXor2 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool t2 = tbt ^ tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t2.TestOutput);		
		}
		
		[Test]
		public void LogicXor3 ()
		{
			Tbool tbf = new Tbool(false);
			Tbool t7 = tbf ^ tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t7.TestOutput);		
		}
		
		[Test]
		public void LogicXor4 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbf ^ tbf ^ tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);			
		}
		
		[Test]
		public void LogicXor5 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt ^ tbf ^ tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);			
		}
		
		[Test]
		public void LogicXor6 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbt ^ tbf ^ tbt;
			Assert.AreEqual("1/1/0001 12:00:00 AM False ", t1.TestOutput);			
		}
		
		[Test]
		public void LogicXor7 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tbool t1 = tbf ^ tbt ^ tbf;
			Assert.AreEqual("1/1/0001 12:00:00 AM True ", t1.TestOutput);			
		}
		
		// BOOL COUNT
		
		[Test]
		public void LogicBoolCount1 ()
		{
			Tbool tbt = new Tbool(true);
			Tnum result = BoolCount(true, tbt);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1 ", result.TestOutput);	
		}
		
		[Test]
		public void LogicBoolCount2 ()
		{
			Tbool tbt = new Tbool(true);
			Tnum result = BoolCount(false, tbt);
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);	
		}
		
		[Test]
		public void LogicBoolCount3 ()
		{
			Tbool tbt = new Tbool(true);
			Tnum result = BoolCount(false, tbt, tbt);
			Assert.AreEqual("1/1/0001 12:00:00 AM 0 ", result.TestOutput);	
		}
		
		[Test]
		public void LogicBoolCount4 ()
		{
			Tbool tbt = new Tbool(true);
			Tnum result = BoolCount(true, tbt, tbt);
			Assert.AreEqual("1/1/0001 12:00:00 AM 2 ", result.TestOutput);	
		}
		
		[Test]
		public void LogicBoolCount5 ()
		{
			Tbool tbt = new Tbool(true);
			Tbool tbf = new Tbool(false);
			Tnum result = BoolCount(true, tbt, tbf);
			Assert.AreEqual("1/1/0001 12:00:00 AM 1 ", result.TestOutput);	
		}
		
		
	}
}
