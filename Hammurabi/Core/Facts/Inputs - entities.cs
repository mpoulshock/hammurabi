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
	// TODO: Derive from class H?
	public static partial class Facts
	{
		
		/// <summary>
		/// Queries the fact base to get all direct objects, given a 
		/// relationship and a subject.  In other words, it returns all
		/// of the direct objects in the knowledge base that meet given
		/// criteria.
		/// </summary>
		public static Tset AllXThat(LegalEntity subj, string relationship)
		{
			Tset result = new Tset();
			
			// Identify all matching fact patterns (Tbools)
			Dictionary<LegalEntity,Tbool> ListOfMatches = new Dictionary<LegalEntity,Tbool>();
			foreach (Fact f in FactBase)
			{
				if (f.subject == subj && f.relationship == relationship)
				{
					ListOfMatches.Add(f.directObject,(Tbool)f.v);
				}
			}
			
			// Identify all breakpoints in the set of matches
			List<Tvar> listOfBooleans = new List<Tvar>();
			foreach (KeyValuePair<LegalEntity,Tbool> pair in ListOfMatches)
			{
				listOfBooleans.Add(pair.Value);
			}
			
			// Foreach breakpoint, identify all entities that meet the 
			// relationship criterion at that point in time
			foreach (DateTime d in H.TimePoints(listOfBooleans))
			{
				List<LegalEntity> entities = new List<LegalEntity>();
				
				foreach (KeyValuePair<LegalEntity,Tbool> pair in ListOfMatches)
				{
					if (pair.Value.AsOf(d).ToBool == true)
					{
						entities.Add(pair.Key);
					}
				}
				
				result.AddState(d,entities);
			}

			// If no matching entities are found in the fact table,
			// return an empty Tset.
			// Note: This is different from an "unknown" Tset, whose
			// members are not known.  Here, we return a Tset that
			// is known to have no members.  Unlike the other input
			// functions, this assumes a "closed world" (in which 
			// unknown facts are presumed false).
			// Rationale: This function returns an aggregation of facts
			// as opposed to a base-level fact.
			if (result.IntervalValues.Count == 0)
				result.AddState(Time.DawnOf,new List<LegalEntity>());
			
			return result;
		}

		/// <summary>
		/// Queries the fact base to get all direct subjects, given a 
		/// relationship and a direct object.  In other words, it returns all
		/// of the subjects in the knowledge base that meet given
		/// criteria.
		/// </summary>
		public static Tset AllXSuchThatX(string relationship, LegalEntity obj)
		{
			Tset result = new Tset();
			
			// Identify all matching fact patterns (Tbools)
			Dictionary<LegalEntity,Tbool> ListOfMatches = new Dictionary<LegalEntity,Tbool>();
			foreach (Fact f in FactBase)
			{
				if (f.relationship == relationship && f.directObject == obj)
				{
					ListOfMatches.Add(f.subject,(Tbool)f.v);
				}
			}
			
			// Identify all breakpoints in the set of matches
			List<Tvar> listOfBooleans = new List<Tvar>();
			
			foreach (KeyValuePair<LegalEntity,Tbool> pair in ListOfMatches)
			{
				listOfBooleans.Add(pair.Value);
			}
			
			// Foreach breakpoint, identify all entities that meet the 
			// relationship criterion at that point in time
			foreach (DateTime d in H.TimePoints(listOfBooleans))
			{
				List<LegalEntity> entities = new List<LegalEntity>();
				
				foreach (KeyValuePair<LegalEntity,Tbool> pair in ListOfMatches)
				{
					if (pair.Value.AsOf(d).ToBool == true)
					{
						entities.Add(pair.Key);
					}
				}
				
				result.AddState(d,entities);
			}

			// If no matching entities are found in the fact table,
			// return an empty Tset.
			// See explanation in AllXThat() above.
			if (result.IntervalValues.Count == 0)
				result.AddState(Time.DawnOf,new List<LegalEntity>());
			
			return result;
		}	
		
		/// <summary>
		/// Queries the fact base to get all objects in a symmetrical
		/// relationship with a given object.
		/// For example: spousesOfBill = AllXSymmetrical(Bill,"IsMarriedTo");
		/// </summary>
		public static Tset AllXSymmetrical(LegalEntity entity, string relationship)
		{
			return Facts.AllXSuchThatX(relationship, entity) |
				   Facts.AllXThat(entity, relationship);
		}
		
		/// <summary>
		/// Queries the fact base to get all known people and returns
		/// the result in an eternal Tset.
		/// </summary>
		public static Tset AllKnownPeople()
		{
			Tset result = new Tset();
			List<LegalEntity> thePeople = new List<LegalEntity>();
			
			foreach (Fact f in FactBase)
			{
				if (f.subject.GetType() == new Person("").GetType())
					if (!thePeople.Contains(f.subject))
						thePeople.Add(f.subject);

				if(f.directObject != null && f.directObject.GetType() == new Person("").GetType())
					if (!thePeople.Contains(f.directObject))
						thePeople.Add(f.directObject);
			}
			
			result.AddState(Time.DawnOf,thePeople);
			return result;
		}
			
	}
}