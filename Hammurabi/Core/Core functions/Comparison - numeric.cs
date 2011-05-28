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
    public partial class Tnum
    {
                
        /// <summary>
        /// Returns true when one Tnum is equal to another
        /// </summary>
        public static Tbool operator == (Tnum hn1, Tnum hn2)    
        {
            return EqualTo(hn1,hn2);
        }
        
        private static Tbool EqualTo(Tnum tn1, Tnum tn2)
        {
            if (AnyAreUnknown(tn1, tn2)) { return new Tbool(); }
            
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(tn1,tn2))
            {    
                bool areEqual = Convert.ToDecimal(slice.Value[0]) == Convert.ToDecimal(slice.Value[1]);
                result.AddState(slice.Key, areEqual);
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when one Tnum is not equal to another
        /// </summary>
        public static Tbool operator != (Tnum hn1, Tnum hn2)    
        {
            return !EqualTo(hn1,hn2);
        }
        
        private static Tbool NotEqualTo(Tnum tn1, Tnum tn2)
        {
            return !EqualTo(tn1,tn2);
        }
        
        /// <summary>
        /// Returns true when one Tnum is greather than another
        /// </summary>
        public static Tbool operator > (Tnum hn1, Tnum hn2)    
        {
            return GreaterThan(hn1,hn2);
        }
        
        private static Tbool GreaterThan(Tnum tn1, Tnum tn2)
        {    
            if (AnyAreUnknown(tn1, tn2)) { return new Tbool(); }
            
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(tn1,tn2))
            {    
                bool isGT = Convert.ToDecimal(slice.Value[0]) > Convert.ToDecimal(slice.Value[1]);
                result.AddState(slice.Key, isGT);
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when one Tnum is greather than or equal to another
        /// </summary>
        public static Tbool operator >= (Tnum hn1, Tnum hn2)    
        {
            return GreaterThanOrEqualTo(hn1,hn2);
        }
        
        private static Tbool GreaterThanOrEqualTo(Tnum tn1, Tnum tn2)
        {
            return GreaterThan(tn1,tn2) || EqualTo(tn1,tn2);
        }

        /// <summary>
        /// Returns true when one Tnum is less than another
        /// </summary>
        public static Tbool operator < (Tnum hn1, Tnum hn2)    
        {
            return LessThan(hn1,hn2);
        }
        
        private static Tbool LessThan(Tnum tn1, Tnum tn2)
        {
            return !GreaterThanOrEqualTo(tn1, tn2);
        }
                
        /// <summary>
        /// Returns true when one Tnum is less than or equal to another
        /// </summary>
        public static Tbool operator <= (Tnum hn1, Tnum hn2)    
        {
            return LessThanOrEqualTo(hn1,hn2);
        }
        
        private static Tbool LessThanOrEqualTo(Tnum tn1, Tnum tn2)
        {
            return LessThan(tn1,tn2) || EqualTo(tn1,tn2);
        }
        
    }
}
