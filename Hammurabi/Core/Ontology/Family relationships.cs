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

namespace Hammurabi
{
	// TODO: Add family relation inference rules where appropriate.
	
	/// <summary>
	/// Represent various types of family relationships among people.
	/// </summary>
	public class Fam
	{
		/// <summary>
		/// Returns whether two people are married.
		/// </summary>
		public static Tbool AreMarried(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsMarriedTo", p2) |
			       Facts.InputTbool(p2, "IsMarriedTo", p1);
		}
		
		/// <summary>
		/// Returns whether two people are legally separated.
		/// </summary>
		public static Tbool AreSeparated(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsSeparatedFrom", p2) |
			       Facts.InputTbool(p2, "IsSeparatedFrom", p1);
		}
		
		/// <summary>
		/// Returns whether two people are divorced.
		/// </summary>
		public static Tbool AreDivorced(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsDivorcedFrom", p2) |
			       Facts.InputTbool(p2, "IsDivorcedFrom", p1);	
		}
		
		/// <summary>
		/// Returns whether one person is the parent of another.
		/// </summary>
		public static Tbool IsParentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsParentOf", p2) |
			       IsBiologicalParentOf(p1, p2) |
			       IsAdoptiveParentOf(p1, p2) |
			       IsFosterParentOf(p1, p2) |
			       IsStepParentOf(p1, p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the biological parent of another.
		/// </summary>
		public static Tbool IsBiologicalParentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsBiologicalParentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the adoptive parent of another.
		/// </summary>
		public static Tbool IsAdoptiveParentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsAdoptiveParentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the foster parent of another.
		/// </summary>
		public static Tbool IsFosterParentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsFosterParentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the step parent of another.
		/// </summary>
		public static Tbool IsStepParentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsStepParentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether two people are siblings.
		/// </summary>
		public static Tbool AreSiblings(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsSiblingOf", p2) |
			       Facts.InputTbool(p2, "IsSiblingOf", p1);
			
			// or, share a parent
		}
		
		/// <summary>
		/// Returns whether two people are half-siblings.
		/// </summary>
		public static Tbool AreHalfSiblings(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsHalfSiblingOf", p2) |
			       Facts.InputTbool(p2, "IsHalfSiblingOf", p1);
			
			// or, share one parent
		}
		
		/// <summary>
		/// Returns whether two people are step-siblings.
		/// </summary>
		public static Tbool AreStepSiblings(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsStepSiblingOf", p2) |
			       Facts.InputTbool(p2, "IsStepSiblingOf", p1);
		}
		
		/// <summary>
		/// Returns whether one person is the grandparent of another.
		/// </summary>
		public static Tbool IsGrandparentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsGrandparentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the great-grandparent of another.
		/// </summary>
		public static Tbool IsGreatGrandparentOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsGreatGrandparentOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the ancestor of another.
		/// </summary>
		public static Tbool IsAncestorOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsAncestorOf", p2);	
		}

		/// <summary>
		/// Returns whether one person has legal custody of another.
		/// </summary>
		public static Tbool HasCustodyOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "HasCustodyOf", p2);	
		}
		
		/// <summary>
		/// Returns whether one person is the legal guardian of another.
		/// </summary>
		public static Tbool IsLegalGuardianOf(Person p1, Person p2)
		{
			return Facts.InputTbool(p1, "IsLegalGuardianOf", p2);	
		}
		
        /// <summary>
        /// Returns whether one person acts in loco parentis of another.
        /// </summary>
        public static Tbool ActsInLocoParentisOf(Person p1, Person p2)
        {
            return Facts.InputTbool(p1, "ActsInLocoParentisOf", p2);   
        }
        
        /// <summary>
        /// Returns whether one person has day-to-day responsibility for
        /// another person.
        /// </summary>
        public static Tbool HasDayToDayResponsibilityFor(Person p1, Person p2)
        {
            return Facts.InputTbool(p1, "HasDayToDayResponsibilityFor", p2);   
        }
        
		/// <summary>
		/// Infers whether one person is the custodial parent of another.
		/// </summary>
		public static Tbool IsCustodialParentOf(Person p1, Person p2)
		{
			return IsParentOf(p1,p2) &
				   HasCustodyOf(p1,p2);	
		}
		
        /// <summary>
        /// Returns whether one person is the next of kin (nearest
        /// blood relative) of another.
        /// </summary>
        public static Tbool IsNextOfKinOf(Person p1, Person p2)
        {
            return Facts.InputTbool(p1, "IsNextOfKinOf", p2);  
        }
		
	}

}


