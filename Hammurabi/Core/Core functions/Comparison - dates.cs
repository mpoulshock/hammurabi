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
    public partial class Tdate
    {

        /// <summary>
        /// Returns true when one Tdate is equal to another.
        /// </summary>
        public static Tbool operator == (Tdate td1, Tdate td2)
        {
            return EqualTo(td1,td2);
        }
        
        /// <summary>
        /// Returns true when one Tdate is not equal to another.
        /// </summary>
        public static Tbool operator != (Tdate td1, Tdate td2)
        {
            return !EqualTo(td1,td2);
        }
        
        /// <summary>
        /// Returns true when one Tdate is later than another.
        /// </summary>
        public static Tbool operator > (Tdate td1, Tdate td2)
        {
            return IsAfter(td1,td2);
        }
        
        private static Tbool IsAfter(Tdate td1, Tdate td2)
        {
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(td1,td2))
            {
                // Any higher-precedence states go next
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    bool isAfter = Convert.ToDateTime(slice.Value[0].Val) > Convert.ToDateTime(slice.Value[1].Val);
                    result.AddState(slice.Key, isAfter);
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Returns true when one Tdate is the same as or later than another.
        /// </summary>
        public static Tbool operator >= (Tdate td1, Tdate td2)
        {
            return IsAtOrAfter(td1,td2);
        }
        
        private static Tbool IsAtOrAfter(Tdate td1, Tdate td2)
        {
            return IsAfter(td1,td2) | EqualTo(td1,td2);
        }

        /// <summary>
        /// Returns true when one Tdate is earlier than another.
        /// </summary>
        public static Tbool operator < (Tdate td1, Tdate td2)
        {
            return !IsAfter(td1,td2);
        }
        
        private static Tbool IsBefore(Tdate td1, Tdate td2)
        {
            return !IsAtOrAfter(td1,td2);
        }
                
        /// <summary>
        /// Returns true when one Tdate is the same as or earlier than another.
        /// </summary>
        public static Tbool operator <= (Tdate td1, Tdate td2)
        {
            return IsBefore(td1,td2) | EqualTo(td1,td2);
        }
    }
}
