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
	public static partial class Facts
	{
		//*********************************************************************
		// Inputs - these should be used in the law-related code
		//*********************************************************************
		
		/// <summary>
		/// Input(sbj-rel-obj) => Tbool
		/// </summary>
		public static Tbool InputTbool(LegalEntity subj, string rel, LegalEntity directObj)	
		{	
			return (Tbool)QueryTvar<Tbool>(subj, rel, directObj);
		}
		
		/// <summary>
		/// Input(sbj-rel-obj) => Tnum
		/// </summary>
		public static Tnum InputTnum(LegalEntity subj, string rel, LegalEntity directObj)		
		{	
			return (Tnum)QueryTvar<Tnum>(subj, rel, directObj);
		}
		
		/// <summary>
		/// Input(sbj-rel-obj) => Tstr
		/// </summary>
		public static Tstr InputTstr(LegalEntity subj, string rel, LegalEntity directObj)			
		{	
			return (Tstr)QueryTvar<Tstr>(subj, rel, directObj);
		}
		
		// TODO: Implement DateTime inputs
		
		
		/// <summary>
		/// Input (sbj-rel) => Tbool
		/// </summary>
		public static Tbool InputTbool(LegalEntity subj, string rel)
		{	
			return (Tbool)QueryTvar<Tbool>(subj, rel);
		}
		
		/// <summary>
		/// Input (sbj-rel-obj) => Tnum
		/// </summary>
		public static Tnum InputTnum(LegalEntity subj, string rel)		
		{	
			return (Tnum)QueryTvar<Tnum>(subj, rel);
		}
		
		/// <summary>
		/// Input (sbj-rel-obj) => Tstr
		/// </summary>
		public static Tstr InputTstr(LegalEntity subj, string rel)		
		{	
			return (Tstr)QueryTvar<Tstr>(subj, rel);
		}

		
		//*********************************************************************
		// Queries 
		//*********************************************************************
		
		/// <summary>
		/// Query a temporal relationship (fact) of two legal entities
		/// </summary>
		private static T QueryTvar<T>(LegalEntity e1, string rel, LegalEntity e2) where T : Tvar
		{
			// Look up fact in table of facts
			foreach (Fact f in FactBase)
			{
				if (f.subject == e1 && f.relationship == rel && f.directObject == e2)
				{
					return (T)f.v;
				}
			}

			// If fact is not found, return an unknown Tvar
			return (T)Auxiliary.ReturnProperTvar<T>();
		}
		
		/// <summary>
		/// Query a temporal property (fact) of a single legal entity
		/// </summary>
		private static T QueryTvar<T>(LegalEntity e1, string rel) where T : Tvar
		{
			foreach (Fact f in FactBase)
			{
				if (f.subject == e1 && f.relationship == rel)
				{
					return (T)f.v;
				}
			}
			return (T)Auxiliary.ReturnProperTvar<T>();
		}
		
		/// <summary>
		/// Query a DateTime relationship between two legal entities.
		/// Example: the date Person1 and Person2 were married.
		/// </summary>
		private static DateTime QueryDateTime(LegalEntity e1, string rel, LegalEntity e2)
		{
			foreach (Fact f in FactBase)
			{
				if (f.subject == e1 && f.relationship == rel && f.directObject == e2)
				{
					return f.time;
				}
			}
			return new DateTime(1,1,1);		// TODO: Make date queries nullable?
		}
		
		/// <summary>
		/// Query a DateTime property of one legal entity.
		/// Example: the date Person1 was born.
		/// </summary> 
		private static DateTime QueryDateTime(LegalEntity e1, string rel)
		{
			foreach (Fact f in FactBase)
			{
				if (f.subject == e1 && f.relationship == rel)
				{
					return f.time;
				}
			}
			return new DateTime(1,1,1);	
		}
		
		
		
	}
}
