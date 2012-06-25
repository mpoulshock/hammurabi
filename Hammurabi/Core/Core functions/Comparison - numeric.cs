// Copyright (c) 2012 Hammura.bi LLC
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
        public static Tbool operator == (Tnum tn1, Tnum tn2)    
        {
            return EqualTo(tn1,tn2);
        }

        /// <summary>
        /// Returns true when one Tnum is equal to another
        /// </summary>
        /// <remarks>
        /// This method is needed (in addition to Tvar.EqualTo) in order to convert the
        /// Hval object to a decimal.
        /// </remarks>
        private static Tbool EqualTo(Tnum tn1, Tnum tn2)
        {
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {
                Hval areEqual = CoreNumericEqualTo(slice.Value[0], slice.Value[1]);
                result.AddState(slice.Key, areEqual);
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Determines whether two Hvals are numerically equal.
        /// </summary>
        private static Hval CoreNumericEqualTo(Hval ob1, Hval ob2)
        {
            Hstate top = PrecedingState(ob1,ob2);
            if (top != Hstate.Known) return new Hval(null,top);

            bool result = Convert.ToDecimal(ob1.Val) == Convert.ToDecimal(ob2.Val);
            return new Hval(result);
        }

        /// <summary>
        /// Returns true when one Tnum is not equal to another
        /// </summary>
        public static Tbool operator != (Tnum hn1, Tnum hn2)    
        {
            return !EqualTo(hn1,hn2);
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
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                    result.AddState(slice.Key, top);
                }
                else
                {
                    bool isGT = Convert.ToDecimal(slice.Value[0].Val) > Convert.ToDecimal(slice.Value[1].Val);
                    result.AddState(slice.Key, isGT);
                }
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when one Tnum is greather than or equal to another.
        /// </summary>
        public static Tbool operator >= (Tnum tn1, Tnum tn2)    
        {
            return GreaterThan(tn1,tn2) || EqualTo(tn1,tn2);
        }

        /// <summary>
        /// Returns true when one Tnum is less than another.
        /// </summary>
        public static Tbool operator < (Tnum tn1, Tnum tn2)    
        {
            return !GreaterThan(tn1,tn2) && !EqualTo(tn1,tn2);
        }
                
        /// <summary>
        /// Returns true when one Tnum is less than or equal to another.
        /// </summary>
        public static Tbool operator <= (Tnum tn1, Tnum tn2)    
        {
            return !GreaterThan(tn1,tn2);
        }
    }
}
