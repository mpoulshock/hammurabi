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
using System;

namespace USC.Tit29
{
    /// <summary>
    /// Contains definitions related to the Family and Medical Leave Act (FMLA).
    /// </summary>
    /// <cite>29 U.S.C. 2611 (2011)</cite>   
    /// <updated>2011-04-08</updated>
    /// <remarks>
    /// Modeling is in initial phase.
    /// </remarks>
    public class Sec2611 : H
    {
        /// <summary>
        /// 2611(2) - Indicates whether a person (p) is an eligible employee
        /// under the FMLA.
        /// </summary>       
        public static Tbool IsEligibleEmployee(Person p, Corp c)
        {
            // 2Ai - employed for at least 12 months as of the date leave is to begin
            //
            //   7 year gap: If the person doesn't meet 12 months in the last seven 
            //   years, but they were employed more than seven years ago, return 
            //   unknown (stub). See 29 CFR 825.110(b).
            //
            //   364 day threshold: see 29 CFR 825.110(b)(3) (12 months = 52 weeks = 364 days).
            DateTime d = DateLeaveBegins(p,c);
            Tbool TwelveMoInLast7Yrs  = Econ.IsEmployedBy(p,c).ElapsedDays(d.AddYears(-7),d) > 364;
            Tbool Employed7YrsAgo     = Econ.IsEmployedBy(p,c).ElapsedDays(Time.DawnOf,d.AddYears(-7)) > 0;
            
            Tbool prongAi = TestOrStubIf(TwelveMoInLast7Yrs, Employed7YrsAgo);

            Tbool prongAii = HoursInLast12Mo(p,c) >= 1250;
            
            Tbool prongB = !Tit5.Sec6301.IsEmployee(p) &&
                           (!Facts.InputTbool(p,"LessThan50EmployeesWithin75MilesOfWorksite",c) ||
                                  Facts.InputTbool(p,"LessThan50EmployeesAtWorksite",c));
            
            Tbool prongD = StubIf(Econ.IsAirlineFlightCrew(p,c));  // otherwise return true 

            return prongAi && prongAii && prongB && prongD;
        }
        
        /// <summary>
        /// Indicates whether a person has worked 1,250 hours in the 12-month period
        /// leading up to the family leave start date.
        /// </summary>
        public static Tnum HoursInLast12Mo(Person p, Corp c)
        {
            DateTime start = DateLeaveBegins(p,c);
            Tnum avgHours = Econ.HoursWorkedPerWeek(p,c);
            Tbool last12Mo = TheTime.IsBetween(start.AddMonths(-12), start);
            Tbool employed = last12Mo & Econ.IsEmployedBy(p,c);
            
            // HACK: Need to create a Tnum.SumOver(interval,start,end) function to implement this correctly...
            return (avgHours / 7) * employed.ElapsedDays(Time.DawnOf, start);
        }
        
        /// <summary>
        /// 2611(4) - Indicates whether a corporation (c) is covered by the FMLA.
        /// </summary>
        /// <remarks>
        /// Subsection (4)(A)(ii) is not modeled.
        /// The "engaged in commerce" test is subsumed by the "number of employees" test. See 29 CFR 825.104(b).
        /// Does not handle situations where one corp has an ownership interest in another.
        /// </remarks>
        public static Tbool IsCoveredEmployer(CorporateEntity c)
        {
            // 50 employees/20 workweeks threshold - "employs 50 or more employees for 
            // each working day during each of 20 or more calendar workweeks in the 
            // current or preceding calendar year."
            // The employer remains covered until it reaches a future point where it no 
            // longer has employed 50 employees for 20 (nonconsecutive) workweeks in the 
            // current and preceding calendar year.  29 CFR 825.105(f).
            Tnum calWeek = TheTime.TheCalendarWeek;
            Tnum weeks = (c.NumberOfEmployees > 50).AlwaysPer(calWeek).CountPer(TheTime.TheYear);
            Tbool meetsThreshold = (weeks > 20).CountPastNIntervals(TheTime.TheYear, 2) >= 1;

            return meetsThreshold  || c.IsPublicAgency;
        }
        
        /// <summary>
        /// 2611(7) - Indicates whether one person is considered the "parent"
        /// of another under the FMLA.
        /// </summary>
        new public static Tbool IsParentOf(Person p1, Person p2)
        {
            return Fam.IsBiologicalParentOf(p1,p2) ||
                   Fam.IsAdoptiveParentOf(p1,p2) ||      // assumed
                   Fam.IsFosterParentOf(p1,p2) ||        // assumed
                   Fam.IsStepparentOf(p1,p2) ||          // assumed
                   Fam.IsLegalGuardianOf(p1,p2) ||       // assumed
                   ActsInLocoParentisOf(p1,p2);
        }
        
        /// <summary>
        /// 2611(11) - Indicates whether a person (p) has a "serious health
        /// condition."
        /// </summary>
        public static Tbool HasSeriousHealthCondition(Person p)
        {
            return Facts.InputTbool(p, "HasSeriousHealthCondition");
        }
        
        /// <summary>
        /// 2611(12) - Indicates whether one person is considered the "child"
        /// of another under the FMLA.
        /// </summary>
        /// <remarks>
        /// The statute uses the terms "son" and "daughter" but because gender
        /// has no bearing on the legal conclusions, "child" is used here.
        /// </remarks>
        public static Tbool IsChildOf(Person p1, Person p2)
        {
            return (Fam.IsBiologicalParentOf(p2,p1) ||
                    Fam.IsAdoptiveParentOf(p2,p1) ||
                    Fam.IsFosterParentOf(p2,p1) ||
                    Fam.IsStepparentOf(p2,p1) ||
                    Fam.IsLegalGuardianOf(p2,p1) ||
                    ActsInLocoParentisOf(p2,p1))
                   &&
                   (p1.Age < 18 ||
                     (CFR.Tit29.Part1630.IsDisabled(p1) &&   // see 29 CFR 825.122 
                      p1.IsIncapableOfSelfCare)
                   );
        }
        
        /// <summary>
        /// Indicates the date the person started or intends to start FMLA leave.
        /// </summary>
        public static DateTime DateLeaveBegins(Person p, Corp c)
        {
            return Facts.InputDate(p,"DateFamilyLeaveBegins",c);
        }
        
        /// <summary>
        /// Indicates whether one person acts in loco parentis of another.
        /// </summary>
        new public static Tbool ActsInLocoParentisOf(Person p1, Person p2)
        {
            // See 29 CFR 825.122
            return Fam.HasDayToDayResponsibilityFor(p1,p2) &&
                   Econ.ProvidesSupportFor(p1,p2); 
        }          
                
    }
}