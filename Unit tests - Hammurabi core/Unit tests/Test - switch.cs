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
    public class TestSwitch : H
    {
        private static Tbool tbt = new Tbool(true);
        private static Tbool tbf = new Tbool(false);
        
        // Switch<Tnum>
        
        [Test]
        public void TnumSwitch1_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> tbf, ()=> new Tnum(41),
                                 ()=> tbt, ()=> new Tnum(42),
                                 ()=> 43);
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch2_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> tbt, ()=> new Tnum(41),
                                 ()=> tbf, ()=> new Tnum(42),
                                 ()=> 43);
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 41 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch3_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> tbf, ()=> 41,
                                 ()=> tbt, ()=> 42,
                                 ()=> new Tnum(43));
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch4_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> false, ()=> 41,
                                 ()=> true, ()=> 42,
                                 ()=> new Tnum(43)); 
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch5_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> tbf, ()=> new Tnum(41),
                                 ()=> tbf, ()=> new Tnum(42),
                                 ()=> 43);

            Assert.AreEqual("1/1/0001 12:00:00 AM 43 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch6_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> tbf, ()=> new Tnum(41),
                                 ()=> tbf, ()=> new Tnum(42),
                                 ()=> new Tnum(Hstate.Uncertain));  

            Assert.AreEqual("1/1/0001 12:00:00 AM Uncertain ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch7_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> false, ()=> 41,
                                 ()=> true, ()=> new Tnum(Hstate.Uncertain),
                                 ()=> 42);   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM Uncertain ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch8_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> false, ()=> 41,
                                 ()=> new Tbool(Hstate.Uncertain), ()=> 101,
                                 ()=> 42);   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM Uncertain ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch9_lazy ()
        {
            Tnum result = Switch<Tnum>(()=> false, ()=> 41,
                                 ()=> new Tbool(Hstate.Uncertain), ()=> new Tnum(Hstate.Stub),
                                 ()=> 42);   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM Uncertain ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch10_lazy ()
        {
            Tnum x = new Tnum(10);
            x.AddState(Time.DawnOf.AddYears(5), 1);
            
            Tnum result = Switch<Tnum>(()=> x <= 1, ()=> new Tnum(1),
                                 ()=> 2);   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 2 1/1/0006 12:00:00 AM 1 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch11_lazy ()
        {
            Tnum x = new Tnum(10);
            x.AddState(Time.DawnOf.AddYears(5), 1);
            
            Tnum result = Switch<Tnum>(()=> x >= 5, ()=> new Tnum(1),
                                 ()=> 2);   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 1 1/1/0006 12:00:00 AM 2 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch12_lazy ()
        {
            Tbool tb = new Tbool(true);
            tb.AddState(Date(2000,1,1), false);
            
            Tnum result = Switch<Tnum>(()=> tb, ()=> new Tnum(41),
                                 ()=> true, ()=> new Tnum(42),
                                 ()=> new Tnum(43));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 41 1/1/2000 12:00:00 AM 42 ", result.TestOutput);        
        }

        [Test]
        public void TnumSwitch13_lazy ()  // Tests whether irrelevant values are skipped/ignored
        {
            Tbool tb1 = new Tbool(true);
            tb1.AddState(Date(2000,1,1), false);

            Tbool tb2 = new Tbool(false);
            tb2.AddState(Date(1900,1,1),true);  // true but irrelevant b/c interval subsubmed by tb1
            tb2.AddState(Date(1912,1,1),false);

            Tnum result = Switch<Tnum>(()=> tb1, ()=> new Tnum(41),
                                 ()=> tb2, ()=> new Tnum(42),
                                 ()=> new Tnum(43));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 41 1/1/2000 12:00:00 AM 43 ", result.TestOutput);        
        }

        // Switch<Tbool>

        [Test]
        public void TboolSwitch1_lazy ()
        {
            Tbool result = Switch<Tbool>(()=> false, ()=> true,
                                  ()=> true, ()=> true, 
                                  ()=> new Tbool(false));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);        
        }

        [Test]
        public void TboolSwitch2_lazy ()
        {
            Tbool result = Switch<Tbool>(()=> false, ()=> false,
                                  ()=> true, ()=> true,
                                  ()=> new Tbool(Hstate.Uncertain));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);        
        }

        [Test]
        public void TboolSwitch3_lazy () 
        {
            Tbool result = Switch<Tbool>(()=> false, ()=> true,
                                  ()=> new Tbool(true));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);        
        }

        // Switch<Tstr>

        [Test]
        public void TstrSwitch1_lazy ()
        {
            Tstr result = Switch<Tstr>(()=> false, ()=> "41",
                                  ()=> true, ()=> "42",
                                  ()=> new Tstr("43"));   
            
            Assert.AreEqual("1/1/0001 12:00:00 AM 42 ", result.TestOutput);        
        }

        // ConditionalAssignment

        [Test]
        public void ConditionalAssignment1 ()
        {
            Tnum tn = new Tnum(4);
            Tbool tb = Tbool.MakeTbool(Time.DawnOf, Hstate.Unstated,
                                       Date(2000,1,1), true,
                                       Date(2001,1,1), false,
                                       Date(2002,1,1), true,
                                       Date(2003,1,1), Hstate.Stub);
            Tnum result = Util.ConditionalAssignment<Tnum>(tb,tn);
            Assert.AreEqual("1/1/0001 12:00:00 AM Null 1/1/2000 12:00:00 AM 4 1/1/2001 12:00:00 AM Null 1/1/2002 12:00:00 AM 4 1/1/2003 12:00:00 AM Null ", result.TestOutput);        
        }

        // MergeTvars

//        [Test]
//        public void MergeTvars1a () // This fails due to an issue w/ MakeTnum
//        {
//            Hval unst = new Hval(Hstate.Unstated);
//            Tnum tn1 = Tnum.MakeTnum(Time.DawnOf, unst,
//                                     Date(2000,1,1), 1,
//                                     Date(2001,1,1), unst,
//                                     Date(2002,1,1), 3,
//                                     Date(2003,1,1), unst);
//            Tnum tn2 = Tnum.MakeTnum(Time.DawnOf, 0,
//                                     Date(2000,1,1), unst,
//                                     Date(2001,1,1), 2,
//                                     Date(2002,1,1), unst,
//                                     Date(2003,1,1), unst);
//            Tnum result = MergeTvars<Tnum>(tn1,tn2);
//            Assert.AreEqual("1/1/0001 12:00:00 AM 0 1/1/2000 12:00:00 AM 1 1/1/2001 12:00:00 AM 2 1/1/2002 12:00:00 AM 3 1/1/2003 12:00:00 AM Unstated ", result.TestOutput);        
//        }

        [Test]
        public void MergeTvars1b ()
        {
            Hval unst = new Hval(null,Hstate.Null);

            Tnum tn1 = new Tnum(unst);
            tn1.AddState(Date(2000,1,1), 1);
            tn1.AddState(Date(2001,1,1), unst);
            tn1.AddState(Date(2002,1,1), 3);
            tn1.AddState(Date(2003,1,1), unst);

            Tnum tn2 = new Tnum(0);
            tn2.AddState(Date(2000,1,1), unst);
            tn2.AddState(Date(2001,1,1), 2);
            tn2.AddState(Date(2002,1,1), unst);
            tn2.AddState(Date(2003,1,1), unst);

            Tnum result = Util.MergeTvars<Tnum>(tn1,tn2);
            Assert.AreEqual("1/1/0001 12:00:00 AM 0 1/1/2000 12:00:00 AM 1 1/1/2001 12:00:00 AM 2 1/1/2002 12:00:00 AM 3 1/1/2003 12:00:00 AM Null ", result.TestOutput);        
        }

        [Test]
        public void MergeTvars2 ()  // Merge unknown Tvar w/ mixed (known and unknown) Tvar
        {
            Hval unst = new Hval(null,Hstate.Null);

            Tnum tn1 = new Tnum(unst);

            Tnum tn2 = new Tnum(0);
            tn2.AddState(Date(2000,1,1), unst);
            tn2.AddState(Date(2001,1,1), 2);
            tn2.AddState(Date(2002,1,1), unst);

            Assert.AreEqual(tn2.TestOutput, Util.MergeTvars<Tnum>(tn1,tn2).TestOutput);        
        }

        //  HasUndefinedValues

        [Test]
        public void HasUndefinedIntervals1 ()
        {
            Tnum r = new Tnum(new Hval(null,Hstate.Null));
            r.AddState(Date(2000,1,1), new Hval(null,Hstate.Unstated));
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(true, Util.HasUndefinedIntervals(r));        
        }

        [Test]
        public void HasUndefinedIntervals2 ()
        {
            Tnum r = new Tnum(7);
            r.AddState(Date(2000,1,1), new Hval(null,Hstate.Unstated));
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(false, Util.HasUndefinedIntervals(r));        
        }

        [Test]
        public void HasUndefinedIntervals3 ()  
        {
            Tnum r = new Tnum(7);
            r.AddState(Date(2000,1,1), 6);
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(false, Util.HasUndefinedIntervals(r));        
        }

        // Inner components of Switch

        [Test]
        public void AssignAndMerge1 ()
        {
            Tbool newCondition = new Tbool(Hstate.Uncertain);
            Tnum initialResult = new Tnum(Hstate.Null);
            Tbool newConditionIsUnknown = new Tbool(true);

            Tnum result = Util.MergeTvars<Tnum>(initialResult,
                                         Util.ConditionalAssignment<Tnum>(newConditionIsUnknown, newCondition));

            Assert.AreEqual("1/1/0001 12:00:00 AM Uncertain ", result.TestOutput); 
        }

        [Test]
        public void AssignAndMerge2 ()
        {
            Tbool newCondition = new Tbool(Hstate.Uncertain);
            Tnum initialResult = new Tnum(Hstate.Null);
            Tbool newConditionIsUnknown = new Tbool(true);

            Tnum result = Util.MergeTvars<Tnum>(initialResult,
                                         Util.ConditionalAssignment<Tnum>(newConditionIsUnknown, newCondition));

            Assert.AreEqual(false, Util.HasUndefinedIntervals(result)); 
        }

        [Test]
        public void AssignAndMerge3 ()
        {
            Tbool newCondition = new Tbool(Hstate.Uncertain);
            Tbool newConditionIsUnknown = Util.HasUnknownState(newCondition);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", newConditionIsUnknown.TestOutput); 
        }
    }
}