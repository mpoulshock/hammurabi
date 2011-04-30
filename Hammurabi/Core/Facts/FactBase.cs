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
using System.Collections.Generic;

namespace Hammurabi
{
	// TODO: Review accessibility level of these methods
	
	public static partial class Facts
	{
		/// <summary>
		/// This is the main global data structure.
		/// It's where all the asserted facts live.
		/// </summary>
		private static List<Fact> FactBase = new List<Fact>();
        
		
		/// <summary>
		/// A Fact object represents a fact that is stored in the knowledge
		/// base.
		/// </summary>
		private class Fact
		{
			public LegalEntity subject;
			public string relationship;
			public LegalEntity directObject;
			public Tvar v;
			public DateTime time;
			
			/// <summary>
			/// Sets a Tvar fact that relates to one legal entity  
			/// </summary>
			public Fact(LegalEntity subj, string rel, Tvar val)
			{
				subject = subj;
				relationship = rel;
				v = val;
			}
			
			/// <summary>
			/// Sets a DateTime fact that relates to one legal entity  
			/// </summary>
			public Fact(LegalEntity subj, string rel, DateTime dt)
			{
				subject = subj;
				relationship = rel;
				time = dt;
			}
			
			/// <summary>
			/// Sets a Tvar fact that establishes a relation between two legal
			/// entities  
			/// </summary>
			public Fact(LegalEntity subj, LegalEntity directObj, string rel, Tvar val)
			{
				subject = subj;
				directObject = directObj;
				relationship = rel;
				v = val;
			}
			
			/// <summary>
			/// Sets a DateTime fact that establishes a relation between two legal
			/// entities  
			/// </summary>
			public Fact(LegalEntity subj, LegalEntity directObj, string rel, DateTime dt)
			{
				subject = subj;
				directObject = directObj;
				relationship = rel;
				time = dt;
			}
			
		}
		
        /// <summary>
        /// Counts how many known facts there are. 
        /// </summary>
        public static int Count()
        {
            return FactBase.Count;
        }
        
		/// <summary>
		/// Retracts all facts in the factbase
		/// </summary>
		public static void Clear()
		{
			FactBase.Clear();
		}

	}
}