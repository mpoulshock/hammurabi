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

namespace USC.Tit26
{
	
	/// <summary>
	/// Determines whether two married taxpayers are eligible to file a joint
	/// return as defined under the Internal Revenue Code, Section 6013.
	/// </summary>
	/// <cite>26 U.S.C. 6013 (2010)</cite>	
	/// <updated>2011-03-08</updated>		
	/// <remarks>
	/// The logic of this provision has not been implemented yet.  Currently,
	/// it just indicates whether the filing status is married filing
	/// jointly.
	/// </remarks> 
	public class Sec6013 : H
	{
		/// <summary>
		/// Indicates whether two married taxpayers are filing a joint return.
		/// </summary>
		public static Tbool AreMFJ(Person p1, Person p2)
		{
			return Facts.Sym(p1, "FilesJointFedTaxReturnWith", p2) ||
				   Facts.Sym(p1, "FedTaxFilingStatus", p2, "Married filing jointly");	
		}

        /// <summary>
        /// Determines whether a person is married filing jointly with *anyone*
        /// in the fact base.
        /// </summary>
        public static Tbool IsMFJ(Person p)
        {
            // Find the person's spouse
            Tset spouse = Facts.AllXSymmetrical(p,"IsMarriedTo");
            
            // See if the person and their spouse are filing a joint return          
            return Exists(spouse, (x,y) => AreMFJ((Person)x, p), null, p);
        }
        
        /// <summary>
        /// Determines whether people file a joint return merely to claim a 
        /// refund.
        /// </summary>
//        public static Tbool FileMFJOnlyToClaimRefund(Person p1, Person p2)
//        {
//            return Facts.InputTbool(p1, "FileMFJOnlyToClaimRefund", p2) |
//                   Facts.InputTbool(p2, "FileMFJOnlyToClaimRefund", p1);
//        }
        
		
	}
}