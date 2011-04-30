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
	
	public partial class Tbool
	{		
		
		// ********************************************************************
		//	AND
		// ********************************************************************
		
		/// <summary>
		/// Temporal AND function: returns true when all of the inputs
		/// are true.
		/// </summary>
		public static Tbool operator & (Tbool tb1, Tbool tb2)
		{
//            // Turn off "GetUnknowns" mode while analyzing short-circuits
//            bool originalMode = Facts.GetUnknowns;
//            Facts.GetUnknowns = false;
//            
//            // Short circuit 1:
//            // If any input is eternally false, return false
////            foreach (Tbool b in list)
////            {
//                if (tb1.IntervalValues.Count == 1 &&
//                    Convert.ToBoolean(tb1.IntervalValues.Values[0]) == false)
//                {
//                    // Restore "GetUnknowns" mode to its original state
//                    Facts.GetUnknowns = originalMode;
//                    
//                    return new Tbool(false);
//                }
////            }
//            
//            if (tb2.IntervalValues.Count == 1 &&
//                    Convert.ToBoolean(tb2.IntervalValues.Values[0]) == false)
//                {
//                    // Restore "GetUnknowns" mode to its original state
//                    Facts.GetUnknowns = originalMode;
//                    
//                    return new Tbool(false);
//                }
//            
//            
//            // Short circuit 2:
//            // If any input IsUnknown, return an unknown Tbool
//            if (AnyAreUnknown(tb1, tb2)) 
//            {
//                // Restore "GetUnknowns" mode to its original state
//                Facts.GetUnknowns = originalMode;
//                
//                return new Tbool(); 
//            }
//            
//            // Restore "GetUnknowns" mode to its original state
//            Facts.GetUnknowns = originalMode;
//            
//            // Else, apply the AND function to the inputs
////            return ApplyFcnToTimeline(x => And(x), list);
//            
			return And(tb1,tb2);
		}
		
		public static Tbool operator & (Tbool tb, bool b)
		{
			return And(tb, new Tbool(b));
		}
		
		public static Tbool operator & (bool b, Tbool tb)
		{
			return And(tb, new Tbool(b));
		}
		
		/// <summary>
		/// Private temporal AND function
		/// </summary>
        private static Tbool And(params Tbool[] list)
        {
            // Short circuit 1:
            // If any input is eternally false, return false
            foreach (Tbool b in list)
            {
                if (b.IntervalValues.Count == 1 &&
                    Convert.ToBoolean(b.IntervalValues.Values[0]) == false)
                {
                    return new Tbool(false);
                }
            }
            
            // Short circuit 2:
            // If any input IsUnknown, return an unknown Tbool
            if (AnyAreUnknown(list)) { return new Tbool(); }
            
            // Else, apply the AND function to the inputs
            return ApplyFcnToTimeline(x => And(x), list);
        }
        
//		private static Tbool And(params Tbool[] list)
//		{
//            // Turn off "GetUnknowns" mode while analyzing short-circuits
//            bool originalMode = Facts.GetUnknowns;
//            Facts.GetUnknowns = false;
//            
//			// Short circuit 1:
//			// If any input is eternally false, return false
//			foreach (Tbool b in list)
//			{
//				if (b.IntervalValues.Count == 1 &&
//				    Convert.ToBoolean(b.IntervalValues.Values[0]) == false)
//				{
//                    // Restore "GetUnknowns" mode to its original state
//                    Facts.GetUnknowns = originalMode;
//                    
//					return new Tbool(false);
//				}
//			}
//			
//			// Short circuit 2:
//			// If any input IsUnknown, return an unknown Tbool
//			if (AnyAreUnknown(list)) 
//            {
//                // Restore "GetUnknowns" mode to its original state
//                Facts.GetUnknowns = originalMode;
//                
//                return new Tbool(); 
//            }
//			
//            // Restore "GetUnknowns" mode to its original state
//            Facts.GetUnknowns = originalMode;
//            
//			// Else, apply the AND function to the inputs
//			return ApplyFcnToTimeline(x => And(x), list);
//		}
		

        
		/// <summary>
		/// Private non-temporal AND function
		/// </summary>
		private static bool And(List<object> list)
		{
			foreach (bool b in list) 
			{
				if (b == false)
				{
					return false; 
				}
			}
	
			return true;
		}
		
		
		// ********************************************************************
		//	OR
		// ********************************************************************

		/// <summary>
		/// Temporal OR function: returns true when at least one of the inputs
		/// is true.
		/// </summary>
		public static Tbool operator | (Tbool tb1, Tbool tb2)
		{
			return Or(tb1,tb2);
		}
		
		public static Tbool operator | (Tbool tb, bool b)
		{
			return Or(tb, new Tbool(b));
		}
		
		public static Tbool operator | (bool b, Tbool tb)
		{
			return Or(tb, new Tbool(b));
		}
		
		/// <summary>
		/// Private temporal OR function
		/// </summary>
		private static Tbool Or(params Tbool[] list)
		{
			// Short circuit 1: If any input is eternally true, return true
			foreach (Tbool b in list)
			{
				if (b.IntervalValues.Count == 1 &&
				    Convert.ToBoolean(b.IntervalValues.Values[0]) == true)
				{
					return new Tbool(true);
				}
			}
			
			// Short circuit 2: If any input IsUnknown, return an unknown Tbool
			if (AnyAreUnknown(list)) { return new Tbool(); }
			
			// Else, apply the OR function to the inputs
			return ApplyFcnToTimeline(x => Or(x), list);
		}
		
		/// <summary>
		/// Private non-temporal OR function
		/// </summary>
		private static bool Or(List<object> list)
		{
			foreach (bool b in list) 
			{
				if (b == true)
				{
					return true; 
				}
			}
	
			return false;
		}
		
		
		// ********************************************************************
		//	NOT
		// ********************************************************************
		
		/// <summary>
		/// Temporal NOT function: returns true when the input is false and
		/// vice versa.
		/// </summary>
		public static Tbool operator ! (Tbool tb1)
		{
			return Not(tb1);
		}
		
		private static Tbool Not(Tbool input)
		{
			if (input.IsUnknown) { return new Tbool(); }
			
			Tbool result = new Tbool();
			
			foreach (KeyValuePair<DateTime,object> slice in input.IntervalValues)
			{
				bool r = !(bool)slice.Value;
				result.AddState(slice.Key, r);
			}
			
			return result;
		}
		
		
		// ********************************************************************
		//	XOR
		// ********************************************************************
		
		/// <summary>
		/// Temporal XOR function: returns true when the input is false and
		/// vice versa.
		/// </summary>
		public static Tbool operator ^ (Tbool tb1, Tbool tb2)
		{
			return Xor(tb1,tb2);
		}
		
		public static Tbool operator ^ (Tbool tb, bool b)
		{
			return Xor(tb, new Tbool(b));
		}
		
		public static Tbool operator ^ (bool b, Tbool tb)
		{
			return Xor(tb, new Tbool(b));
		}
		
		/// <summary>
		/// Private temporal XOR function
		/// </summary>
		private static Tbool Xor(params Tbool[] list)
		{
			// Result is unknown if any input is unknown
			if (AnyAreUnknown(list)) { return new Tbool(); }
			
			return ApplyFcnToTimeline(x => Xor(x), list);
		}
		
		/// <summary>
		/// Private non-temporal OR function
		/// </summary>
		private static bool Xor(List<object> list)
		{
			int countT = 0;
			foreach (bool v in list)
			{
				if (v == true)
				{
					countT++;
				}
			}
			
			if (countT > 1) { return false; }

			if (list.Contains(true)) { return true; }
			
			return false;			
		}
		
		
		// **********************************************************
		//	Other
		// **********************************************************
		
		/// <summary>
		/// Apply a function to the Tbool timeline.
		/// </summary>
		private static Tbool ApplyFcnToTimeline(Func<List<object>,object> fcn, params Tbool[] list)
		{
			Tbool result = new Tbool();
			
			foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(list))
			{	
				result.AddState(slice.Key, fcn(slice.Value));
			}
			
			return result.Lean;
		}
		
		
	}
	
	#pragma warning restore 660, 661
}

