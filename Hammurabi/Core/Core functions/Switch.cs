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
          *  - Make generic Switch function (for Tbool, Tstr, Tdate)
		  */ 

		/// <summary>
		/// Returns a Tnum when its associated Tbool is true.  
		/// </summary>
        /// <remarks>
        /// Similar in principle to a C# switch statement.
        /// Sample usage: Switch(Tbool1,Tnum1, Tbool2,Tnum2, ...).  
        /// Returns Tnum1 if Tbool2 is true, else Tnum2 if Tbool2 is true, etc.  
        /// The values in the output Tnum are nullable decimals.
        /// </remarks>
		public static Tnum Switch(Tbool case1, Tnum value1, Tnum defaultValue)
        {
            return SwitchCore(case1, value1, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1, 
                                  Tbool case2, Tnum value2, 
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1, 
                                  Tbool case2, Tnum value2, 
                                  Tbool case3, Tnum value3, 
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1,
                                  Tbool case2, Tnum value2,
                                  Tbool case3, Tnum value3,
                                  Tbool case4, Tnum value4,
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, case4, value4, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1,
                                  Tbool case2, Tnum value2,
                                  Tbool case3, Tnum value3,
                                  Tbool case4, Tnum value4,
                                  Tbool case5, Tnum value5,
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1,
                                  Tbool case2, Tnum value2,
                                  Tbool case3, Tnum value3,
                                  Tbool case4, Tnum value4,
                                  Tbool case5, Tnum value5,
                                  Tbool case6, Tnum value6,
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                              case6, value6, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1,
                                  Tbool case2, Tnum value2,
                                  Tbool case3, Tnum value3,
                                  Tbool case4, Tnum value4,
                                  Tbool case5, Tnum value5,
                                  Tbool case6, Tnum value6,
                                  Tbool case7, Tnum value7,
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                              case6, value6, case7, value7, defaultValue);
        }
        
        public static Tnum Switch(Tbool case1, Tnum value1,
                                  Tbool case2, Tnum value2,
                                  Tbool case3, Tnum value3,
                                  Tbool case4, Tnum value4,
                                  Tbool case5, Tnum value5,
                                  Tbool case6, Tnum value6,
                                  Tbool case7, Tnum value7,
                                  Tbool case8, Tnum value8,
                                  Tnum defaultValue)
        {
            return SwitchCore(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                              case6, value6, case7, value7, case8, value8, defaultValue);
        }
        
        /// <summary>
        /// Temporal SWITCH function that takes unrestricted input parameters.
        /// </summary>
        private static Tnum SwitchCore(params Tvar[] inputs)
        {
            // This will force all inputs to be evaluated (there will be no short-circuiting)
            if (AnyAreUnknown(inputs)) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            // Apply the non-temporal switch function across the timeline
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(inputs))
            {   
                result.AddState(slice.Key, SwitchCore(slice.Value));
            }
            
            return result.Lean;
        }

		/// <summary>
		/// Non-temporal SWITCH function.
		/// </summary>
		private static decimal? SwitchCore(List<object> list)
		{
			// Iterate through pairs of inputs
			for (int arg=0; arg < list.Count-2; arg = arg + 2)
			{
				if (Convert.ToBoolean(list[arg]) == true)
				{
                    return Auxiliary.ToNullaDecimal(list[arg+1]);
				}
			}
            
            // Else, return the default value...
            return Auxiliary.ToNullaDecimal(list[list.Count-1]);
		} 
	}
}