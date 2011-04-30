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
		/*
		  * TODO: 
		  *  - Implement unknowns
		  *  - Handle rule exhaustion
		  *  - Handle malformed inputs somehow or another
		  */ 

		/// <summary>
		/// Temporal SWITCH function.  Returns a Tnum when its associated Tbool
		/// is true.  Used like: Switch(Tbool1,Tnum1, Tbool2,Tnum2, ...).  
		/// Returns Tnum1 if Tbool2 is true, else Tnum2 if Tbool2 is true, etc.  
		/// The values in the output Tnum are nullable decimals.
		/// </summary>
		public static Tnum Switch(params object[] inputs)
		{
			// Validate inputs and convert non-temporal inputs to temporal ones
			
			Tvar[] temporalInputs = new Tvar[inputs.Length];

			for (int arg=0; arg < inputs.Length; arg = arg + 2)
			{
				// If first object in the pair is not already a Tbool, make it a Tbool
				if (!Object.ReferenceEquals(inputs[arg].GetType(), new Tbool().GetType()))
				{
					temporalInputs[arg] = new Tbool(Convert.ToBoolean(inputs[arg]));
				}
				else
				{
					temporalInputs[arg] = (Tbool)inputs[arg];
				}
				
				// If second object in pair is not already a Tnum, make it a Tnum
				if (!Object.ReferenceEquals(inputs[arg+1].GetType(), new Tnum().GetType()))
				{
					temporalInputs[arg+1] = new Tnum(Convert.ToDecimal(inputs[arg+1]));
				}
				else
				{
					temporalInputs[arg+1] = (Tnum)inputs[arg+1];
				}
			}
			
			// Apply the non-temporal switch function across the timeline
			
			Tnum result = new Tnum();
			
			foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(temporalInputs))
			{	
				result.AddState(slice.Key, Switch(slice.Value));
			}
			
			return result.Lean;
		}

		/// <summary>
		/// Private non-temporal SWITCH function
		/// </summary>
		private static decimal? Switch(List<object> list)
		{
			// Iterate through pairs of inputs
			for (int arg=0; arg < list.Count; arg = arg + 2)
			{
				if (Convert.ToBoolean(list[arg]) == true)
				{
					try
					{
						return Convert.ToDecimal(list[arg+1]);
					}
					catch
					{
						return null;
					}
				}
			}
			
			return null;
		}

		
	}
}