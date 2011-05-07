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
	/// <summary>
	/// Represent various types of economic relationships.
	/// </summary>
	public class Econ
	{
        /// <summary>
        /// Indicates whether a person is employed by a particular employer
        /// (corporation).
        /// </summary>
        public static Tbool IsEmployedBy(Person p, Corp c)
        {
            return Facts.InputTbool(p, "IsEmployedBy", c);
        }
        
        /// <summary>
        /// Returns a person's job (or job title) at a particular employer
        /// (corporation).
        /// </summary>
        public static Tstr PositionAt(Person p, Corp c)
        {
            return Facts.InputTstr(p, "PositionAt", c);
        }
        
		/// <summary>
		/// Returns whether a person is a student.
		/// </summary>
		public static Tbool IsStudent(Person p1)
		{
			return Facts.InputTbool(p1, "IsStudent");
		}
		
        /// <summary>
        /// Returns whether two people live together (plain language concept).
        /// </summary>
        public static Tbool LivesWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, "LivesWith", p2); 
        }
        
        /// <summary>
        /// Returns whether two people are members of the same household.
        /// </summary>
        public static Tbool SharesHouseholdWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, "SharesHouseholdWith", p2) ||
                   LivesWith(p1, p2);   // tentative assumption   
        }
        
        /// <summary>
        /// Returns whether two people share a principal abode.
        /// </summary>
        public static Tbool SharesPrincipalAbodeWith(Person p1, Person p2)
        {
            return Facts.Sym(p1, "SharesPrincipalAbodeWith", p2) ||
                   LivesWith(p1, p2);   // tentative assumption 
        }
        
        /// <summary>
        /// Returns whether one person provides financial support for another.
        /// </summary>
        public static Tbool ProvidesSupportFor(Person p1, Person p2)
        {
            return Facts.InputTbool(p1, "ProvidesSupportFor", p2);  
        }
        
		/// <summary>
		/// Returns the % a person financially supports themselves.
		/// </summary>
		public static Tnum PercentSelfSupport(Person p1)
		{
			return Facts.InputTnum(p1, "PercentSelfSupport");
		}
		
	}

}


