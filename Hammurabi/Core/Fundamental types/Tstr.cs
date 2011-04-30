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
	#pragma warning disable 660, 661
	
	/// <summary>
	/// A string object whose value changes at various  points in time. 
	/// </summary>
	public partial class Tstr : Tvar
	{
		
		/// <summary>
		/// Constructs an unknown Tstr. 
		/// </summary>
		public Tstr()
		{
		}
		
		/// <summary>
		/// Constructs a Tstr that is eternally set to a given value. 
		/// </summary>
		public Tstr(string val)
		{
			this.SetEternally(val);
		}
		
		/// <summary>
		/// Removes redundant intervals from a Tstr. 
		/// </summary>
		public Tstr Lean
		{
			get
			{
				return this.LeanTvar<Tstr>();
			}
		}
		
		/// <summary>
		/// Converts a Tstr to a string.
		/// Returns null if the Tstr is unknown or if it's value changes over
		/// time (that is, if it's not eternal).
		/// </summary>
		new public string ToString
		{
			get
			{
				if (this.IsUnknown || TimeLine.Count > 1) { return null; }
				
				return (Convert.ToString(TimeLine.Values[0]));
			}
		}
		
		/// <summary>
		/// Returns the value of a Tstr at a specified point in time. 
		/// </summary>
		public Tstr AsOf(DateTime dt)
		{
			if (this.IsUnknown) { return new Tstr(); }
			
			return (Tstr)this.AsOf<Tstr>(dt);
		}
				
		
		// ********************************************************************
		// IsAlways / IsEver
		// ********************************************************************
		
		/// <summary>
		/// Returns true if the Tstr always has a specified value. 
		/// </summary>
		public Tbool IsAlways(string val)
		{
			return IsAlwaysTvar<Tstr>(val, Time.DawnOf, Time.EndOf);
		}
		
		/// <summary>
		/// Returns true if the Tstr always has a specified value between two
		/// given dates. 
		/// </summary>
		public Tbool IsAlways(string val, DateTime start, DateTime end)
		{
			return IsAlwaysTvar<Tstr>(val, start, end);
		}
		
		/// <summary>
		/// Returns true if the Tstr ever has a specified value. 
		/// </summary>
		public Tbool IsEver(string val)
		{
			return IsEverTvar(val);
		}
		
		/// <summary>
		/// Returns true if the Tstr ever has a specified value between two
		/// given dates. 
		/// </summary>
		public Tbool IsEver(string val, DateTime start, DateTime end)
		{
			return IsEverTvar<Tstr>(val, start, end);
		}
		
		
		//**********************************************************
		// Concatenation
		//**********************************************************
		
		/// <summary>
		/// Concatenates two or more Tstrs. 
		/// </summary>
		public static Tstr operator + (Tstr hs1, Tstr hs2)	
		{
			return Concatenate(hs1,hs2);
		}
		public static Tstr operator + (Tstr hs1, string s)	
		{
			return Concatenate(hs1, new Tstr(s));
		}
		public static Tstr operator + (string s, Tstr hs2)	
		{
			return Concatenate(new Tstr(s),hs2);
		}
		
		private static Tstr Concatenate(params Tstr[] list)
		{
			if (AnyAreUnknown(list)) { return new Tstr(); }
			
			Tstr result = new Tstr();
			
			foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(list))
			{	
				result.AddState(slice.Key, Concatenate(slice.Value));
			}
			
			return result.Lean;	
		}
		
		private static string Concatenate(List<object> list)
		{
			string result = "";
			foreach (object v in list) 
			{
				result += Convert.ToString(v); 
			}
			return result;
		}
		
		
		//**********************************************************
		// Length
		//**********************************************************
		
		/// <summary>
		/// Returns the length (in characters) of a Tstr at various 
		/// points in time.
		/// </summary>
		public Tnum Length
		{
			get
			{
				if (this.IsUnknown) { return new Tnum(); }
				
				Tnum result = new Tnum();
				
				SortedList<DateTime, object> line = this.TimeLine;
				
				for (int i=0; i<line.Count; i++) 
				{
					string item = Convert.ToString(line.Values[i]);
					result.AddState(line.Keys[i], item.Length);
				}
				
				return result;
			}
		}
		
		
		//**********************************************************************
		// Contains / StartsWith / EndsWith
		//**********************************************************************

		/// <summary>
		/// Returns true when a Tstr contains a given string. 
		/// </summary>
		public Tbool Contains(string substr)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			SortedList<DateTime, object> line = this.TimeLine;
			
			for (int i=0; i< line.Count; i++) 
			{
				string s = Convert.ToString(line.Values[i]);				
				result.AddState(line.Keys[i], s.Contains(substr));
			}
			
			return result;
		}
			
		/// <summary>
		/// Returns true when a Tstr starts with a given string. 
		/// </summary>
		public Tbool StartsWith(string substr)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			SortedList<DateTime, object> line = this.TimeLine;
			
			for (int i=0; i< line.Count; i++) 
			{
				string s = Convert.ToString(line.Values[i]);				
				result.AddState(line.Keys[i], s.StartsWith(substr));
			}
			
			return result;	
		}
		
		/// <summary>
		/// Returns true when a Tstr ends with a given string. 
		/// </summary>
		public Tbool EndsWith(string substr)
		{
			if (this.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			SortedList<DateTime, object> line = this.TimeLine;
			
			for (int i=0; i< line.Count; i++) 
			{
				string s = Convert.ToString(line.Values[i]);				
				result.AddState(line.Keys[i], s.EndsWith(substr));
			}
			
			return result;	
		}		
		
		
		//**********************************************************
		// Overloaded comparison operators
		//**********************************************************
		
		/// <summary>
		/// Returns true when two Tstrs are equal. 
		/// </summary>
		public static Tbool operator == (Tstr ts1, Tstr ts2)
		{
			return EqualTo(ts1,ts2);
		}
		
		public static Tbool operator == (Tstr ts1, string s)
		{
			return EqualTo(ts1, new Tstr(s));
		}
		
		/// <summary>
		/// Returns true when two Tstrs are not equal. 
		/// </summary>
		public static Tbool operator != (Tstr ts1, Tstr ts2)
		{
			return !EqualTo(ts1,ts2);
		}
		
		public static Tbool operator != (Tstr ts1, string s)
		{
			return !EqualTo(ts1, new Tstr(s));
		}
		
		
		//**********************************************************************
		// Other
		//**********************************************************************	
		
		/// <summary>
		/// Maps a function across the intervals of a Tstr. CONSIDER DELETING.
		/// </summary>
		private Tstr ApplyStrFcnToTimeline(Func<string,string,string,string> fcn, string s1, string s2)
		{
			Tstr result = new Tstr();
			
			SortedList<DateTime, object> line = this.TimeLine;
			
			for (int i=0; i< line.Count; i++) 
			{
				string val = fcn(Convert.ToString(line.Values[i]),s1,s2);
				
				result.AddState(line.Keys[i], val);
			}
			
			return result;
		}
		
	}
	
	#pragma warning restore 660, 661
}
