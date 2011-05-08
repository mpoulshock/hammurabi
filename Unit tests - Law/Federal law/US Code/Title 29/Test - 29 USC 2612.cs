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
//
// NO REPRESENTATION OR WARRANTY IS MADE THAT THIS PROGRAM ACCURATELY REFLECTS
// OR EMBODIES ANY APPLICABLE LAWS, REGULATIONS, RULES OR EXECUTIVE ORDERS 
// ("LAWS"). YOU SHOULD RELY ONLY ON OFFICIAL VERSIONS OF LAWS PUBLISHED BY THE 
// RELEVANT GOVERNMENT AUTHORITY, AND YOU ASSUME THE RESPONSIBILITY OF 
// INDEPENDENTLY VERIFYING SUCH LAWS. THE USE OF THIS PROGRAM IS NOT A 
// SUBSTITUTE FOR THE ADVICE OF AN ATTORNEY.

using Hammurabi;
using NUnit.Framework;
using System;

namespace Hammurabi.UnitTests
{
    /// <summary>
    /// 29 U.S.C. 2612 - FMLA leave.
    /// </summary>
    [TestFixture]
    public class USC_Tit29_Sec2612 : H
    {
        // Simplest scenarios in which a person is/is not eligible for FMLA leave.

        [Test]
        public void Simple1 ()
        {
            /// Instantiate some legal entities
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");
            
            // Assert facts about the entities (here, none)
            Facts.Clear();          

            // Get and evaluate result
            DateTime theDate = new DateTime(2011,4,15);
            Tbool result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).AsOf(theDate);
            Assert.AreEqual("Unknown", result.TestOutput);
        }

        [Test]
        public void Simple2 ()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");

            Facts.Clear();
            Facts.Assert(e, "ReasonForRequestingLeaveFrom",c, "Other");          

            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).AsOf(theDate).ToBool;
            Assert.AreEqual(false, result);
        }
        
        [Test]
        public void Simple3 ()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");

            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c, false);  

            bool? result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).ToBool;
            Assert.AreEqual(false, result);
        }
         
        [Test]
        public void Simple4 ()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");

            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 2000);
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "HoursWorkedInLast12Months",c, 1500);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "PositionAt", c, "Unknown");
            Facts.Assert(e, "ReasonForRequestingLeaveFrom",c, "Other");          

            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).AsOf(theDate).ToBool;
            Assert.AreEqual(false, result);
        }
        
        [Test]
        public void Simple5 ()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");

            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 2000);
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "HoursWorkedInLast12Months",c, 1500);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "PositionAt", c, "Unknown");
            Facts.Assert(e, "ReasonForRequestingLeaveFrom",c, "To adopt a child");          

            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).AsOf(theDate).ToBool;
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void Test_2612aC1_1()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");

            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 2000);
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "HoursWorkedInLast12Months",c, 1500);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "PositionAt", c, "Unknown");
            Facts.Assert(e, "ReasonForRequestingLeaveFrom",c, "To care for family member with a health condition");          

            Tbool result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Test_2612aC1_2()
        {
            Person e = new Person("the employee");
            Corp c = new Corp("the employer");
            Person f = new Person("the family member");

            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 2000);
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "HoursWorkedInLast12Months",c, 1500);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "PositionAt", c, "Unknown");
            Facts.Assert(e, "ReasonForRequestingLeaveFrom",c, "To care for family member with a health condition");
            Facts.Assert(e, "NeedsLeaveToProvideCareFor", f);
            Facts.Assert(e, "IsMarriedTo", f);
            Facts.Assert(f, "HasSeriousHealthCondition");

            DateTime theDate = new DateTime(2011,4,15);
            Tbool result = USC.Tit29.Sec2612.IsEntitledToRegLeaveFrom(e,c).AsOf(theDate);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
    }
}
