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
    /// 29 U.S.C. 2611 - FMLA definitions.
    /// </summary>
    [TestFixture]
    public class USC_Tit29_Sec2611 : H
    {
        private static Person e = new Person("the employee");
        private static Corp c = new Corp("the employer");
        
        // Covered employer
        
        [Test]
        public void Covered_Employer_1 ()
        {
            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 2000);         

            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2611.IsCoveredEmployer(c).AsOf(theDate).ToBool;
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void Covered_Employer_2 ()
        {
            Govt c = new Govt("the government agency");
            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 3);         

            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2611.IsCoveredEmployer(c).AsOf(theDate).ToBool;
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void Covered_Employer_3 ()
        {
            Facts.Clear();          
            DateTime theDate = new DateTime(2011,4,15);
            Tbool result = USC.Tit29.Sec2611.IsCoveredEmployer(c).AsOf(theDate);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Not_Covered_Employer ()
        {
            Facts.Clear();
            Facts.Assert(c, "NumberOfEmployees", 3);         
            DateTime theDate = new DateTime(2011,4,15);
            bool? result = USC.Tit29.Sec2611.IsCoveredEmployer(c).AsOf(theDate).ToBool;
            Assert.AreEqual(false, result);
        }
        
        // Eligible employee
        
        [Test]
        public void Eligible_Employee_1 ()
        {
            Facts.Clear(); 
            DateTime theDate = new DateTime(2011,4,15);
            Tbool result = USC.Tit29.Sec2611.IsEligibleEmployee(e,c).AsOf(theDate);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Eligible_Employee_2 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c, false);  
            bool? result = USC.Tit29.Sec2611.IsEligibleEmployee(e,c).ToBool;
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Eligible_Employee_3 ()
        {
            Facts.Reset();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, Time.DawnOf.AddDays(1));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 1);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "PositionAt", c, "Unknown");          

            DateTime theDate = new DateTime(2020,4,15);
            bool? result = USC.Tit29.Sec2611.IsEligibleEmployee(e,c).AsOf(theDate).ToBool;
            Assert.AreEqual(false, result);
        }
        
        [Test]
        public void Eligible_Employee_4 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, Time.DawnOf.AddDays(1));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 40);
            Facts.Assert(e, "IsEmployeeUnder5USC6301", false);
            Facts.Assert(e, "LessThan50EmployeesWithin75MilesOfWorksite", c, false);
            Facts.Assert(e, "IsAirlineFlightCrew", c, false);     

            Tbool result = USC.Tit29.Sec2611.IsEligibleEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        // MetHourThreshold
        
        [Test]
        public void MetHourThreshold_1 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,6,1));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 40);

            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void MetHourThreshold_2 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,6,1));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 100);   // unlikely

            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void MetHourThreshold_3 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,8,15));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 40);   // a close call

            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void MetHourThreshold_4 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,8,15));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 30);  
            
            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void MetHourThreshold_5 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,12,31));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 24);  
            
            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void MetHourThreshold_6 ()
        {
            Facts.Clear();
            Facts.Assert(e, "IsEmployedBy", c);
            Facts.Assert(e, "DateStartedWorkingAt", c, new DateTime(2011,1,1));
            Facts.Assert(e,"DateFamilyLeaveBegins",c, new DateTime(2011,12,31));
            Facts.Assert(e, "HoursWorkedPerWeek", c, 24.04);  
            
            Tbool result = USC.Tit29.Sec2611.MetHourThreshold(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
    }
}
