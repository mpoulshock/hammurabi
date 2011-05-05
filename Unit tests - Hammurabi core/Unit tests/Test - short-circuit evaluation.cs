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
using System;

namespace Hammurabi.UnitTests.CoreFcns
{
    #pragma warning disable 219
    
    [TestFixture]
    public class ShortCircuitEval : H
    {
        // A string to keep track of the order in which items are investigated
        private static string result = "";
        
        // Some functions to simulate "rules"
        private static Tbool MethodF()
        {
            result += "F";
            return new Tbool(false);
        }
        private static Tbool MethodT()
        {
            result += "T";
            return new Tbool(true);
        }
        private static Tbool MethodU()
        {
            result += "U";
            return new Tbool();
        }
        
        // Tests for correct IsTrue / IsFalse determinations
        
        [Test]
        public void IsDefinitelyFalse1 ()
        {
            Tbool t = new Tbool(true);
            Assert.AreEqual(false, t.IsFalse);      
        }
        
        [Test]
        public void IsDefinitelyFalse2 ()
        {
            Tbool t = new Tbool();
            Assert.AreEqual(false, t.IsFalse);      
        }
        
        [Test]
        public void IsDefinitelyFalse3 ()
        {
            Tbool t = new Tbool(false);
            Assert.AreEqual(true, t.IsFalse);      
        }
        
        [Test]
        public void IsDefinitelyFalse4 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(DateTime.Now, false);
            Assert.AreEqual(false, t.IsFalse);      
        }
        
        [Test]
        public void IsDefinitelyTrue1 ()
        {
            Tbool t = new Tbool(true);
            Assert.AreEqual(true, t.IsTrue);      
        }
        
        [Test]
        public void IsDefinitelyTrue2 ()
        {
            Tbool t = new Tbool();
            Assert.AreEqual(false, t.IsTrue);      
        }
        
        [Test]
        public void IsDefinitelyTrue3 ()
        {
            Tbool t = new Tbool(false);
            Assert.AreEqual(false, t.IsTrue);      
        }
        
        [Test]
        public void IsDefinitelyTrue4 ()
        {
            Tbool t = new Tbool(true);
            t.AddState(DateTime.Now, false);
            Assert.AreEqual(false, t.IsTrue);      
        }
        
        // Tests for (1) order in which items are evaluated and 
        // (2) correctness of the result
        
        // &&
        
        [Test]
        public void And_TT_Result ()
        {
            Tbool t = MethodT() && MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void And_TT_Order ()
        {
            result = "";
            Tbool t = MethodT() && MethodT();
            Assert.AreEqual("TT", result);      
        }
        
        [Test]
        public void And_TF_Result ()
        {
            Tbool t = MethodT() && MethodF();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_TF_Order ()
        {
            result = "";
            Tbool t = MethodT() && MethodF();
            Assert.AreEqual("TF", result);      
        }
        
        [Test]
        public void And_TU_Result ()
        {
            Tbool t = MethodT() && MethodU();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void And_TU_Order ()
        {
            result = "";
            Tbool t = MethodT() && MethodU();
            Assert.AreEqual("TU", result);      
        }
        
        [Test]
        public void And_FT_Result ()
        {
            Tbool t = MethodF() && MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_FT_Order ()
        {
            result = "";
            Tbool t = MethodF() && MethodT();
            Assert.AreEqual("F", result);      
        }
        
        [Test]
        public void And_FF_Result ()
        {
            Tbool t = MethodF() && MethodF();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_FF_Order ()
        {
            result = "";
            Tbool t = MethodF() && MethodF();
            Assert.AreEqual("F", result);      
        }
        
        [Test]
        public void And_FU_Result ()
        {
            Tbool t = MethodF() && MethodU();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_FU_Order ()
        {
            result = "";
            Tbool t = MethodF() && MethodU();
            Assert.AreEqual("F", result);      
        }
        
        [Test]
        public void And_UT_Result ()
        {
            Tbool t = MethodU() && MethodT();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void And_UT_Order ()
        {
            result = "";
            Tbool t = MethodU() && MethodT();
            Assert.AreEqual("UT", result);      
        }
        
        [Test]
        public void And_UF_Result ()
        {
            Tbool t = MethodU() && MethodF();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_UF_Order ()
        {
            result = "";
            Tbool t = MethodU() && MethodF();
            Assert.AreEqual("UF", result);      
        }
        
        [Test]
        public void And_UU_Result ()
        {
            Tbool t = MethodU() && MethodU();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void And_UU_Order ()
        {
            result = "";
            Tbool t = MethodU() && MethodU();
            Assert.AreEqual("UU", result);      
        }
        
        [Test]
        public void And_TFT_Order ()
        {
            result = "";
            Tbool t = MethodT() && MethodF() && MethodT();
            Assert.AreEqual("TF", result);      
        }
        
        [Test]
        public void And_TFT_Result ()
        {
            Tbool t = MethodT() && MethodF() && MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void And_FTT_Order ()
        {
            result = "";
            Tbool t = MethodF() && MethodT() && MethodT();
            Assert.AreEqual("F", result);      
        }
        
        [Test]
        public void And_FTT_Result ()
        {
            Tbool t = MethodF() && MethodT() && MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        // ||
        
        [Test]
        public void Or_TT_Result ()
        {
            Tbool t = MethodT() || MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void Or_TT_Order ()
        {
            result = "";
            Tbool t = MethodT() || MethodT();
            Assert.AreEqual("T", result);      
        }
        
        [Test]
        public void Or_TF_Result ()
        {
            Tbool t = MethodT() || MethodF();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void Or_TF_Order ()
        {
            result = "";
            Tbool t = MethodT() || MethodF();
            Assert.AreEqual("T", result);      
        }
        
        [Test]
        public void Or_TU_Result ()
        {
            Tbool t = MethodT() || MethodU();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void Or_TU_Order ()
        {
            result = "";
            Tbool t = MethodT() || MethodU();
            Assert.AreEqual("T", result);      
        }
        
        [Test]
        public void Or_FT_Result ()
        {
            Tbool t = MethodF() || MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void Or_FT_Order ()
        {
            result = "";
            Tbool t = MethodF() || MethodT();
            Assert.AreEqual("FT", result);      
        }
        
        [Test]
        public void Or_FF_Result ()
        {
            Tbool t = MethodF() || MethodF();
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", t.TestOutput);      
        }
        
        [Test]
        public void Or_FF_Order ()
        {
            result = "";
            Tbool t = MethodF() || MethodF();
            Assert.AreEqual("FF", result);      
        }
        
        [Test]
        public void Or_FU_Result ()
        {
            Tbool t = MethodF() || MethodU();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void Or_FU_Order ()
        {
            result = "";
            Tbool t = MethodF() || MethodU();
            Assert.AreEqual("FU", result);      
        }
        
        [Test]
        public void Or_UT_Result ()
        {
            Tbool t = MethodU() || MethodT();
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", t.TestOutput);      
        }
        
        [Test]
        public void Or_UT_Order ()
        {
            result = "";
            Tbool t = MethodU() || MethodT();
            Assert.AreEqual("UT", result);      
        }
        
        [Test]
        public void Or_UF_Result ()
        {
            Tbool t = MethodU() || MethodF();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void Or_UF_Order ()
        {
            result = "";
            Tbool t = MethodU() || MethodF();
            Assert.AreEqual("UF", result);      
        }
        
        [Test]
        public void Or_UU_Result ()
        {
            Tbool t = MethodU() || MethodU();
            Assert.AreEqual("Unknown", t.TestOutput);      
        }
        
        [Test]
        public void Or_UU_Order ()
        {
            result = "";
            Tbool t = MethodU() || MethodU();
            Assert.AreEqual("UU", result);      
        }
    }
    
    #pragma warning restore 219
}