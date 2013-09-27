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
    public class Hstates : H
    {
        private static string eternallyUnstated = "Unstated";
        private static Tbool tbt = new Tbool(true);
        private static Tbool tbf = new Tbool(false);
        private static Tbool tbu = new Tbool(Hstate.Unstated);
        private static Tset theSet = new Tset(Hstate.Unstated);
        private static Tnum n = new Tnum(Hstate.Unstated);

        private static Tnum tnv()
        {
            Tnum result = new Tnum(Hstate.Stub);
            result.AddState(Date(2001,1,1), Hstate.Uncertain);
            result.AddState(Date(2002,1,1), Hstate.Unstated);
            return result;
        }

        // Math - addition
        
        [Test]
        public void Unknown_Add_1 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = n + n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Add_2 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = tnv() + n2;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - subtraction

        [Test]
        public void Unknown_Subtract_1 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = n - n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Subtract_2 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = tnv() - n2;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - multiplication
        
        [Test]
        public void Unknown_Mult_1 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = n * n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        [Test]
        public void Unknown_Mult_2 ()
        {
            Tnum n2 = new Tnum(0);
            Tnum result = n * n2;
            Assert.AreEqual(0, result.Out);        
        }
        
        [Test]
        public void Unknown_Mult_3 ()
        {
            Tnum result = n * 0;
            Assert.AreEqual(0, result.Out);        
        }

        [Test]
        public void Unknown_Mult_4 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = tnv() * n2;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - div

        [Test]
        public void Unknown_Div_2 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = tnv() / n2;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - modulo

        [Test]
        public void Modulo_1 ()
        {
            Tnum result = 3 % 2;
            Assert.AreEqual(1, result.Out);        
        }

        [Test]
        public void Unknown_Modulo_1 ()
        {
            Tnum result = n % 3;
            Assert.AreEqual("Unstated", result.Out);        
        }

        [Test]
        public void Unknown_Modulo_2 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = tnv() % n2;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - abs
        
        [Test]
        public void Unknown_Abs_1 ()
        {
            Tnum result = Abs(n);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Abs_2 ()
        {
            Tnum result = Abs(tnv());
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - round
        
        [Test]
        public void Unknown_Round_1 ()
        {
            Tnum result = n.RoundUp(2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Round_2 ()
        {
            Tnum result = tnv().RoundToNearest(10);
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        [Test]
        public void Unknown_Round_3 ()
        {
            Tnum result = tnv().RoundUp(10);
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }
        [Test]
        public void Unknown_Round_4 ()
        {
            Tnum result = tnv().RoundDown(10);
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }


        // Math - min
        
        [Test]
        public void Unknown_Min_1 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = Min(n, n2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Min_2 ()
        {
            Tnum result = Min(tnv(),3);
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Math - max
        
        [Test]
        public void Unknown_Max_1 ()
        {
            Tnum n2 = new Tnum(4);
            Tnum result = Max(n, n2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Max_2 ()
        {
            Tnum result = Max(tnv(),3);
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", result.Out);        
        }

        // Tvar.Lean
        
        [Test]
        public void Unknown_Lean_1 ()
        {
            Tbool result = tbu.Lean;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Lean_2 ()
        {
            Tbool t = new Tbool(Hstate.Stub);
            t.AddState(DateTime.Now, Hstate.Stub);
            Assert.AreEqual("Stub", t.Lean.Out);        
        }

        // Tvar.AsOf
        
        [Test]
        public void Unknown_AsOf_1 ()
        {
            Tbool result = tbu.AsOf(DateTime.Now);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        // Tvar.IsAlways / IsEver
        
        [Test]
        public void Unknown_IsAlways_1 ()
        {
            Tbool result = tbu.IsAlwaysTrue();
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        [Test]
        public void Unknown_IsEver_1 ()
        {
            Tbool result = tbu.IsEverTrue();
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        // String concatenation

        [Test]
        public void Unknown_Concat_1 ()
        {
            Tstr ts2 = new Tstr(Hstate.Unstated);
            Tstr ts3 = " x" + ts2;
            Assert.AreEqual(eternallyUnstated, ts3.Out);            
        }

        // Set.AsOf
        
        [Test]
        public void Unknown_SetAsOf_1 ()
        {
            Assert.AreEqual(eternallyUnstated, theSet.AsOf(Time.DawnOf).Out);    
        }
        
        // Set.IsSubsetOf
        
        [Test]
        public void Unknown_Subset_1 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
            Tset s1 = new Tset(P1,P2);    
            Assert.AreEqual(eternallyUnstated, theSet.IsSubsetOf(s1).Out);        
        }
        
        // Set.Contains
        
        [Test]
        public void Unknown_SetContains_1 ()
        {
            Thing P1 = new Thing("P1");
            Assert.AreEqual(eternallyUnstated, theSet.Contains(P1).Out);        
        }
        
        // Set equality
        
        [Test]
        public void Unknown_SetEquality_1 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
            Tset s1 = new Tset(P1,P2);
            Tbool res = s1 == theSet;
            Assert.AreEqual(eternallyUnstated, res.Out);        
        }
        
        [Test]
        public void Unknown_SetEquality_2 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
            Tset s1 = new Tset(P1,P2);
            Tbool res = s1 != theSet;
            Assert.AreEqual(eternallyUnstated, res.Out);        
        }
        
        // IsAtOrBefore
        
        [Test]
        public void Unknown_IsAtOrBefore_1 ()
        {
            Tdate td1 = new Tdate(2000,1,1);
            Tdate td2 = new Tdate(Hstate.Unstated);
            Tbool result = td2 <= td1;
            Assert.AreEqual(eternallyUnstated, result.Out);    
        }

        // Numeric comparison
        
        [Test]
        public void Unknown_NumericComparison1 ()
        {
            Tbool r = tnv() > 7;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison2 ()
        {
            Tbool r = tnv() == 7;
            Assert.AreEqual("{Dawn: Stub; 1/1/2001: Uncertain; 1/1/2002: Unstated}", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison3 ()
        {
            Tbool r = new Tnum(Hstate.Uncertain) == new Tnum(Hstate.Unstated);
            Assert.AreEqual("Uncertain", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison4 ()
        {
            Tbool r = new Tnum(Hstate.Uncertain) == new Tnum(Hstate.Uncertain);
            Assert.AreEqual("Uncertain", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison5 ()
        {
            Tbool r = new Tnum(Hstate.Stub) == new Tnum(Hstate.Uncertain);
            Assert.AreEqual("Stub", r.Out);    
        }

        // Unknown entity instances (that are function arguments)

        [Test]
        public void Unknown_EntInst1 ()
        {
            Thing p = new Thing("");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p));    
        }

        [Test]
        public void Unknown_EntInst2 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p));    
        }

        [Test]
        public void Unknown_EntInst3 ()
        {
            Thing p1 = new Thing("");
            Thing p2 = new Thing("Jane");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst4 ()
        {
            Thing p1 = new Thing("Jim");
            Thing p2 = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst5 ()
        {
            Thing p1 = new Thing("");
            Thing p2 = new Thing("");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst6 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p,"someString"));    
        }

        [Test]
        public void Unknown_EntInst7 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p,new Tstr("")));    
        }
    }
}