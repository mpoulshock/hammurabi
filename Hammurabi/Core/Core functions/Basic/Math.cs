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

namespace Hammurabi
{
    #pragma warning disable 660, 661
    
    public partial class Tnum
    {        
        /// <summary>
        /// Adds two Tnums together.
        /// </summary>
        public static Tnum operator + (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline<Tnum>(x => CoreSum(x), tn1, tn2);
        }
        private static Hval CoreSum(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) + Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Subtracts one Tnum from another.
        /// </summary>
        public static Tnum operator - (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline<Tnum>(x => Minus(x), tn1, tn2);
        }
        private static Hval Minus(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) - Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Multiplies two Tnums together.
        /// </summary>
        public static Tnum operator * (Tnum tn1, Tnum tn2)    
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                decimal val1 = Convert.ToDecimal(slice.Value [0].Val);
                decimal val2 = Convert.ToDecimal(slice.Value [1].Val);

                // Short circuit 1
                if (val1 == 0 || val2 == 0)
                {
                    result.AddState(slice.Key, new Hval(0));
                }
                else if (top != Hstate.Known)      // Short circuit 2
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                               // Do the math
                {
                    decimal prod = val1 * val2;
                    result.AddState(slice.Key, new Hval(prod));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Divides one Tnum by another. 
        /// </summary>
        public static Tnum operator / (Tnum tn1, Tnum tn2)    
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                decimal denominator = Convert.ToDecimal(slice.Value[1].Val);

                if (denominator == 0)   // Short circuit 1: Div-by-zero
                {
                    result.AddState(slice.Key, new Hval(null));
                }
                else if (top != Hstate.Known)                     // Short circuit 2: Hstates
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                                              // Do the math
                {
                    decimal r = Convert.ToDecimal(slice.Value[0].Val) / denominator;
                    result.AddState(slice.Key, new Hval(r));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Temporal modulo function. 
        /// </summary>
        public static Tnum operator % (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline<Tnum>(x => Mod(x), tn1, tn2);
        }
        private static Hval Mod(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) % Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Temporal absolute value function
        /// </summary>
        public Tnum Abs
        {
            get
            {
                return Switch<Tnum>(() => this >= 0, () => this, () => 0 - this);
            }
        }

        
        // ********************************************************************
        // ROUNDING FUNCTIONS
        // ********************************************************************
        
        /// <summary>
        /// Rounds a value to the nearest multiple of a given number.  By default, 
        /// it rounds up in case of a tie.
        /// </summary>
        public Tnum RoundToNearest(Tnum multiple)
        {
            return RoundToNearest(multiple,false);
        }

        public Tnum RoundToNearest(Tnum multiple, Tbool breakTieByRoundingDown)
        {
            Tnum diff = this % multiple;
            return Switch<Tnum>(() => diff > multiple / 2, () => this - diff + multiple,
                                ()=> diff < multiple / 2 || breakTieByRoundingDown, ()=> this - diff,
                                () => true, () => this - diff + multiple);
        }

        /// <summary>
        /// Rounds a value up to the next multiple of a given number.
        /// </summary>
        public Tnum RoundUp(Tnum multiple)
        {
            return Switch<Tnum>(() => this % multiple != 0, () => this - (this % multiple) + multiple,
                                () => true, () => this);
        }        
        
        /// <summary>
        /// Rounds a value down to the next multiple of a given number.
        /// </summary>
        public Tnum RoundDown(Tnum multiple)
        {
            return Switch<Tnum>(() => this % multiple != 0, () => this - (this % multiple),
                                () => true, () => this);
        }
    }
    
    #pragma warning restore 660, 661
}