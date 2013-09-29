// Copyright (c) 2012-2013 Hammura.bi LLC
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
    
namespace Akkadian
{
    public partial class Tnum
    {       
        /// <summary>
        /// Returns true when one Tnum is equal to another
        /// </summary>
        public static Tbool operator == (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline<Tbool>(x => NumericEqualTo(x), tn1, tn2);
        }
        private static Hval NumericEqualTo(List<Hval> list)
        {
            return new Hval(Convert.ToDecimal(list[0].Val) == Convert.ToDecimal(list[1].Val));
        }

        /// <summary>
        /// Returns true when one Tnum is not equal to another
        /// </summary>
        public static Tbool operator != (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline<Tbool>(x => NumericNotEqualTo(x), tn1, tn2);
        }
        private static Hval NumericNotEqualTo(List<Hval> list)
        {
            return new Hval(Convert.ToDecimal(list[0].Val) != Convert.ToDecimal(list[1].Val));
        }

        /// <summary>
        /// Returns true when one Tnum is greather than another
        /// </summary>
        public static Tbool operator > (Tnum tn1, Tnum tn2)
        {
            return ApplyFcnToTimeline<Tbool>(x => GrTh(x), tn1, tn2);
        }
        private static Hval GrTh(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) > Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tnum is greather than or equal to another.
        /// </summary>
        public static Tbool operator >= (Tnum tn1, Tnum tn2)
        {
            return ApplyFcnToTimeline<Tbool>(x => GrEq(x), tn1, tn2);
        }
        private static Hval GrEq(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) >= Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tnum is less than another.
        /// </summary>
        public static Tbool operator < (Tnum tn1, Tnum tn2)
        {
            return ApplyFcnToTimeline<Tbool>(x => LsTh(x), tn1, tn2);
        }
        private static Hval LsTh(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) < Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tnum is less than or equal to another.
        /// </summary>
        public static Tbool operator <= (Tnum tn1, Tnum tn2)
        {
            return ApplyFcnToTimeline<Tbool>(x => LsEq(x), tn1, tn2);
        }
        private static Hval LsEq(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) <= Convert.ToDecimal(list[1].Val);
        }
    }
}