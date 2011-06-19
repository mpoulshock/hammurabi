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
		  * TODO: Implement unknowns
          *  
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
        public static T Switch<T>(Tbool case1, T value1, T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, defaultValue);
        }
        
        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2, 
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, defaultValue);
        }
        
        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, defaultValue);
        }
        
        
        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  Tbool case4, T value4,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, case4, value4, defaultValue);
        }
        
        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  Tbool case4, T value4,
                                  Tbool case5, T value5,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, defaultValue);
        }
        
  
        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  Tbool case4, T value4,
                                  Tbool case5, T value5,
                                  Tbool case6, T value6,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                                 case6, value6, defaultValue);
        }

        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  Tbool case4, T value4,
                                  Tbool case5, T value5,
                                  Tbool case6, T value6,
                                  Tbool case7, T value7,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                                 case6, value6, case7, value7, defaultValue);
        }

        public static T Switch<T>(Tbool case1, T value1, 
                                  Tbool case2, T value2,
                                  Tbool case3, T value3,
                                  Tbool case4, T value4,
                                  Tbool case5, T value5,
                                  Tbool case6, T value6,
                                  Tbool case7, T value7,
                                  Tbool case8, T value8,
                                  T defaultValue) where T : Tvar
        {
            return SwitchCore<T>(case1, value1, case2, value2, case3, value3, case4, value4, case5, value5, 
                                 case6, value6, case7, value7, case8, value8, defaultValue);
        }
        
        /// <summary>
        /// Temporal SWITCH function that takes unrestricted input parameters.
        /// </summary>
        private static T SwitchCore<T>(params Tvar[] inputs) where T : Tvar
        {
            // This will force all inputs to be evaluated (there will be no short-circuiting)
            if (AnyAreUnknown(inputs)) { return (T)Auxiliary.ReturnProperTvar<T>(); }
            
            T result = (T)Auxiliary.ReturnProperTvar<T>();
            
            // Apply the non-temporal switch function across the timeline
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(inputs))
            {   
                result.AddState(slice.Key, SwitchCore(slice.Value));
            }
            
            return result.LeanTvar<T>();
        }
        
		/// <summary>
		/// Non-temporal SWITCH function.
		/// </summary>
        private static object SwitchCore(List<object> list)
        {
            // Iterate through pairs of inputs
            for (int arg=0; arg < list.Count-2; arg = arg + 2)
            {
                if (Convert.ToBoolean(list[arg]) == true)
                {
                    return list[arg+1];
                }
            }
            
            // Else, return the default value...
            return list[list.Count-1];
        }

        
        
        
        
        
        
        
        
	}
}