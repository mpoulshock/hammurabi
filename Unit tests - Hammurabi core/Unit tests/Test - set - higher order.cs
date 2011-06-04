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
    [TestFixture]
    public class SetHigherOrder : H
    {        
        /*
             * Three assets whose values and 
             * ownership changes over time:
             * 
             * M "Owns" A        ----------
             * M "Owns" B        -----
             * M "Owns" C           -------
             * A "ValueOf"       1111111111
             * B "ValueOf"       2222222222 
             * C "ValueOf"       3333333344 
             * 
             * value M's assts = 3336644455
             */
        
        // Some legal entities
        private static Person M = new Person("M");
        private static Property A = new Property("A");
        private static Property B = new Property("B");
        private static Property C = new Property("C");
        
        // Set up a new test
        private static void NewTest()
        {
            Tnum valA = new Tnum();
            valA.AddState(Time.DawnOf,1);
            
            Tnum valB = new Tnum();
            valB.AddState(Time.DawnOf,2);
            
            Tnum valC = new Tnum();
            valC.AddState(Time.DawnOf,3);
            valC.AddState(new DateTime(2011,1,14), 4);
            
            Tbool ownA = new Tbool(true);
            
            Tbool ownB = new Tbool();
            ownB.AddState(Time.DawnOf, true);
            ownB.AddState(new DateTime(2008,1,1), false);
            
            Tbool ownC = new Tbool();
            ownC.AddState(Time.DawnOf, false);
            ownC.AddState(new DateTime(2005,1,1), true);
            
            Facts.Clear();
            Facts.Assert(A, "ValueOf", valA);
            Facts.Assert(B, "ValueOf", valB);
            Facts.Assert(C, "ValueOf", valC);
            Facts.Assert(M, "Owns", A, ownA);
            Facts.Assert(M, "Owns", B, ownB);
            Facts.Assert(M, "Owns", C, ownC);
        }
        
        // "Pretend" functions for testing purposes
        private static Tnum AssetValue(Property asset)
        {
            return Input.Tnum(asset,"ValueOf");
        }
        private static Tbool AssetValueLessThan4(Property asset)    
        { 
            return AssetValue(asset) < 4;
        }
        private static Tbool AssetValueIndeterminacy(Property asset)    
        { 
            return new Tbool();        // used to test unknowns
        }
        
        // AllXThat
    
        [Test]
        public void AllXThat1 ()
        {
            NewTest();
            Tset theAssets      = Facts.AllXThat(M,"Owns");
            Assert.AreEqual("1/1/0001 12:00:00 AM A, B 1/1/2005 12:00:00 AM A, B, C 1/1/2008 12:00:00 AM A, C ", 
                            theAssets.TestOutput);        
        }
        
        [Test]
        public void AllXThat2 ()
        {
            Facts.Clear();
            
            Person P1 = new Person("P1");
            Person P3 = new Person("P3");
            Person P4 = new Person("P4");

            Tbool tb1 = new Tbool();
            tb1.AddState(Time.DawnOf, false);
            tb1.AddState(new DateTime(2005,12,20),true);
            
            Tbool tb2 = new Tbool();
            tb2.AddState(Time.DawnOf, false);
            tb2.AddState(new DateTime(2008,3,8),true);
            
            Facts.Assert(P1, "IsParentOf", P3, tb1);
            Facts.Assert(P1, "IsParentOf", P4, tb2);
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 12/20/2005 12:00:00 AM P3 3/8/2008 12:00:00 AM P3, P4 ", 
                            Facts.AllXThat(P1,"IsParentOf").TestOutput);
        }
        
        // AllXSuchThatX
        
        [Test]
        public void AllXSuchThatX1 ()
        {
            Facts.Clear();
            
            Person P1 = new Person("P1");
            Person P3 = new Person("P3");
            Person P4 = new Person("P4");

            Tbool tb1 = new Tbool();
            tb1.AddState(Time.DawnOf, false);
            tb1.AddState(new DateTime(2005,12,20),true);
            
            Tbool tb2 = new Tbool();
            tb2.AddState(Time.DawnOf, false);
            tb2.AddState(new DateTime(2008,3,8),true);
            
            Facts.Assert(P1, "IsParentOf", P3, tb1);
            Facts.Assert(P1, "IsParentOf", P4, tb2);
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 12/20/2005 12:00:00 AM P1, P2 ", 
                            Facts.AllXSuchThatX("IsParentOf",P1).TestOutput);
        }
        
        [Test]
        public void AllXSuchThatX2 ()
        {
            Facts.Clear();
            Person P1 = new Person("P1");
            Tset result = Facts.AllXSuchThatX("IsParentOf",P1);
            Assert.AreEqual("1/1/0001 12:00:00 AM ", result.TestOutput);
        }
        
        // AllXSymmetrical
        
        [Test]
        public void AllXSymmetrical1 ()
        {
            Facts.Clear();
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Facts.Assert(P1,"IsMarriedTo",P2);
            Tset theSpouses = Facts.AllXSymmetrical(P1,"IsMarriedTo");    
            Assert.AreEqual("1/1/0001 12:00:00 AM P2 ", theSpouses.TestOutput);
        }
        
        [Test]
        public void AllXSymmetrical2 ()
        {
            Facts.Clear();
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Person P3 = new Person("P3");
            Facts.Assert(P2,"IsMarriedTo",P1);
            Facts.Assert(P1,"IsMarriedTo",P3);
            Tset theSpouses = Facts.AllXSymmetrical(P1,"IsMarriedTo");    
            Assert.AreEqual("1/1/0001 12:00:00 AM P2, P3 ", theSpouses.TestOutput);
        }
        
        // AllKnownPeople
        
        [Test]
        public void AllKnownPeople1 ()
        {
            Facts.Clear();
            
            Person P1 = new Person("P1");
            Person P3 = new Person("P3");
            Person P4 = new Person("P4");

            Facts.Assert(P1, "IsParentOf", P3, new Tbool(true));
            Facts.Assert(P1, "IsParentOf", P4, new Tbool(true));
            
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P3, P4 ", 
                            Facts.AllKnownPeople().TestOutput);
        }
        
        [Test]
        public void AllKnownPeople2 ()
        {
            Facts.Clear();
            Tset result = Facts.AllKnownPeople();
            Assert.AreEqual("1/1/0001 12:00:00 AM ", result.TestOutput);
        }
        
        [Test]
        public void AllKnownPeople3 ()
        {
            Facts.Clear();
            
            Person P1 = new Person("P1");
            Person P2 = new Person("P2");
            Person P3 = new Person("P3");

            Facts.Assert(P1,"IsMarriedTo",P2);
            Facts.Assert(P3, "IsPermanentlyAndTotallyDisabled", new Tbool(false));
            Tset result = Facts.AllKnownPeople();
            Assert.AreEqual("1/1/0001 12:00:00 AM P1, P2, P3 ", result.TestOutput);
        }
        
        // Sum
    
        [Test]
        public void Set_Sum ()
        {
            NewTest();
            Tset theAssets      = Facts.AllXThat(M,"Owns");
            Tnum sumOfAssets = Sum(theAssets, x => AssetValue((Property)x));
            Assert.AreEqual("1/1/0001 12:00:00 AM 3 1/1/2005 12:00:00 AM 6 1/1/2008 12:00:00 AM 4 1/14/2011 12:00:00 AM 5 ", 
                            sumOfAssets.TestOutput);        
        }
        
        [Test]
        public void Set_Sum_Unknown_1 ()
        {
            NewTest();
            Tset theAssets      = new Tset();
            Tnum sumOfAssets = Sum(theAssets, x => AssetValue((Property)x));
            Assert.AreEqual("Unknown", sumOfAssets.TestOutput);        
        }
        
        [Test]
        public void Set_Sum_Unknown_2 ()
        {
            NewTest();
            Tset theAssets      = Facts.AllXThat(M,"Owns");
            Tnum sumOfAssets = Sum(theAssets, x => { return new Tnum();} );
            Assert.AreEqual("Unknown", sumOfAssets.TestOutput);    
        }
        
        // Filter
    
        [Test]
        public void Test3 ()
        {
            NewTest();
            Tset theAssets      = Facts.AllXThat(M,"Owns");
            Tset cheapAssets = Filter(theAssets, x => AssetValueLessThan4((Property)x));            
            Assert.AreEqual("1/1/0001 12:00:00 AM A, B 1/1/2005 12:00:00 AM A, B, C 1/1/2008 12:00:00 AM A, C 1/14/2011 12:00:00 AM A ", 
                            cheapAssets.TestOutput);        
        }
        
        // Exists
    
        [Test]
        public void Exists_1 ()
        {
            NewTest();
            Tset theAssets = Facts.AllXThat(M,"Owns");
            Tbool areAnyCheapAssets = Exists(theAssets, x => AssetValueLessThan4((Property)x));
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", areAnyCheapAssets.TestOutput);        
        }
        
        [Test]
        public void Exists_2_Unknown ()
        {
            NewTest();
            Tset theAssets = new Tset();
            Tbool areAnyCheapAssets = Exists(theAssets, x => AssetValueLessThan4((Property)x));
            Assert.AreEqual("Unknown", areAnyCheapAssets.TestOutput);        
        }
        
        [Test]
        public void Exists_3_Unknown ()
        {
            NewTest();
            Tset theAssets = Facts.AllXThat(M,"Owns");
            Tbool areAnyCheapAssets = Exists(theAssets, x => AssetValueIndeterminacy((Property)x));
            Assert.AreEqual("Unknown", areAnyCheapAssets.TestOutput);
        }
        
        // ForAll
    
        [Test]
        public void Test5 ()
        {
            NewTest();
            Tset theAssets = Facts.AllXThat(M,"Owns");
            Tbool allAssetsAreCheap = ForAll(theAssets, x => AssetValueLessThan4((Property)x));
            Assert.AreEqual("1/1/0001 12:00:00 AM True 1/14/2011 12:00:00 AM False ", allAssetsAreCheap.TestOutput);        
        }

        
    }
}
