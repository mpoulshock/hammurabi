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

using System;

namespace Hammurabi
{
    /// <summary>
    /// Represent various types of economic relationships.
    /// </summary>
    public class Econ : H
    {
        /// <summary>
        /// Indicates whether a person is employed by a particular employer
        /// (corporation).  It is assumed that "employee" does not include 
        /// independent contractors.
        /// </summary>
        public static Tbool IsEmployedBy(Person p, CorporateEntity c)
        {
            // Allows for simple temporal input (start date)
            // Need to enhance this later...
            Tbool isEmp;
            
            // If interview, ask only "nature of employment relationship"
            if (Facts.GetUnknowns)
            {
                isEmp = Facts.InputTstr(p, r.NatureOfEmploymentRelationship, c) == "Employee";
            }
            else
            {
                isEmp = Facts.Either(Facts.InputTstr(p, r.NatureOfEmploymentRelationship, c) == "Employee",
                                     Facts.InputTbool(p, r.IsEmployedBy, c));
            }

            // Get employment start date, if necessary            
            if (isEmp.IsTrue)
            {
                DateTime start = Facts.InputDate(p, r.DateStartedWorkingAt, c);
                
                if (start != Time.DawnOf)
                {
                    Tbool result = new Tbool(false);
                    result.AddState(start,true);  
                    return result;
                }
                else { return new Tbool(true); }
            }
            else
            {
                return isEmp;
            }
        }
        
        /// <summary>
        /// Returns whether a person is an independent contractor at an
        /// employer.
        /// </summary>
        public static Tbool IsIndependentContractor(Person p, CorporateEntity c)
        {
            Tbool isIC;
            
            // If interview, ask only "nature of employment relationship"
            if (Facts.GetUnknowns)
            {
                isIC = Facts.InputTstr(p, r.NatureOfEmploymentRelationship, c) == "Independent contractor";
            }
            else
            {
                isIC = Facts.Either(Facts.InputTstr(p, r.NatureOfEmploymentRelationship, c) == "Independent contractor",
                                     Facts.InputTbool(p, r.IsIndependentContractor, c));
            }

            // Get employment start date, if necessary
            if (isIC.IsTrue)
            {
                DateTime start = Facts.InputDate(p, r.DateStartedWorkingAt, c);
                
                if (start != Time.DawnOf)
                {
                    Tbool result = new Tbool(false);
                    result.AddState(start,true);  
                    return result;
                }
                else { return new Tbool(true); }
            }
            else
            {
                return isIC;
            }
        }
        
        /// <summary>
        /// Returns the date a person started working at an employer.
        /// </summary>
        public static DateTime DateStartedWorkAt(Person p, CorporateEntity c)
        {
            return Facts.InputDate(p, r.DateStartedWorkingAt, c);
        }
        
        /// <summary>
        /// Returns the number of hours per week that a person works at a given
        /// employer.
        /// </summary>
        public static Tnum HoursWorkedPerWeek(Person p, CorporateEntity c)
        {
            return Facts.InputTnum(p, r.HoursWorkedPerWeek, c);
        }
        
        /// <summary>
        /// Determines whether a person is a full-time employee.
        /// </summary>
        public static Tbool IsFullTimeEmployee(Person p, CorporateEntity c)
        {
            Tnum hours = HoursWorkedPerWeek(p,c);
            
            // If 35 or more hours  => True
            // If 30 or fewer hours => False
            // Else                 => Null
            return IsEmployedBy(p,c) & TestOrStubIf(hours >= 35, hours < 35 && hours >= 30);
        }
        
        /// <summary>
        /// Returns the length of an employee's initial probationary period (in
        /// months) at an employer.
        /// </summary>
        public static Tnum LengthOfInitialProbation(Person p, CorporateEntity c)
        {
            return Facts.InputTnum(p, "LengthOfInitialProbationaryPeriodAtEmployerInMonths", c);
        }
        
        /// <summary>
        /// Returns whether a person is a student.
        /// </summary>
        public static Tbool IsStudent(Person p)
        {
            return Facts.InputTbool(p, r.IsStudent);
        }
        
        /// <summary>
        /// Returns whether two people live together (plain language concept).
        /// </summary>
        public static Tbool LivesWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.LivesWith, p2); 
        }
        
        /// <summary>
        /// Returns whether two people are members of the same household.
        /// </summary>
        public static Tbool SharesHouseholdWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.SharesHouseholdWith, p2) ||
                   LivesWith(p1, p2);   // tentative assumption   
        }
        
        /// <summary>
        /// Returns whether two people share a principal abode.
        /// </summary>
        public static Tbool SharesPrincipalAbodeWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.SharesPrincipalAbodeWith, p2) ||
                   LivesWith(p1, p2);   // tentative assumption 
        }
        
        /// <summary>
        /// Returns whether one person provides financial support for another.
        /// </summary>
        public static Tbool ProvidesSupportFor(Person p1, Person p2)
        {
            return Facts.InputTbool(p1, r.ProvidesSupportFor, p2);  
        }
        
        /// <summary>
        /// Returns the % a person financially supports themselves.
        /// </summary>
        public static Tnum PercentSelfSupport(Person p)
        {
            return Facts.InputTnum(p, r.PercentSelfSupport);
        }
        
        /// <summary>
        /// How random...
        /// </summary>
        public static Tbool IsAirlineFlightCrew(Person p, Corp c)
        {
            return Facts.InputTbool(p, r.IsAirlineFlightCrew, c);
        }
        
    }

}


