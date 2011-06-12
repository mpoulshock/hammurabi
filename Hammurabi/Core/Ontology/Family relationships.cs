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
        /// Returns a string indicating the familial relationship between
        /// two people.
        /// </summary>      
        public static Tstr Relationship(Person p1, Person p2)
        {
            return Input.Tstr(p1, r.FamilyRelationship, p2);
        }
        
        /// <summary>
        /// Returns whether two people are married.
        /// </summary>
        //  TODO: Add shortcut assertions: IsMarriedTo, IsMarried
//        public static Tbool AreMarried(Person p1, Person p2)
//        {
//            if (Facts.HasBeenAsserted(p1, r.FamilyRelationship, p2))
//            {
//                Tbool m = Facts.Sym(p1, r.FamilyRelationship, p2, "Spouse");
//                
//                if (m.IsTrue)
//                {
//                    Facts.Assert(p1, r.MaritalStatus, "Married");
//                    Facts.Assert(p2, r.MaritalStatus, "Married");
//                }
//                
//                return m; 
//            }
//            
//            else if (Facts.GetUnknowns == false)
//            {
//                Tbool n = Facts.Sym(p1, r.IsMarriedTo, p2);
//                
//                if (n.IsTrue)
//                {
//                    Facts.Assert(p1, r.MaritalStatus, "Married");
//                    Facts.Assert(p2, r.MaritalStatus, "Married");
//                }
//                
//                return n; 
//            }
//            
//            return new Tbool();
//        }
        
        public static Tbool AreMarried(Person p1, Person p2)
        {
            Tbool m = Facts.Sym(p1, r.FamilyRelationship, p2, "Spouse");
            
            if (!m.IsUnknown) { return m; }
            
            else if (Facts.GetUnknowns == false)
            {
                return Facts.Sym(p1, r.IsMarriedTo, p2);
            }
            
            return new Tbool();
        }
        
        /// <summary>
        /// Returns the person's spouse.
        /// </summary>
        public static Person SpouseOf(Person p)
        {
            // Assumes only one.  TODO: Generalize and temporalize.
            Person spouse = Input.Person(p, r.IsMarriedTo);
            
            // "Shortcut" assertion
            if (spouse.Name != "")
            {
                Facts.Assert(p, r.FamilyRelationship, spouse, "Spouse");
                Facts.Assert(p, r.MaritalStatus, "Married");
            }
            
            return spouse;
        }
        
        /// <summary>
        /// Returns whether two people are domestic partners.
        /// </summary>
        public static Tbool IsDomesticPartnerOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Domestic partner");
        }
        
        /// <summary>
        /// Returns whether two people are in a civil union.
        /// </summary>
        public static Tbool InCivilUnion(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Partner by civil union");
        }
        
        /// <summary>
        /// Returns whether two people are ex-spouses.
        /// </summary>
        private static Tbool ExSpouses(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Former spouse");
        }
        
        /// <summary>
        /// Returns whether two people are legally separated.
        /// </summary>
        public static Tbool AreSeparated(Person p1, Person p2)
        {
            return Facts.Sym(p1, "IsSeparatedFrom", p2);
        }
        
        /// <summary>
        /// Returns whether two people are divorced.
        /// </summary>
        public static Tbool AreDivorced(Person p1, Person p2)
        {
            return Facts.Sym(p1, "IsDivorcedFrom", p2); 
        }
        
        /// <summary>
        /// Returns whether one person is the parent of another.
        /// </summary>
        public static Tbool IsParentOf(Person p1, Person p2)
        {
            Tbool isP = IsBiologicalParentOf(p1, p2) ||
                        IsAdoptiveParentOf(p1, p2) ||
                        IsFosterParentOf(p1, p2) ||
                        IsStepparentOf(p1, p2);  
            
            if (!isP.IsUnknown) { return isP; }
            
            else if (Facts.GetUnknowns == false)
            {
                return Input.Tbool(p1, r.IsParentOf, p2);
            }
            
            return new Tbool();  
        }
        
        /// <summary>
        /// Returns whether one person is the biological parent of another.
        /// </summary>
        public static Tbool IsBiologicalParentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Biological parent", "Biological child");
        }
        
        /// <summary>
        /// Returns whether one person is the adoptive parent of another.
        /// </summary>
        public static Tbool IsAdoptiveParentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Adoptive parent", "Adopted child");
        }
        
        /// <summary>
        /// Returns whether one person is the foster parent of another.
        /// </summary>
        public static Tbool IsFosterParentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Foster parent", "Foster child");   
        }
        
        /// <summary>
        /// Returns whether one person is the step parent of another.
        /// </summary>
        public static Tbool IsStepparentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Stepparent", "Stepchild");  
        }
        
        /// <summary>
        /// Returns whether two people are siblings.
        /// </summary>
        public static Tbool AreSiblings(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Sibling");
            
            // or, share a parent
        }
        
        /// <summary>
        /// Returns whether two people are half-siblings.
        /// </summary>
        public static Tbool AreHalfSiblings(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Half sibling");
            
            // or, share one parent
        }
        
        /// <summary>
        /// Returns whether two people are step-siblings.
        /// </summary>
        public static Tbool AreStepsiblings(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Stepsibling");
        }
        
        /// <summary>
        /// Returns whether one person is the grandparent of another.
        /// </summary>
        public static Tbool IsGrandparentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Grandparent", "Grandchild");
        }
        
        /// <summary>
        /// Returns whether one person is the great-grandparent of another.
        /// </summary>
        public static Tbool IsGreatGrandparentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Great-grandparent", "Great-grandchild");    
        }
  
        /// <summary>
        /// Returns whether one person is the great-great-grandparent of another.
        /// </summary>
        public static Tbool IsGreatGreatGrandparentOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Great-great-grandparent", "Great-great-grandchild");   
        }
        
        /// <summary>
        /// Returns whether one person is the aunt or uncle of another.
        /// </summary>
        public static Tbool IsAuntOrUncleOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Aunt or uncle", "Niece or nephew");    
        }
        
        /// <summary>
        /// Returns whether one person is the great aunt or uncle of another.
        /// </summary>
        public static Tbool IsGreatAuntOrUncleOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Great aunt or uncle", "Grand niece or nephew");
        }
        
        /// <summary>
        /// Returns whether one person is the great-great aunt or uncle of another.
        /// </summary>
        public static Tbool IsGreatGreatAuntOrUncleOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Great-great aunt or uncle", "Great-grand niece or nephew");
        }
        
        /// <summary>
        /// Returns whether two people are cousins.
        /// </summary>
        public static Tbool IsCousinOf(Person p1, Person p2)
        {
            return IsFirstCousinOf(p1, p2) || IsNonFirstCousinOf(p1, p2);
        }
        
        /// <summary>
        /// Returns whether two people are first cousins.
        /// </summary>
        public static Tbool IsFirstCousinOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "First cousin");    
        }
        
        /// <summary>
        /// Returns whether two people are non-first cousins.
        /// </summary>
        public static Tbool IsNonFirstCousinOf(Person p1, Person p2)
        {
            return Facts.Sym(p1, r.FamilyRelationship, p2, "Other cousin");    
        }      
        
        /// <summary>
        /// Returns whether one person is the ancestor of another.
        /// </summary>
        public static Tbool IsAncestorOf(Person p1, Person p2)
        {
            return Input.Tbool(p1, "IsAncestorOf", p2);    
        }

        /// <summary>
        /// Returns whether one person has legal custody of another.
        /// </summary>
        public static Tbool HasCustodyOf(Person p1, Person p2)
        {
            return Input.Tbool(p1, r.HasCustodyOf, p2);    
        }
        
        /// <summary>
        /// Returns whether one person is the legal guardian of another.
        /// </summary>
        public static Tbool IsLegalGuardianOf(Person p1, Person p2)
        {
            return !NotAGuardianOf(p1, p2) &&
                   Input.Tbool(p1, r.IsLegalGuardianOf, p2);   
        }
        
        /// <summary>
        /// Returns whether one person acts in loco parentis of another.
        /// </summary>
        public static Tbool ActsInLocoParentisOf(Person p1, Person p2)
        {
            return !NotAGuardianOf(p1, p2) && 
                   Input.Tbool(p1, r.ActsInLocoParentisOf, p2);   
        }
        
        /// <summary>
        /// Returns whether one person has day-to-day responsibility for
        /// another person.
        /// </summary>
        public static Tbool HasDayToDayResponsibilityFor(Person p1, Person p2)
        {
            return !NotAGuardianOf(p1, p2) && 
                   Input.Tbool(p1, r.HasDayToDayResponsibilityFor, p2);   
        }
        
        /// <summary>
        /// Indicates people who can not be guardians.
        /// </summary>
        private static Tbool NotAGuardianOf(Person p1, Person p2)
        {
            return IsParentOf(p2, p1) ||
                   InCivilUnion(p1, p2) || 
                   IsDomesticPartnerOf(p1, p2) ||
                   ExSpouses(p1, p2);
        }
        
        /// <summary>
        /// Infers whether one person is the custodial parent of another.
        /// </summary>
        public static Tbool IsCustodialParentOf(Person p1, Person p2)
        {
            return IsParentOf(p1,p2) &&
                   HasCustodyOf(p1,p2); 
        }
        
        /// <summary>
        /// Returns whether one person is the next of kin (nearest
        /// blood relative) of another.
        /// </summary>
        public static Tbool IsNextOfKinOf(Person p1, Person p2)
        {
            return Input.Tbool(p1, r.IsNextOfKinOf, p2);  
        }        
    }
}
