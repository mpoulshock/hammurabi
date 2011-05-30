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
    #pragma warning disable 660, 661
    
    public partial class Tnum
    {
        // ********************************************************************
        // ADDITION
        // ********************************************************************
        
        /// <summary>
        /// Adds two Tnums together.
        /// </summary>
        public static Tnum operator + (Tnum tn1, Tnum tn2)    
        {
            if (AnyAreUnknown(tn1,tn2)) { return new Tnum(); }
            
            return ApplyFcnToTimeline(x => Sum(x), tn1, tn2);
        }
        
        /// <summary>
        /// Private non-temporal SUM function.
        /// </summary>
        public static decimal Sum(List<object> list)
        {
            decimal sum = 0;
            foreach (object v in list) 
            {
                sum += Convert.ToDecimal(v); 
            }
            return sum;
        }
        
        
        // ********************************************************************
        // SUBTRACTION
        // ********************************************************************
        
        /// <summary>
        /// Subtracts one Tnum from another.
        /// </summary>
        public static Tnum operator - (Tnum tn1, Tnum tn2)    
        {
            if (AnyAreUnknown(tn1,tn2)) { return new Tnum(); }
            
            return ApplyFcnToTimeline(x => Subtract(x), tn1, tn2);
        }
        
        /// <summary>
        /// Private non-temporal SUBTRACT function.
        /// </summary>
        private static decimal Subtract(List<object> list)
        {
            decimal sum = Convert.ToDecimal(list[0]) * 2;
            
            foreach (object v in list) 
            {
                sum -= Convert.ToDecimal(v); 
            }
            
            return sum;
        }
        
        
        // ********************************************************************
        // MULTIPLICATION
        // ********************************************************************
        
        /// <summary>
        /// Multiplies two Tnums together.
        /// </summary>
        public static Tnum operator * (Tnum tn1, Tnum tn2)    
        {
            // Short circuit 1: If any eternal zeros, return zero
            if ((tn1.IntervalValues.Count == 1 && Convert.ToDecimal(tn1.IntervalValues.Values[0]) == 0) ||
                (tn2.IntervalValues.Count == 1 && Convert.ToDecimal(tn2.IntervalValues.Values[0]) == 0))
            {
                return new Tnum(0);
            }
            
            // Short circuit 2: If any unknowns, return unknown
            if (AnyAreUnknown(tn1, tn2)) { return new Tnum(); }
            
            return ApplyFcnToTimeline(x => Prod(x), tn1, tn2);
        }
        
        /// <summary>
        /// Private non-temporal PRODUCT function
        /// </summary>
        private static decimal Prod(List<object> list)
        {
            decimal sum = 1;
            
            foreach (object v in list) 
            {
                sum *= Convert.ToDecimal(v); 
            }
            
            return sum;
        }
        
        
        // ********************************************************************
        // DIVISION
        // ********************************************************************
        
        /// <summary>
        /// Divides one Tnum by another. 
        /// </summary>
        public static Tnum operator / (Tnum tn1, Tnum tn2)    
        {
            if (AnyAreUnknown(tn1, tn2)) { return new Tnum(); }
            
            return ApplyFcnToTimeline(x => Divide(x), tn1, tn2);
        }
        
        /// <summary>
        /// Private non-temporal DIVISION function. 
        /// </summary>
        private static decimal Divide(List<object> list)
        {
            // TODO: Handle div-by-zero errors
            
            return Convert.ToDecimal(list[0]) / Convert.ToDecimal(list[1]);
        }
        
        
        // ********************************************************************
        // MODULO
        // ********************************************************************
        
        /// <summary>
        /// Temporal MODULO function. 
        /// </summary>
        public static Tnum operator % (Tnum tn1, Tnum tn2)    
        {
            if (AnyAreUnknown(tn1, tn2)) { return new Tnum(); }
            
            return ApplyFcnToTimeline(x => Modulo(x), tn1, tn2);
        }
        
        /// <summary>
        /// Private non-temporal MODULO function. 
        /// </summary>
        private static decimal Modulo(List<object> list)
        {
            return Convert.ToDecimal(list[0]) % Convert.ToDecimal(list[1]);
        }
        
        
        // ********************************************************************
        // ABSOLUTE VALUE
        // ********************************************************************
        
        /// <summary>
        /// Temporal ABSOLUTE VALUE function
        /// </summary>
        public Tnum Abs
        {
            get
            {
                if (this.IsUnknown) { return new Tnum(); }
            
                Tnum result = new Tnum();
                
                foreach (KeyValuePair<DateTime,object> slice in this.IntervalValues)
                {
                    result.AddState(slice.Key, System.Math.Abs(Convert.ToDecimal(slice.Value)));
                }
                
                return result;
            }
        }

        
        // ********************************************************************
        // ROUNDING FUNCTIONS
        // ********************************************************************
        
        /// <summary>
        /// Temporal ROUND TO NEAREST function.  Rounds a value to the nearest
        /// multiple of a given number.  By default, it rounds up in case of
        /// a tie.
        /// </summary>
        public Tnum RoundToNearest(double multiple)
        {
            return RoundToNearest(multiple,false);
        }
        
        public Tnum RoundToNearest(double multiple, bool breakTieByRoundingDown)
        {
            if (this.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,object> de in this.TimeLine)
            {
                decimal val = RoundToNearest(Convert.ToString(de.Value), multiple, breakTieByRoundingDown);
                
                result.AddState(de.Key, val);
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Private non-temporal ROUND TO NEAREST function
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
        /// Temporal ROUND UP function.  Rounds a value up to the next multiple
        /// of a given number.
        /// </summary>
        public Tnum RoundUp(double multiple)
        {
            if (this.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,object> de in this.TimeLine)
            {
                decimal val = CoreRoundUp(Convert.ToString(de.Value), multiple);
                
                result.AddState(de.Key, val);
            }

            return result.Lean;
        }        
        
        /// <summary>
        /// Private non-temporal ROUND UP function.
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
        /// Temporal ROUND DOWN function.  Rounds a value down to the next
        /// multiple of a given number.
        /// </summary>
        public Tnum RoundDown(double multiple)
        {
            if (this.IsUnknown) { return new Tnum(); }
            
            Tnum result = new Tnum();
            
            foreach(KeyValuePair<DateTime,object> de in this.TimeLine)
            {
                decimal val = CoreRoundDown(Convert.ToString(de.Value), multiple);
                
                result.AddState(de.Key, val);
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Private non-temporal ROUND DOWN function.
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
