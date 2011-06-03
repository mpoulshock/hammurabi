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

namespace USC.Tit26
{
    /// <summary>
    /// Determines whether one person is the "dependent" of another, as
    /// defined under Internal Revenue Code, Section 152.
    /// </summary>
    /// <cite>26 U.S.C. 152 (2011)</cite>    
    /// <updated>2011-03-28</updated>
    /// <remarks>
    /// Subsections c4, d3-5, e, and f have not been modeled.
    /// </remarks>
    public class Sec152 : H
    {
        /// <summary>
        /// Indicates whether one person (d) is a dependent of a taxpayer (tp).
        /// </summary>
        public static Tbool IsDependentOf(Person d, Person tp)
        {
            return (IsQualifyingChildOf(d, tp) || IsQualifyingRelativeOf(d, tp)) &&
                    !CannotBeADependentOf(d, tp);
        }
        
        /// <summary>
        /// Indicates whether one person is a "qualifying child" of another.
        /// </summary>
        public static Tbool IsQualifyingChildOf(Person d, Person tp)
        {
            Tnum taxYear = Sec441.TaxYear(d);
            
            // c1 - general elements (excluding those in c2, c3, and c4)
            Tbool genTest = Econ.SharesPrincipalAbodeWith(d,tp).ElapsedDaysPer(taxYear) > 182.5 &&
                            Econ.PercentSelfSupport(d) < 0.5 &&        // per TY
                            (!Sec6013.IsMFJ(d) | Input.Tbool(d,"MFJOnlyForRefund"));
            
            // c2 - relationship test
            Tbool relationTest = Fam.IsParentOf(tp,d) ||
                                 Fam.IsGrandparentOf(tp,d) ||
                                 Fam.IsGreatGrandparentOf(tp,d) ||
                                 Fam.AreSiblings(tp,d) ||
                                 Fam.AreStepsiblings(tp,d); // or descendant of sib or step-sib

            // c3 - age test
            Tbool under19 = (d.Age < 19).AlwaysPer(taxYear);
            Tbool under24 = (d.Age < 19).AlwaysPer(taxYear);
            Tbool ageTest = (d.Age < tp.Age && (under19 || (Econ.IsStudent(d) & under24))) ||
                            Sec22.IsDisabledPT(d);
    
            // c4 - when 2 or more people can claim the same qualifying child
            Tbool c4Test = StubIf(Input.Tbool(d, "CanBeClaimedAsQCByTwoTaxpayers"));
            
            return genTest && relationTest && ageTest && c4Test;
        }
        
        /// <summary>
        /// Indicates whether one person is a "qualifying relative" of another.
        /// Subsections d3 and d4 are omitted.
        /// </summary>
        public static Tbool IsQualifyingRelativeOf(Person d, Person tp)
        {
            // d1 - general elements (excluding those in d2-5)
            Tset allOtherKnownPeople = Facts.AllKnownPeople() - d - tp;
            Tbool genTest = (Sec61.GrossIncome(d) == 0 ||                         // shortcut
                             Sec61.GrossIncome(d) < Sec151.ExemptionAmount(d)) && // per TY
//                          PercentageSupportProvided(tp,d) > 0.5 & // per TY
                            !Exists(allOtherKnownPeople, (x,y) => IsQualifyingChildOf(d, (Person)x), d, null);
            
            // d2 - relationship test
            Tbool relationTest = Fam.IsParentOf(tp,d) ||             // A
                                 Fam.IsGrandparentOf(tp,d) ||
                                 Fam.IsGreatGrandparentOf(tp,d) ||
                                 Fam.AreSiblings(tp,d) ||            // B
                                 Fam.AreStepsiblings(tp,d) ||
                                 Fam.IsParentOf(d,tp) ||             // C
                                 Fam.IsGrandparentOf(d,tp) ||
                                 Fam.IsGreatGrandparentOf(d,tp) ||
                                 Fam.IsStepparentOf(d,tp) ||         // D
                                 Fam.IsAuntOrUncleOf(tp,d) ||        // E
                                 Fam.IsAuntOrUncleOf(d,tp) ||        // F
//                               Fam.IsParentInLawOf(tp,d) ||        // G
//                               Fam.IsParentInLawOf(d,tp) ||
//                               Fam.AreSiblingsInLaw(d,tp) ||
                                 (Econ.SharesPrincipalAbodeWith(tp,d) &&
                                  Econ.SharesHouseholdWith(tp,d));     // H - and not spouse
            
            // d4 - multiple support agreements 
            Tbool d4Test = StubIf(Input.Tbool(d, "AnotherTaxpayerProvidedSupportFor"));
                
            return genTest && relationTest && d4Test;
        }
        
        /// <summary>
        /// Exceptions to whether a person can be a dependent. 152(b).
        /// </summary>
        public static Tbool CannotBeADependentOf(Person d, Person tp)
        {
            // b1 - taxpayer cannot be a dependent & potential dependent cannot have a dependent
            // Note: searching the fact base for possible dependents and defining this test
            // recursively leads to infinite loops and probably is a case of overengineering.
            Tbool depTest = !CanClaimSomeoneAsDep(d) && !CanBeClaimedAsDepBySomeone(tp);
            
            // b2 - potential dependent cannot be married filing jointly
            // See IRS Publication 501 (2010), page 10.
            Tbool jointReturnTest = IfThen(Sec6013.IsMFJ(d), Input.Tbool(d,"FileMFJOnlyToClaimRefund"));
            
            // b3 - potential dependent must meet the citizenship test
            Tbool adoptionTest    = Fam.IsAdoptiveParentOf(tp,d) &&
                                    Econ.SharesPrincipalAbodeWith(d,tp) &&
                                    Econ.SharesHouseholdWith(d,tp) &&
                                    (tp.IsUSCitizen || tp.IsUSNational);
            Tbool citizenshipTest = d.IsUSCitizen ||
                                    d.IsUSResident ||
                                    d.IsResidentOf("Canada") ||
                                    d.IsResidentOf("Mexico") || 
                                    adoptionTest;
            
            return !depTest || !jointReturnTest || !citizenshipTest;
        }
        
        /// <summary>
        /// Indicates whether a person *can be* claimed (not whether they actually
        /// are being claimed) as a dependent by *someone* (anyone).
        /// </summary>
        private static Tbool CanBeClaimedAsDepBySomeone(Person p)
        {
            return Input.Tbool(p,"CanBeClaimedAsDependentBySomeone");
        }
        
        /// <summary>
        /// Indicates whether a person *can* claim (not whether they actually
        /// claim) *someone* else (anyone) as a dependent.
        /// </summary>
        private static Tbool CanClaimSomeoneAsDep(Person p)
        {
            return Input.Tbool(p,"CanClaimSomeoneAsDependent");
        }
        
    }
}
