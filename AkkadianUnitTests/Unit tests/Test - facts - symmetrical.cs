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
    public class SymmetricalFacts : H
    {
        private static Thing p1 = new Thing("P1");
        private static Thing p2 = new Thing("P2");
        
        // .Sym combos
        
        [Test]
        public void SymTT ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Facts.Assert(p2, "IsMarriedTo", p1, true);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void SymTF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Facts.Assert(p2, "IsMarriedTo", p1, false);                         // contradictory assertion
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);    // what is desired here? (or forbid contradictions)
        }
        
        [Test]
        public void SymTU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void SymFF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Facts.Assert(p2, "IsMarriedTo", p1, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(false , result.Out);       
        }  
        
        [Test]
        public void SymFU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(false , result.Out);       
        }
        
        [Test]
        public void SymUU ()
        {
            Facts.Clear();
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("Unstated", result.Out);       
        }
        
        // .Either - correct result?
        
        [Test]
        public void Either_Result_1 ()
        {
            Facts.Clear();
            Facts.Assert(p1, "Fam.FamilyRelationship", p2, "Biological parent");
            Tbool result = Fam.IsBiologicalParentOf(p1, p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void Either_Result_2 ()
        {
            Facts.Clear();
            Facts.Assert(p2, "Fam.FamilyRelationship", p1, "Biological child");
            Tbool result = Fam.IsBiologicalParentOf(p1, p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void Either_Result_3 ()
        {
            Facts.Clear();
            Tbool result = Fam.IsBiologicalParentOf(p1, p2);
            Assert.AreEqual("Unstated", result.Out);       
        }

        // Some family relationship inputs
        
        [Test]
        public void Fam_1 ()
        {
            Facts.Reset();
            Facts.Assert(p1, "Fam.FamilyRelationship", p2, "Partner by civil union");
            bool result = Fam.InCivilUnion(p1, p2).IsTrue;
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Fam_2 ()
        {
            Facts.Reset();
            Facts.Assert(p2, "Fam.FamilyRelationship", p1, "Partner by civil union");
            bool result = Fam.InCivilUnion(p1, p2).IsTrue;
            Assert.AreEqual(true, result);       
        }
        
        [Test]
        public void Fam_3 ()
        {
            Facts.Reset();
            Facts.Assert(p1, "Fam.FamilyRelationship", p2, "Something other than civil union...");
            bool result = Fam.InCivilUnion(p1, p2).IsTrue;
            Assert.AreEqual(false, result);       
        }
        
        [Test]
        public void Fam_4 ()
        {
            Facts.Reset();
            Facts.Assert(p2, "Fam.FamilyRelationship", p1, "Something other than civil union...");
            bool result = Fam.InCivilUnion(p1, p2).IsTrue;
            Assert.AreEqual(false, result);       
        }
        
    }
    
    #pragma warning restore 219
}
