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

namespace USC.Tit29
{
    /// <summary> 
    /// Makes determinations related to the Family and Medical Leave Act (FMLA).
    /// </summary>
    /// <cite>29 U.S.C. 2612 (2011)</cite>   
    /// <updated>2011-03-31</updated>
    /// <remarks>
    /// Modeling is in initial phase.
    /// </remarks>
    public class Sec2612 : H
    {
        /// <summary>
        /// Indicates whether it is during the period in which an employee (e)
        /// is entitled to FMLA leave from corporation (c).
        /// </summary>
        public static Tbool IsDuringEligiblePeriod(Person e, Corp c)
        {
            return Stub();
        }
        
        /// <summary>
        /// 2612(a) - Returns the number of weeks of FMLA leave to which an
        /// employee (e) at corporation (c) is entitled in a 12-month period.
        /// </summary>
        public static Tnum NumberWeeksEntitled(Person e, Corp c)
        {
            return Switch(IsEntitledToServiceLeaveFrom(e,c), 26,
                          IsEntitledToRegLeaveFrom(e,c), 12,
                          true, 0);
        }
        
        /// <summary>
        /// 2612(a)(1) - Indicates whether an employee (e) is eligible for 
        /// "regular" (non-servicemember) FMLA leave from a corporation (c).
        /// </summary>
        public static Tbool IsEntitledToRegLeaveFrom(Person e, Corp c)
        {
            return Sec2611.IsCoveredEmployer(c) &&
                   Sec2611.IsEligibleEmployee(e,c) &&
                   (a1A(e,c) || a1B(e,c) || a1C(e,c) || a1D(e,c) || a1E(e,c));
        }

        /// <summary>
        /// 2612(a)(1)(A) - Birth of child
        /// </summary>
        private static Tbool a1A(Person e, Corp c)
        {
            return ReasonForLeave(e,c) == "To care for their newborn child";
        }
        
        /// <summary>
        /// 2612(a)(1)(B) - Adopted / foster child
        /// </summary>
        private static Tbool a1B(Person e, Corp c)
        {
            Tstr reason = ReasonForLeave(e,c);
            return reason == "To adopt a child" || reason == "To become a foster parent";
        }
        
        /// <summary>
        /// 2612(a)(1)(C) - Family member sick
        /// </summary>
        private static Tbool a1C(Person e, Corp c)
        {
            return ReasonForLeave(e,c) == "To care for family member with a health condition" &&
                   (Fam.AreMarried(e, SickFam(e)) || 
                    Sec2611.IsChildOf(SickFam(e), e) || 
                    Sec2611.IsParentOf(SickFam(e), e)) &&
                   Sec2611.HasSeriousHealthCondition(SickFam(e));
        }
        
        /// <summary>
        /// 2612(a)(1)(D) - Employee sick
        /// </summary>
        private static Tbool a1D(Person e, Corp c)
        {
            return ReasonForLeave(e,c) == "Employee cannot work due to health condition" &&
                   Sec2611.HasSeriousHealthCondition(e);
        }
        
        /// <summary>
        /// 2612(a)(1)(E) - Qualifying exigency / active duty
        /// </summary>
        private static Tbool a1E(Person e, Corp c)
        {
            return ReasonForLeave(e,c) == "Need arising due to family member serving in Armed Forces";
        }
        
        /// <summary>
        /// 2612(a)(3) - Indicates whether an employee (e) is eligible for
        /// servicemember family leave from a corporation (c).
        /// </summary>
        public static Tbool IsEntitledToServiceLeaveFrom(Person e, Corp c)
        {
            Person fam = (Person)Facts.AllXThat(e,"NeedsLeaveToProvideCareFor").ToPerson;  // assumes only one
            
            return ReasonForLeave(e,c) == "To care for a family member in the Armed Forces" &&
                   Sec2611.IsCoveredEmployer(c) &&
                   Sec2611.IsEligibleEmployee(e,c) &&
                   (Fam.AreMarried(e,fam) || Sec2611.IsChildOf(e,fam) || Sec2611.IsParentOf(e,fam) || Fam.IsNextOfKinOf(e,fam));
        }
        
        /// <summary>
        /// Returns a person's reason for requesting family leave from an employer.
        /// </summary>
        public static Tstr ReasonForLeave(Person e, Corp c)
        {
            return Facts.InputTstr(e,"ReasonForRequestingLeaveFrom",c);
        }
        
        /// <summary>
        /// Returns the person who the employee needs to take care of.
        /// </summary>
        private static Person SickFam(Person e)
        {
            return Facts.InputPerson(e, "NeedsLeaveToProvideCareFor");
        }
        
    }
}