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

using System;
using Hammurabi;
using NUnit.Framework;
using USC.Tit26;

namespace Hammurabi.UnitTests
{
    /// <summary>
    /// 26 U.S.C. 152 - Dependents under Internal Revenue Code.
    /// </summary>
    [TestFixture]
    public class USC_Tit26_Sec152 : H
    {
        private static DateTime endOf2010 = Date(2010,12,31);
        private Person t = new Person("Taxpayer");
        
        /// <summary>
        /// Simplest scenario in which a person is a dependent (parent of a 
        /// young child).
        /// </summary>
        [Test]
        public void Pub501_Simple ()
        {
            // Instantiate some people
            Person c = new Person("Child");

            // Assert facts about the people
            Facts.Clear();
            Facts.Assert(t, r.IsParentOf, c);
            Facts.Assert(c, r.LivesWith, t);
            Facts.Assert(c, r.DateOfBirth, Date(2000,1,1));
            Facts.Assert(c, "IsUSCitizen");
            Facts.Assert(c, "CanBeClaimedAsQCByTwoTaxpayers", false);
            Facts.Assert(c, r.PercentSelfSupport, 0);
            Facts.Assert(c, "CanClaimSomeoneAsDependent", false);
            Facts.Assert(t, "CanBeClaimedAsDependentBySomeone", false);
            Facts.Assert(c, "AnotherTaxpayerProvidedSupportFor", false);
            Facts.Assert(c, "GrossIncome", 0);

            // Get and evaluate result
            bool? result = Sec152.IsDependentOf(c,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(true, result);
        }
        
        /// <summary>
        /// IRS Pub. 501 (2010), Page 10, Example 1.
        /// Cannot be a dependent because fails joint return test.
        /// </summary>
        [Test]
        public void Pub501_1 ()
        {
            // Instantiate some people
            Person d = new Person("Daughter");
            Person s = new Person("Son-in-law");

            // Assert facts about the people
            Facts.Clear();
            Facts.Assert(d, r.IsMarriedTo, s);
            Facts.Assert(d, "FilesJointFedTaxReturnWith", s);
            Facts.Assert(d, "FileMFJOnlyToClaimRefund", false);  // ???

            // These facts are irrelevant but are in the fact pattern
            Facts.Assert(t, r.IsParentOf, d);
            Facts.Assert(t, r.LivesWith, d);
            Facts.Assert(d, r.DateOfBirth, Date(1992,1,1));
            Facts.Assert(s, "IsInArmedForces");
            Facts.Assert(d, "IsUSCitizen");

            // Get and evaluate result
            bool? result = Sec152.IsDependentOf(d,t).ToBool;
            Assert.AreEqual(false, result);
        }
        
        /// <summary>
        /// IRS Pub. 501 (2010), Page 10, Example 2 (son).
        /// </summary>
        [Test]
        public void Pub501_2_a ()
        {
            Person s = new Person("Son");
            Person d = new Person("Daughter-in-law");

            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1992,1,1));
            Facts.Assert(d, r.DateOfBirth, Date(1991,1,1));
            Facts.Assert(s, r.IsMarriedTo, d);
            Facts.Assert(s, "IsUSCitizen");
            Facts.Assert(d, "FilesJointFedTaxReturnWith", s);
            Facts.Assert(s, "FileMFJOnlyToClaimRefund");
            Facts.Assert(s, "CanClaimSomeoneAsDependent", false);
            Facts.Assert(t, "CanBeClaimedAsDependentBySomeone", false);

            bool? result = Sec152.CannotBeADependentOf(s,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(false, result);
        }
        
        /// <summary>
        /// IRS Pub. 501 (2010), Page 10, Example 2 (daughter-in-law).
        /// </summary>
        [Test]
        public void Pub501_2_b ()
        {
            Person s = new Person("Son");
            Person d = new Person("Daughter-in-law");

            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1992,1,1));
            Facts.Assert(d, r.DateOfBirth, Date(1991,1,1));
            Facts.Assert(s, r.IsMarriedTo, d);
            Facts.Assert(d, "IsUSCitizen");
            Facts.Assert(d, "FilesJointFedTaxReturnWith", s);
            Facts.Assert(d, "FileMFJOnlyToClaimRefund");
            Facts.Assert(d, "CanClaimSomeoneAsDependent", false);
            Facts.Assert(t, "CanBeClaimedAsDependentBySomeone", false);

            bool? result = Sec152.CannotBeADependentOf(d,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(false, result);
        }
        
        /// <summary>
        /// IRS Pub. 501 (2010), Page 10, Example 3.
        /// </summary>
        [Test]
        public void Pub501_3 ()
        {
            Person s = new Person("Son");
            Person d = new Person("Daughter-in-law");

            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1992,1,1));
            Facts.Assert(d, r.DateOfBirth, Date(1991,1,1));
            Facts.Assert(s, r.IsMarriedTo, d);
            Facts.Assert(s, "IsUSCitizen");
            Facts.Assert(d, "FilesJointFedTaxReturnWith", s);
            Facts.Assert(s, "FileMFJOnlyToClaimRefund", false);
            Facts.Assert(s, "CanClaimSomeoneAsDependent", false);
            Facts.Assert(t, "CanBeClaimedAsDependentBySomeone", false);

            bool? result = Sec152.CannotBeADependentOf(s,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// IRS Pub. 501 (2010), Page 11, Unnumbered example.
        /// </summary>
        [Test]
        public void Pub501_4 ()
        {
            Person s = new Person("Son");
            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1991,12,10));
            Facts.Assert(s, "IsFullTimeStudent", false);
            Facts.Assert(s, "IsPermanentlyAndTotallyDisabled", false);

            bool? result = Sec152.IsDependentOf(s,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// IRS Pub. 501 (2010), Page 11, Unnumbered example.
        /// </summary>
        [Test]
        public void Pub501_5 ()
        {
            Person s = new Person("Son");
            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1991,12,10));
            Facts.Assert(s, "IsFullTimeStudent");
            Facts.Assert(s, "IsPermanentlyAndTotallyDisabled", false);

            bool? result = Sec152.IsDependentOf(s,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// IRS Pub. 501 (2010), Page 11, Unnumbered example.
        /// </summary>
        [Test]
        public void Pub501_6 ()
        {
            Person s = new Person("Son");
            Facts.Clear();
            Facts.Assert(s, r.DateOfBirth, Date(1991,12,10));
            Facts.Assert(s, "IsFullTimeStudent", false);
            Facts.Assert(s, "IsPermanentlyAndTotallyDisabled");

            bool? result = Sec152.IsDependentOf(s,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// IRS Pub. 501 (2010), Page 11, Example 1.
        /// </summary>
        [Test]
        public void Pub501_7 ()
        {
            Person s = new Person("Spouse");
            Person b = new Person("Brother");

            Facts.Clear();
            Facts.Assert(t, r.DateOfBirth, Date(1989,1,1));
            Facts.Assert(s, r.DateOfBirth, Date(1989,1,1));
            Facts.Assert(t, r.IsMarriedTo, s);
            Facts.Assert(b, "IsSiblingOf", t);
            Facts.Assert(b, r.DateOfBirth, Date(1987,1,1));
            Facts.Assert(b, "IsFullTimeStudent");
            Facts.Assert(b, "IsMarried", false);
            Facts.Assert(t, r.LivesWith, s);
            Facts.Assert(t, r.LivesWith, b);
            Facts.Assert(b, r.IsDisabled, false);
            Facts.Assert(t, "AreFilingMFJ", s);

            bool? result = Sec152.IsDependentOf(b,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// IRS Pub. 501 (2010), Page 11, Example 2.
        /// </summary>
        [Test]
        public void Pub501_8 ()
        {
            Person s = new Person("Spouse");
            Person b = new Person("Brother");

            Facts.Clear();
            Facts.Assert(t, r.DateOfBirth, Date(1989,1,1));
            Facts.Assert(s, r.DateOfBirth, Date(1985,1,1));
            Facts.Assert(t, r.IsMarriedTo, s);
            Facts.Assert(b, "IsSiblingOf", t);
            Facts.Assert(b, r.DateOfBirth, Date(1987,1,1));
            Facts.Assert(b, "IsFullTimeStudent");
            Facts.Assert(b, "IsMarried", false);
            Facts.Assert(t, r.LivesWith, s);
            Facts.Assert(t, r.LivesWith, b);
            Facts.Assert(b, r.IsDisabled, false);
            Facts.Assert(t, "AreFilingMFJ", s);

            bool? result = Sec152.IsDependentOf(b,t).AsOf(endOf2010).ToBool;
            Assert.AreEqual(true, result);
        }
    }
}
