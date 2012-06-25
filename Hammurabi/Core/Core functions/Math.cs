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
    
    public partial class Tnum
    {        
        /// <summary>
        /// Adds two Tnums together.
        /// </summary>
        public static Tnum operator + (Tnum tn1, Tnum tn2)    
        {
            return ApplyFcnToTimeline(x => Sum(x), tn1, tn2);
        }

        /// <summary>
        /// Non-temporal sum function.
        /// </summary>
        public static Hval Sum(List<Hval> list)
        {
            Hstate top = PrecedingState(list);
            if (top != Hstate.Known)
            {
                return new Hval(null,top);
            }

            decimal sum = 0;
            foreach (Hval v in list) 
            {
                sum += Convert.ToDecimal(v.Val); 
            }
            return new Hval(sum);
        }

        /// <summary>
        /// Subtracts one Tnum from another.
        /// </summary>
        public static Tnum operator - (Tnum tn1, Tnum tn2)    
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known)
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    decimal sum = Convert.ToDecimal(slice.Value[0].Val) * 2;
                    foreach (Hval v in slice.Value) 
                    {
                        sum -= Convert.ToDecimal(v.Val); 
                    }
                    result.AddState(slice.Key, new Hval(sum));
                }
            }
            
            return result.Lean;
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

                if (AnyHvalIsAZero(slice.Value))   // Short circuit 1
                {
                    result.AddState(slice.Key, new Hval(0));
                }
                else if (top != Hstate.Known)      // Short circuit 2
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                               // Do the math
                {
                    decimal prod = 1;
                    foreach (Hval v in slice.Value) 
                    {
                        prod *= Convert.ToDecimal(v.Val); 
                    }
                    result.AddState(slice.Key, new Hval(prod));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Determines whether any input value is a 0.
        /// </summary>
        private static bool AnyHvalIsAZero(List<Hval> list)
        {
            foreach (Hval h in list)
            {
                if (Convert.ToDecimal(h.Val) == 0) return true;
            }
            return false;
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
                    result.AddState(slice.Key, new Hval(Hstate.Uncertain));
                }
                else if (top != Hstate.Known)                     // Short circuit 2: Hstates
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                                              // Do the math
                {
                    decimal r = Convert.ToDecimal(slice.Value[0].Val) / Convert.ToDecimal(slice.Value[1].Val);
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
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known)
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    decimal r = Convert.ToDecimal(slice.Value[0].Val) % Convert.ToDecimal(slice.Value[1].Val);
                    result.AddState(slice.Key, new Hval(r));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Temporal absolute value function
        /// </summary>
        public Tnum Abs
        {
            get
            {
                Tnum result = new Tnum();
                
                foreach (KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
                {
                    if (!slice.Value.IsKnown)
                    {
                        result.AddState(slice.Key, slice.Value);
                    }
                    else
                    {
                        result.AddState(slice.Key, System.Math.Abs(Convert.ToDecimal(slice.Value.Val)));
                    }
                }
                
                return result;
            }
        }

        
        // ********************************************************************
        // ROUNDING FUNCTIONS
        // ********************************************************************
        
        /// <summary>
        /// Rounds a value to the nearest multiple of a given number.  By default, 
        /// it rounds up in case of a tie.
        /// </summary>
        public Tnum RoundToNearest(double multiple)
        {
            return RoundToNearest(multiple,false);
        }
        
        public Tnum RoundToNearest(double multiple, bool breakTieByRoundingDown)
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
            {
                if (!de.Value.IsKnown)
                {
                    result.AddState(de.Key, de.Value);
                }
                else
                {
                    decimal val = RoundToNearest(Convert.ToString(de.Value.Val), multiple, breakTieByRoundingDown);
                    result.AddState(de.Key, new Hval(val));
                }
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Non-temporal round-to-nearest function
        /// </summary>
        private static decimal RoundToNearest(string theNum, double multiple, bool breakTieByRoundingDown)
        {
            decimal num = Convert.ToDecimal(theNum);
            decimal mult = Convert.ToDecimal(multiple);
            decimal diff = num % mult;
            
            if (diff > mult / 2)
            {
                return num - diff + mult;
            }           
            else if (diff < mult / 2 || breakTieByRoundingDown)
            {
                return num - diff;
            }
            
            return num - diff + mult;
        }
        
        /// <summary>
        /// Rounds a value up to the next multiple of a given number.
        /// </summary>
        public Tnum RoundUp(double multiple)
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
            {
                if (!de.Value.IsKnown)
                {
                    result.AddState(de.Key, de.Value);
                }
                else
                {
                    decimal val = CoreRoundUp(Convert.ToString(de.Value.Val), multiple);
                    result.AddState(de.Key, new Hval(val));
                }
            }

            return result.Lean;
        }        
        
        /// <summary>
        /// Non-temporal round-up function.
        /// </summary>
        private static decimal CoreRoundUp(string theNum, double multiple2)
        {
            decimal num = Convert.ToDecimal(theNum);
            decimal multiple = Convert.ToDecimal(multiple2);
            decimal diff = num % multiple;
            decimal result = num;
            
            if (diff != 0)
            {
                result = num - diff + multiple;
            }

            return result;
        }
        
        /// <summary>
        /// Rounds a value down to the next multiple of a given number.
        /// </summary>
        public Tnum RoundDown(double multiple)
        {
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
            {
                if (!de.Value.IsKnown)
                {
                    result.AddState(de.Key, de.Value);
                }
                else
                {
                    decimal val = CoreRoundDown(Convert.ToString(de.Value.Val), multiple);
                    result.AddState(de.Key, new Hval(val));
                }
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Non-temporal round-down function.
        /// </summary>
        private static decimal CoreRoundDown(string theNum, double multiple2)
        {
            decimal num = Convert.ToDecimal(theNum);
            decimal multiple = Convert.ToDecimal(multiple2);
            decimal diff = num % multiple;
            decimal result = num;
            
            if (diff != 0)
            {
                result = num - diff;
            }

            return result;
        }

    }
    
    #pragma warning restore 660, 661
}