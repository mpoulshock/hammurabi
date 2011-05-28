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
    
    /// <summary>
    /// An temporal object that represents boolean values along a timeline.
    /// </summary>
    public partial class Tbool : Tvar
    {
        
        /// <summary>
        /// Constructs an "unknown" Tbool - that is, one with no states 
        /// </summary>
        public Tbool()
        {
        }
        
        /// <summary>
        /// Constructs a Tbool that is eternally set to a specified boolean value
        /// </summary>
        public Tbool(bool? val)
        {
            this.SetEternally(val);
        }
        
        /// <summary>
        /// Implicitly converts bools to Tbools.
        /// </summary>
        public static implicit operator Tbool(bool b) 
        {
            return new Tbool(b);
        }
        
        /// <summary>
        /// Removes redundant intervals from the Tbool. 
        /// </summary>
        public Tbool Lean
        {
            get
            {
                return this.LeanTvar<Tbool>();
            }
        }

        /// <summary>
        /// Returns the value of a Tbool at a specified point in time. 
        /// </summary>
        public Tbool AsOf(DateTime dt)
        {
            if (this.IsUnknown) { return new Tbool(); }
            
            return (Tbool)this.AsOf<Tbool>(dt);
        }
        
        /// <summary>
        /// Returns true if the Tbool always has a specified boolean value. 
        /// </summary>
        public Tbool IsAlways(bool val)
        {
            return IsAlwaysTvar<Tbool>(val, Time.DawnOf, Time.EndOf);
        }
        
        /// <summary>
        /// Returns true if the Tbool always has a specified boolean value
        /// between two given dates. 
        /// </summary>
        public Tbool IsAlways(bool val, DateTime start, DateTime end)
        {
            return IsAlwaysTvar<Tbool>(val, start, end);
        }
        
        /// <summary>
        /// Returns true if the Tbool ever has a specified boolean value. 
        /// </summary>
        public Tbool IsEver(bool val)
        {
            return IsEverTvar(val);
        }
        
        /// <summary>
        /// Returns true if the Tbool ever has a specified boolean value
        /// between two given dates. 
        /// </summary>
        public Tbool IsEver(bool val, DateTime start, DateTime end)
        {
            return IsEverTvar<Tbool>(val, start, end);
        }
        
        /// <summary>
        /// Returns the DateTime when the Tbool is first true.
        /// </summary>
        public DateTime DateFirstTrue 
        {
            get
            {
                return DateFirst<Tbool>(true);
            }
        }
        
        /// <summary>
        /// Returns the DateTime when the Tbool is first false.
        /// </summary>
        public DateTime DateFirstFalse 
        {
            get
            {
                return DateFirst<Tbool>(false);
            }
        }
        
        /// <summary>
        /// Overloaded boolean operator: True.
        /// </summary>
        public static bool operator true (Tbool tb)
        {
            return tb.IsTrue;
        }
        
        /// <summary>
        /// Returns true only if the Tbool is true during the window of concern;
        /// otherwise false. 
        /// </summary>
        public bool IsTrue
        {
            get
            {
                if (Facts.WindowOfConcernIsDefault)
                {
                    return this.IntervalValues.Count == 1 && 
                           Convert.ToBoolean(this.IntervalValues.Values[0]) == true; 
                }
                if (Facts.WindowOfConcernIsPoint)
                {
                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
                }
                
                return Convert.ToBoolean(this.IsAlways(true, 
                                                       Facts.WindowOfConcernStart, 
                                                       Facts.WindowOfConcernEnd
                                                       ).ToBool);
            }
        }
        
        /// <summary>
        /// Overloaded boolean operator: False.
        /// </summary>
        public static bool operator false (Tbool tb)
        {
            return tb.IsFalse;
        }
        
        /// <summary>
        /// Returns true only if the Tbool is false during the window of concern;
        /// otherwise true. 
        /// </summary>
        public bool IsFalse
        {
            get
            {
                if (Facts.WindowOfConcernIsDefault)
                {
                    return this.IntervalValues.Count == 1 && 
                           Convert.ToBoolean(this.IntervalValues.Values[0]) == false; 
                }
                if (Facts.WindowOfConcernIsPoint)
                {
                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
                }
                
                return Convert.ToBoolean(this.IsAlways(false, 
                                                       Facts.WindowOfConcernStart, 
                                                       Facts.WindowOfConcernEnd
                                                       ).ToBool);
            }
        }
        
        /// <summary>
        /// Converts a Tbool to a nullable boolean.
        /// Returns null if the Tbool is unknown, if it's value changes over
        /// time (that is, if it's not eternal), and when it's null.
        /// </summary>
        public bool? ToBool
        {
            get
            {
                if (this.IsUnknown || TimeLine.Count > 1) { return null; }
                
                return (bool?)TimeLine.Values[0];
            }
        }
        
        /// <summary>
        /// Overloaded boolean operator: Equal.
        /// </summary>
        public static Tbool operator == (Tbool tb1, Tbool tb2)
        {
            return EqualTo(tb1,tb2);
        }
        public static Tbool operator == (Tbool tb, bool b)
        {
            return EqualTo(tb,new Tbool(b));
        }
        public static Tbool operator == (bool b, Tbool tb)
        {
            return EqualTo(tb,new Tbool(b));
        }
        
        /// <summary>
        /// Overloaded boolean operator: Not equal.
        /// </summary>
        public static Tbool operator != (Tbool tb1, Tbool tb2)
        {
            return !EqualTo(tb1,tb2);
        }
        public static Tbool operator != (Tbool tb, bool b)
        {
            return !EqualTo(tb,new Tbool(b));
        }
        public static Tbool operator != (bool b, Tbool tb)
        {
            return !EqualTo(tb,new Tbool(b));
        }

    }
    
    #pragma warning restore 660, 661
}