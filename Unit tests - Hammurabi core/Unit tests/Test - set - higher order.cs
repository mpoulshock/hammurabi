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
using System;
using System.Collections.Generic;

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
        private static Thing M = new Thing("M");
        private static Thing A = new Thing("A");
        private static Thing B = new Thing("B");
        private static Thing C = new Thing("C");
        private static Thing c = new Thing("corp");
        
        // Set up a new test
        private static void NewTest()
        {
            Tnum valA = new Tnum(1);
            
            Tnum valB = new Tnum(2);
            
            Tnum valC = new Tnum(3);
            valC.AddState(new DateTime(2011,1,14), 4);
            
            Tbool ownA = new Tbool(true);
            
            Tbool ownB = new Tbool(true);
            ownB.AddState(new DateTime(2008,1,1), false);
            
            Tbool ownC = new Tbool(false);
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
        private static Tnum AssetValue(Thing asset)
        {
            return Facts.QueryTvar<Tnum>("ValueOf", asset);
        }
        private static Tbool AssetValueLessThan4(Thing asset)    
        { 
            return AssetValue(asset) < 4;
        }
        private static Tbool AssetValueIndeterminacy(Thing asset)    
        { 
            return new Tbool(Hstate.Unstated);        // used to test unknowns
        }
        private static Tbool Owns(Thing p, Thing r)
        {
            return Facts.QueryTvar<Tbool>("Owns", p, r);
        }
        private static Tbool IsParentOf(Thing p1, Thing p2)
        {
            return Facts.QueryTvar<Tbool>("IsParentOf", p1, p2);
        }
        private static Tset TheThings()
        {
            return new Tset(A,B,C);
        }
        
        // Filter
    
        [Test]
        public void Filter1 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter(_ => Owns (M,_));
            Assert.AreEqual("{Dawn: A, B; 1/1/2005: A, B, C; 1/1/2008: A, C}", theAssets.Out);        
        }
        
        [Test]
        public void Filter2 ()
        {
            Facts.Clear();
            
            Thing P1 = new Thing("P1");
            Thing P3 = new Thing("P3");
            Thing P4 = new Thing("P4");
            
            Facts.Assert(P1, "IsParentOf", P3, true);
            Facts.Assert(P1, "IsParentOf", P4, true);
            Facts.Assert(P1, "IsParentOf", P1, false);  // An absurd thing to have to assert

            Tset people = new Tset(P1,P3,P4);
            Tset result = people.Filter( _ => IsParentOf(P1,_));
            
            Assert.AreEqual("P3, P4", result.Out);
        }
        
        [Test]
        public void Filter3 ()
        {
            Facts.Clear();

            Thing P1 = new Thing("P1");
            Thing P3 = new Thing("P3");
            Thing P4 = new Thing("P4");

            Tbool tb1 = new Tbool(false);
            tb1.AddState(new DateTime(2005,12,20),true);
            
            Tbool tb2 = new Tbool(false);
            tb2.AddState(new DateTime(2008,3,8),true);
            
            Facts.Assert(P1, "IsParentOf", P3, tb1);
            Facts.Assert(P1, "IsParentOf", P4, tb2);
            Facts.Assert(P1, "IsParentOf", P1, false);  // An absurd thing to have to assert

            Tset people = new Tset(P1,P3,P4);
            Tset result = people.Filter( _ => IsParentOf(P1,_));
            
            Assert.AreEqual("{Dawn: ; 12/20/2005: P3; 3/8/2008: P3, P4}", result.Out);
        }
        
        [Test]
        public void Filter4 ()
        {
            Facts.Clear();
            
            Thing P1 = new Thing("P1");
            Thing P3 = new Thing("P3");
            Thing P4 = new Thing("P4");
            
            Facts.Assert(P1, "IsParentOf", P3, true);
            Facts.Assert(P1, "IsParentOf", P4, true);
            Facts.Assert(P4, "IsParentOf", P3, false);
            Facts.Assert(P3, "IsParentOf", P3, false);

            Tset people = new Tset(P1,P3,P4);
            Tset result = people.Filter( _ => IsParentOf(_,P3));
            Assert.AreEqual("P1", result.Out);
        }
        
        [Test]
        public void Filter5a ()
        {
            Facts.Clear();
            Thing P1 = new Thing("P1");
            Tset people = new Tset(P1);
            Tset result = people.Filter( _ => IsParentOf(_,P1));
            Assert.AreEqual("Unstated", result.Out); // Compare with the test Filter5b below
        }

        [Test]
        public void Filter5b ()
        {
            Facts.Clear();
            Thing P1 = new Thing("P1");
            Tset people = new Tset(P1);
            Facts.Assert(P1, "IsParentOf", P1, false);
            Tset result = people.Filter( _ => IsParentOf(_,P1));
            Assert.AreEqual("", result.Out);
        }

        [Test]
        public void Filter6 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tset cheapAssets = theAssets.Filter(x => AssetValueLessThan4((Thing)x));            
            Assert.AreEqual("{Dawn: A, B; 1/1/2005: A, B, C; 1/1/2008: A, C; 1/14/2011: A}", cheapAssets.Out);        
        }

        [Test]
        public void Filter7_Unknown ()
        {
            NewTest();
            Tbool areAnyCheapAssets = TheThings().Exists(x => AssetValueIndeterminacy(x));
            Assert.AreEqual("Unstated", areAnyCheapAssets.Out);
        }

        // Tset.Count

        [Test]
        public void CountUnknown1 ()
        {
            Thing P1 = new Thing("P1");
            Tset tsv = new Tset(Hstate.Stub);
            tsv.AddState(Date(2000,01,01),P1);
            tsv.AddState(Date(2001,01,01),Hstate.Uncertain);
            Assert.AreEqual("{Dawn: Stub; 1/1/2000: 1; 1/1/2001: Uncertain}", tsv.Count.Out);
        }

        // AllKnownPeople
        
        [Test]
        public void AllKnownPeople1 ()
        {
            Facts.Clear();
            
            Thing P1 = new Thing("P1");
            Thing P3 = new Thing("P3");
            Thing P4 = new Thing("P4");

            Facts.Assert(P1, "IsParentOf", P3, new Tbool(true));
            Facts.Assert(P1, "IsParentOf", P4, new Tbool(true));

            Assert.AreEqual("P1, P3, P4", Facts.AllKnownPeople().Out);
        }
        
        [Test]
        public void AllKnownPeople2 ()
        {
            Facts.Clear();
            Tset result = Facts.AllKnownPeople();
            Assert.AreEqual("", result.Out);
        }
        
        [Test]
        public void AllKnownPeople3 ()
        {
            Facts.Clear();
            
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
            Thing P3 = new Thing("P3");

            Facts.Assert(P1, "IsMarriedTo", P2, true);
            Facts.Assert(P3, "IsPermanentlyAndTotallyDisabled", new Tbool(false));
            Tset result = Facts.AllKnownPeople();
            Assert.AreEqual("P1, P2, P3", result.Out);
        }
        
        [Test]
        public void AllKnownPeople4 ()
        {
            Facts.Clear();
            Thing P1 = new Thing("P1");
            Facts.Assert(P1,"Gender", "Male");
            Tset result = Facts.AllKnownPeople();
            Assert.AreEqual("P1", result.Out);
        }
        
        // Sum
    
        [Test]
        public void Set_Sum1 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tnum sumOfAssets = theAssets.Sum(x => AssetValue((Thing)x));
            Assert.AreEqual("{Dawn: 3; 1/1/2005: 6; 1/1/2008: 4; 1/14/2011: 5}", sumOfAssets.Out);        
        }
        
        [Test]
        public void Set_Sum2 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tnum sumOfAssets = theAssets.Sum(x => AssetValue((Thing)x));
            Assert.AreEqual("{Dawn: 3; 1/1/2005: 6; 1/1/2008: 4; 1/14/2011: 5}", sumOfAssets.Out);        
        }
        
        [Test]
        public void Set_Sum_Unknown_1 ()
        {
            NewTest();
            Tset theAssets = new Tset(Hstate.Unstated);
            Tnum sumOfAssets = theAssets.Sum(x => AssetValue((Thing)x));
            Assert.AreEqual("Unstated", sumOfAssets.Out);        
        }
        
        [Test]
        public void Set_Sum_Unknown_2 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tnum sumOfAssets = theAssets.Sum(x => NullFcn(x) );
            Assert.AreEqual("Stub", sumOfAssets.Out);    
        }
        private static Tnum NullFcn(Thing p)
        {
            return new Tnum(Hstate.Stub);
        }
        
        // Exists
    
        [Test]
        public void Exists_1 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tbool areAnyCheapAssets = theAssets.Exists(x => AssetValueLessThan4(x));
            Assert.AreEqual(true, areAnyCheapAssets.Out);        
        }
        
        [Test]
        public void Exists_2_Unknown ()
        {
            NewTest();
            Tset theAssets = new Tset(Hstate.Unstated);
            Tbool areAnyCheapAssets = theAssets.Exists(x => AssetValueLessThan4(x));
            Assert.AreEqual("Unstated", areAnyCheapAssets.Out);        
        }
        
        [Test]
        public void Exists_3_Unknown ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tbool areAnyCheapAssets = theAssets.Exists(x => AssetValueIndeterminacy(x));
            Assert.AreEqual("Unstated", areAnyCheapAssets.Out);
        }
        
        // ForAll
    
        [Test]
        public void ForAll1 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tbool allAssetsAreCheap = theAssets.ForAll( x => AssetValueLessThan4((Thing)x));
            Assert.AreEqual("{Dawn: true; 1/14/2011: false}", allAssetsAreCheap.Out);        
        }
  
        // OrderBy

        [Test]
        public void OrderBy1 ()
        {
            NewTest();
            Tset theAssets = TheThings().Filter( _ => Owns (M,_));
            Tset orderedAssets = theAssets.OrderBy(x => AssetValue((Thing)x) * -1);            
            Assert.AreEqual("{Dawn: B, A; 1/1/2005: C, B, A; 1/1/2008: C, A}", orderedAssets.Out);        
        }

        // Functions compiled from Akkadian to C#
        
        [Test]
        public void Compiled1a ()
        {
            Facts.Clear();
            Tbool result = AllAreMale(new Tset(new List<Thing>()));
            Assert.AreEqual(true, result.Out);        
        }
        
        [Test]
        public void Compiled1b ()
        {
            Facts.Clear();
            Thing p = new Thing("p");
            Facts.Assert(p,"Gender","Male");
            Tbool result = AllAreMale(new Tset(p));
            Assert.AreEqual(true, result.Out);        
        }
        
        [Test]
        public void Compiled1c ()
        {
            Facts.Clear();
            Thing p1 = new Thing("p1");
            Thing p2 = new Thing("p2");
            Facts.Assert(p1,"Gender","Male");
            Facts.Assert(p2,"Gender","Male");
            Tbool result = AllAreMale(new Tset(p1,p2));
            Assert.AreEqual(true, result.Out);        
        }
        
        [Test]
        public void Compiled1d ()
        {
            Facts.Clear();
            Thing p1 = new Thing("p1");
            Thing p2 = new Thing("p2");
            Facts.Assert(p1,"Gender","Male");
            Facts.Assert(p2,"Gender","Female");
            Tbool result = AllAreMale(new Tset(p1,p2));
            Assert.AreEqual(false, result.Out);        
        }
        
        private static Tbool AllAreMale(Tset theSet)
        {
            return theSet.ForAll( _ => IsMale(_));
        }
        
        // ForAll using a filter method with two parameters
        
        [Test]
        public void Compiled2a ()
        {
            Facts.Clear();
            Thing c = new Thing("corp");
            Tbool result = SomeoneWorksAt(c, new Tset(new List<Thing>()));
            Assert.AreEqual(false, result.Out);        
        }
        
        [Test]
        public void Compiled2b ()
        {
            // What conclusion to draw when no relationship is expressed? "Unstated"
            Facts.Clear();
            Thing c = new Thing("corp");
            Thing p = new Thing("p");
            Tbool result = SomeoneWorksAt(c, new Tset(p));
            Assert.AreEqual("Unstated", result.Out);        
        }
        
        [Test]
        public void Compiled2c ()
        {
            Facts.Clear();
            Thing c = new Thing("corp");
            Thing p = new Thing("p");
            Facts.Assert(p, "Econ.EmploymentRelationship", c, "Employee");
            Tbool result = SomeoneWorksAt(c, new Tset(p));
            Assert.AreEqual(true, result.Out);        
        }
        
        [Test]
        public void Compiled2d ()
        {
            Facts.Clear();
            Thing c = new Thing("corp");
            Thing p = new Thing("p");
            Facts.Assert(p, "Econ.EmploymentRelationship", c, "Intern");
            Tbool result = SomeoneWorksAt(c, new Tset(p));
            Assert.AreEqual(false, result.Out);        
        }

        [Test]
        public void Compiled2g ()
        {
            Facts.Clear();
            Thing p = new Thing("p");
            Thing c = new Thing("c");
            Thing c2 = new Thing("c2");
            Facts.Assert(p, "Econ.EmploymentRelationship", c2, "Employee");
            Tbool result = Econ.IsEmployedBy(p,c2);
            Assert.AreEqual(true, result.Out);        
        }
        
        [Test]
        public void Compiled2h ()
        {
            Facts.Clear();
            Thing p = new Thing("person");
            Thing c = new Thing("c");
            Thing c2 = new Thing("c2");
            Facts.Assert(p, "Econ.EmploymentRelationship", c2, "Employee");
            Facts.Assert(p, "Econ.EmploymentRelationship", c, "Intern");
            Tbool result = SomeoneWorksAt(c, new Tset(p));  
            Assert.AreEqual(false, result.Out);        
        }
        
        private static Tbool SomeoneWorksAt(Thing c, Tset theSet)
        {
            return theSet.Exists( _ => Econ.IsEmployedBy(_,c));
        }
        
    }
}
