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

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class UnresolvedConcepts : H
    { 
        // Fact default assumption issue
        // Should some facts be assumed false?  Not doing so requires a host of 
        // facts to be asserted in order to make the situation explicit...
  
        [Test]
        public void AllXThat2 ()
        {
            Facts.Clear();
            
            Person P1 = new Person("P1");
            Person P3 = new Person("P3");
            Person P4 = new Person("P4");
            
            Facts.Assert(P1, "IsParentOf", P3);
            Facts.Assert(P1, "IsParentOf", P4);
//            Facts.Assert(P1, "IsParentOf", P1, false);  // Test fails when this is toggled out!!!
            
            Tset result = Facts.AllKnownPeople().Filter( _ => IsParentOf(P1,_));
            
            Assert.AreEqual("Time.DawnOf P3, P4 ", 
                            result.TestOutput);
        }
        
        private static Tbool IsParentOf(Person p1, Person p2)
        {
            return Facts.QueryTvar<Tbool>("IsParentOf", p1, p2);
        }
        
        // Fact default assumption issue - another example...
        
        [Test]
        public void UnknownEmploymentRelationship () 
        {
            Facts.Clear();
            Person p = new Person("p");
            Thing c = new Thing("c");
            Thing c2 = new Thing("c2");
            Facts.Assert(p, "EmploymentRelationship", c2, "Employee");
            Tbool result = Econ.IsEmployedBy(p,c);  // returns Unknown b/c this relationship has not been asserted
            Assert.AreEqual("Time.DawnOf False ", result.TestOutput);        
        }
        
        [Test]
        public void UnknownExistenceDueToUnknownFact ()  
        {
            Facts.Clear();
            Person p = new Person("p");
            Thing c = new Thing("c");
            Thing c2 = new Thing("c2");
            Facts.Assert(p, "EmploymentRelationship", c2, "Employee");
            Tbool result = SomeoneWorksAt(c);  // returns Unknown b/c it's unknown whether IsEmployedBy(p,c)
            Assert.AreEqual("Time.DawnOf False ", result.TestOutput);        
        }

        private static Tbool SomeoneWorksAt(Thing c)
        {
            return Facts.AllKnownPeople().Exists ( _ => Econ.IsEmployedBy(_,c));
        }
        
        // When the earlier date is put after the prior date, should DayDiff return a negative number?
        
        [Test]
        public void Tdate_DayDiff_2 ()
        {
            Tdate td1 = new Tdate(2010,1,1);
            Tdate td2 = new Tdate(2000,1,1);
            Tnum result = Tdate.DayDiff(td1,td2);
            Assert.AreEqual("Time.DawnOf 3653 ", result.TestOutput);        
        }
        
        // When making a running count of the true subintervals with in (say) a year, 
        // what to do with the last subinterval before a new year starts...?
        
        [Test]
        public void FT_RunningCountPer_4 ()
        {
            Tbool t = new Tbool();
            t.AddState(Time.DawnOf,false);
            t.AddState(Date(2010,11,1), true);
            t.AddState(Date(2010,12,1), true);
            t.AddState(Date(2011,1,1), true);
            t.AddState(Date(2011,2,1), false);
            Tnum actual = t.RunningCountPer(TheYear);
            string expected = "Time.DawnOf 0 12/1/2010 12:00:00 AM 1 1/1/2011 12:00:00 AM 0 2/1/2011 12:00:00 AM 1 3/1/2011 12:00:00 AM 2 1/1/2012 12:00:00 AM 0 ";
            Assert.AreEqual(expected, actual.TestOutput);      
        }
    }
}


