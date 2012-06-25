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
    #pragma warning disable 660, 661
    
    public partial class Tbool
    {        
        
        // ********************************************************************
        //    AND
        // ********************************************************************
        
        /*
         *  "Truth table" for AND:
         * 
         *     If one input is False, returns False.
         *     Else if one input is Stub, returns Stub.
         *     Else if one input is Uncertain, returns Uncertain.
         *     Else if one input is Unstated, returns Unstated.
         *     Else, returns True.
         */
        
        /// <summary>
        /// Temporal AND function.
        /// </summary>
        public static Tbool operator & (Tbool tb1, Tbool tb2)
        {         
            return And(tb1,tb2);
        }
        
        /// <summary>
        /// Private temporal AND function
        /// </summary>
        private static Tbool And(Tbool t1, Tbool t2)
        {
            // Short circuit:
            // If any input is eternally false, return false
            if (t1.IsFalse || t2.IsFalse) return new Tbool(false);

            // Else, apply the AND function to the inputs
            Tbool result = new Tbool();
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(t1,t2))
            {    
                result.AddState(slice.Key, CoreAnd(slice.Value));
            }
            return result.Lean;
        }

        /// <summary>
        /// Private non-temporal AND function
        /// </summary>
        private static Hval CoreAnd(List<Hval> list)
        {
            // One false falsifies the conclusion
            foreach (Hval h in list)
            {
                if (h.IsFalse) return new Hval(false);
            }

            // Any higher-precedence states go next
            Hstate top = PrecedingState(list);
            if (top != Hstate.Known) return new Hval(null,top);

            // Otherwise, true
            return new Hval(true);
        }

        
        // ********************************************************************
        //    OR
        // ********************************************************************

        /*
         *  "Truth table" for OR:
         * 
         *     If one input is False, returns True.
         *     Else if one input is Stub, returns Stub.
         *     Else if one input is Uncertain, returns Uncertain.
         *     Else if one input is Unstated, returns Unstated.
         *     Else, returns False.
         */
        
        /// <summary>
        /// Temporal OR function.
        /// </summary>
        public static Tbool operator | (Tbool tb1, Tbool tb2)
        {
            return Or(tb1,tb2);
        }

        /// <summary>
        /// Private temporal OR function
        /// </summary>
        private static Tbool Or(Tbool t1, Tbool t2)
        {
            // Short circuit:
            // If any input is eternally false, return false
            if (t1.IsTrue || t2.IsTrue) return new Tbool(true);

            // Else, apply the AND function to the inputs
            Tbool result = new Tbool();
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(t1,t2))
            {    
                result.AddState(slice.Key, CoreOr(slice.Value));
            }
            return result.Lean;
        }

        /// <summary>
        /// Private non-temporal OR function
        /// </summary>
        private static Hval CoreOr(List<Hval> list)
        {
            // One true makes the conclusion true
            foreach (Hval h in list)
            {
                if (h.IsTrue) return new Hval(true);
            }

            // Any higher-precedence states go next
            Hstate top = PrecedingState(list);
            if (top != Hstate.Known) return new Hval(null,top);

            // Otherwise, false
            return new Hval(false);
        }

        
        // ********************************************************************
        //    NOT
        // ********************************************************************
        
        /// <summary>
        /// Temporal NOT function: returns true when the input is false and
        /// vice versa.
        /// </summary>
        public static Tbool operator ! (Tbool tb)
        {
            Tbool result = new Tbool();
            
            foreach (KeyValuePair<DateTime,Hval> slice in tb.IntervalValues)
            {
                if (slice.Value.IsKnown)
                {
                    bool bval = !Convert.ToBoolean(slice.Value.Val);
                    result.AddState(slice.Key, bval);
                }
                else
                {
                    result.AddState(slice.Key, slice.Value);
                }
            }
            
            return result.Lean;
        }
        
        // ********************************************************************
        //    XOR
        // ********************************************************************
        
        /// <summary>
        /// Temporal XOR function: returns true when the input is false and
        /// vice versa.
        /// </summary>
        public static Tbool operator ^ (Tbool tb1, Tbool tb2)
        {
            // Temporarily using the ^ operator for Facts.Either()!!!
            return Facts.Either(tb1,tb2);
//            return Xor(tb1,tb2);
        }
        
        /// <summary>
        /// Private temporal XOR function
        /// </summary>
//        private static Tbool Xor(params Tbool[] list)
//        {
//            // Result is unknown if any input is unknown
//            if (AnyAreUnknown(list)) { return new Tbool(); }
//            
//            return ApplyFcnToTimeline(x => Xor(x), list);
//        }
//        
//        /// <summary>
//        /// Private non-temporal OR function
//        /// </summary>
//        private static bool Xor(List<object> list)
//        {
//            int countT = 0;
//            foreach (bool v in list)
//            {
//                if (v == true)
//                {
//                    countT++;
//                }
//            }
//            
//            if (countT > 1) { return false; }
//
//            if (list.Contains(true)) { return true; }
//            
//            return false;            
//        }
        
    }
    
    #pragma warning restore 660, 661
}

