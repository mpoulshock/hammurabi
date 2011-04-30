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
	public partial class H
	{		
		
        // ********************************************************************
        //  IF-THEN
        // ********************************************************************
        
        /// <summary>
        /// Temporal IF-THEN function: used to represent a nested if-then
        /// statement within in a boolean expression.
        /// </summary>
        public static Tbool IfThen(Tbool tb1, Tbool tb2)
        {       
            return !tb1 |tb2;
        }
        
		// ********************************************************************
		//	BOOL COUNT
		// ********************************************************************	
		
		// TODO: Generalize BoolCount to ValCount(val, Tvar[] list)?
		
		/// <summary>
		/// Counts the number of boolean inputs that have the same value
		/// value as the first (test) argument
		/// </summary>
		public static Tnum BoolCount (bool test, params Tbool[] list)
		{
			// Result is unknown if any input is unknown
			if (AnyAreUnknown(list)) { return new Tnum(); }
			
			Tnum result = new Tnum();
			
			foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(list))
			{	
				result.AddState(slice.Key, BoolCountK(test, slice.Value));
			}
			
			return result.Lean;
		}
		
		/// <summary>
		/// Private non-temporal BOOL COUNT function.
		/// </summary>
		private static int BoolCountK(bool test, List<object> list)
		{
			int count = 0;
			foreach (object v in list)
			{
				if (Convert.ToBoolean(v) == test)
				{
					count++;
				}
			}
			
			return count;
		}
		
		
		// ********************************************************************
		//	MIN / MAX
		// ********************************************************************
		
		/// <summary>
		/// Returns the minimum value of the given inputs.  Accepts Tnums, ints,
		/// doubles, and decimals.
		/// </summary>
		public static Tnum Min(params object[] list)
		{
			Tnum[] output = new Tnum[list.Length];
			
			for (int i=0; i<list.Length; i++)
			{
				Tnum newVal = ToTnum(list[i]);
				
				if (newVal.IsUnknown) { return new Tnum(); }

				output[i] = newVal;
			}
			
			return ApplyFcnToTimeline(x => Auxiliary.Minimum(x), output);
		}
		
		/// <summary>
		/// Returns the maximum value of the given inputs.  Accepts Tnums, ints,
		/// doubles, and decimals.
		/// </summary>
		public static Tnum Max(params object[] list)
		{
			Tnum[] output = new Tnum[list.Length];
			
			for (int i=0; i<list.Length; i++)
			{
				Tnum newVal = ToTnum(list[i]);
				
				if (newVal.IsUnknown) { return new Tnum(); }

				output[i] = newVal;
			}
			
			return ApplyFcnToTimeline(x => Auxiliary.Maximum(x), output);
		}


		// ********************************************************************
		// MISC OF THE MISC
		// ********************************************************************

		/// <summary>
		/// Converts ints, doubles, and decimals to (eternal) Tnums.
		/// </summary>
		private static Tnum ToTnum (object o)
		{
			if (Object.ReferenceEquals(o.GetType(), new Tnum().GetType()))
			{
				return (Tnum)o;
			}
			else 
			{
				try
				{
					Tnum result = new Tnum();
					result.SetEternally(Convert.ToDecimal(o));
					return result;
				}
				catch
				{
					Tnum result = new Tnum();
					return result;
				}
			}
		}
		
		
	}
}

