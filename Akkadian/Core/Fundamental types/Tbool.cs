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

        public Tbool(Hstate state)
        {
            this.SetEternally(state);
        }

        public Tbool(Hval v)
        {
            this.SetEternally(v);
        }

        public Tbool(bool b)
        {
            this.SetEternally(b);
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
        public Tbool AsOf(Tdate dt)
        {
            return this.AsOf<Tbool>(dt);
        }

        /// <summary>
        /// Returns a Tbool in which the values are shifted in time relative to
        /// the dates.
        /// </summary>
        public Tbool Shift(int offset, Tnum temporalPeriod)
        {
            return this.Shift<Tbool>(offset, temporalPeriod);
        }

        /// <summary>
        /// Returns a Tbool in which the last value in a time period is the
        /// final value.
        /// </summary>
        public Tbool PeriodEndVal(Tnum temporalPeriod)
        {
            return this.PeriodEndVal<Tbool>(temporalPeriod).Lean;
        }

        /// <summary>
        /// Indicates whether the Tbool is always true.
        /// </summary>
        public Tbool IsAlwaysTrue()
        {
            // If timeline varies, it cannot always be the given value
            if (this.TimeLine.Count > 1) return false;

            // If val is unknown and base Tvar is eternally unknown,
            // return the typical precedence state
            if (!this.FirstValue.IsKnown)
            {
                return new Tbool(this.FirstValue);
            }

            // Else, test for equality
            if (this.FirstValue.IsEqualTo(true)) return true;

            return false;
        }

        /// <summary>
        /// Determines whether this instance is always true during a specified interval.
        /// </summary>
        public Tbool IsAlwaysTrue(Tdate start, Tdate end)
        {
            Tbool equalsVal = this == true; 

            Tbool isDuringInterval = Time.IsBetween(start, end);
            
            Tbool isOverlap = equalsVal & isDuringInterval;

            Tbool overlapAndIntervalAreCoterminous = isOverlap == isDuringInterval;

            return !overlapAndIntervalAreCoterminous.IsEver(false);
        }

        /// <summary>
        /// Determines whether this instance is ever true during a specified time period.
        /// </summary>
        public Tbool IsEverTrue(Tdate start, Tdate end)
        {
            Tbool equalsVal = this == true; 
            
            Tbool isDuringInterval = Time.IsBetween(start, end);
            
            Tbool isOverlap = equalsVal & isDuringInterval;
            
            return isOverlap.IsEverTrue();
        }
        
        /// <summary>
        /// Returns true if the Tbool ever has a specified boolean value. 
        /// </summary>
        public Tbool IsEverTrue()
        {
            return this.IsEver(true);
        }

        /// <summary>
        /// Determines whether the Tvar is ever the specified boolean val.
        /// </summary>
        private Tbool IsEver(Hval val)
        {
            // If val is unknown and base Tvar is eternally unknown,
            // return the typical precedence state
            if (!val.IsKnown && this.TimeLine.Count == 1)
            {
                if (!this.FirstValue.IsKnown)
                {
                    Hstate s = PrecedingState(this.FirstValue, val);
                    return new Tbool(s);
                }
            }

            // If val is unknown, return its state
            if (!val.IsKnown) return new Tbool(val);
           
            // If the base Tvar is ever val, return true
            foreach (Hval h in this.TimeLine.Values)
            {
                if (h.IsEqualTo(val)) return true;
            }

            // If base Tvar has a time period of unknownness, return 
            // the state with the proper precedence
            Hstate returnState = PrecedenceForMissingTimePeriods(this);
            if (returnState != Hstate.Known) return new Tbool(returnState);

            return false;
        }

        /// <summary>
        /// Returns the DateTime when the Tbool is first true.
        /// </summary>
        public Tdate DateFirstTrue
        {
            get
            {
                SortedList<DateTime, Hval> line = this.TimeLine;
                Hval result = new Hval(null,Hstate.Stub);
    
                // If Tval is eternally unknown, return that value
                if (this.IsEternallyUnknown)
                {
                    result = this.FirstValue;
                }
                else
                {
                    // For each time interval...
                    for (int i = 0; i < line.Count; i++) 
                    {
                        // If you encounter an unknown interval, return that value
                        // Warning: Could be problematic b/c initial intervals are likely to be unknown...
                        if (!line.Values[i].IsKnown)
                        {
                            // result = line.Values[i];
                        }
                        // Look for the date when the Tvar has the given value
                        else if (Convert.ToBoolean(line.Values[i].Val))
                        {
                            result = line.Keys[i];
                            return new Tdate(result);
                        }
                        else
                        {
                            // If Tvar never has the given value, return the default value
                        }
                    }
                }
    
                return new Tdate(result);
            }
        }

        /// <summary>
        /// Returns the DateTime when the Tbool is true for the last time
        /// </summary>
        public Tdate DateLastTrue
        {
            get
            {
                SortedList<DateTime, Hval> line = this.TimeLine;
                Hval result = new Hval(null,Hstate.Stub);
    
                // If Tval is eternally unknown, return that value
                if (this.IsEternallyUnknown)
                {
                    result = this.FirstValue;
                }
                else
                {
                    // For each time interval...
                    for (int i = 0; i < line.Count; i++) 
                    {
                        // If you encounter an unknown interval, return that value
                        // Warning: Could be problematic b/c initial intervals are likely to be unknown...
                        if (!line.Values[i].IsKnown)
                        {
                            // result = line.Values[i];
                        }
                        // Look for the date when the Tvar has the given value
                        else if (Convert.ToBoolean(line.Values[i].Val))
                        {
                            if (i >= line.Count-1)
                            {
                                result = Time.EndOf;
                            }
                            else
                            {
                                result = line.Keys[i+1].AddDays(-1);
                            }
                        }
                        else
                        {
                            // If Tvar never has the given value, return the default value
                        }
                    }
                }
    
                return new Tdate(result);
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
        /// Used in symmetrical facts and short-circuit evaluation.
        /// </summary>
        public bool IsTrue
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsTrue;

//                if (Facts.WindowOfConcernIsDefault)
//                {
//                    return this.IntervalValues.Count == 1 && 
//                           Convert.ToBoolean(this.IntervalValues.Values[0].Val) == true; 
//                }
//                if (Facts.WindowOfConcernIsPoint)
//                {
//                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
//                }
//                
//                return Convert.ToBoolean(this.IsAlways(true, 
//                                                       Facts.WindowOfConcernStart, 
//                                                       Facts.WindowOfConcernEnd
//                                                       ).ToBool);
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
                return this.IsEternal && this.FirstValue.IsFalse;

//                if (Facts.WindowOfConcernIsDefault)
//                {
//                    return this.IsEternal && this.FirstValue.IsFalse;
//                }
//                if (Facts.WindowOfConcernIsPoint)
//                {
//                    return Convert.ToBoolean((!this).AsOf(Facts.WindowOfConcernStart));
//                }
//                
//                return Convert.ToBoolean((!this).IsAlwaysTrue( 
//                                                       Facts.WindowOfConcernStart, 
//                                                       Facts.WindowOfConcernEnd
//                                                       ).ToBool);
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
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (bool?)this.FirstValue.Val;
            }
        }

        /// <summary>
        /// Overloaded boolean operator: Equal.
        /// </summary>
        public static Tbool operator == (Tbool tb1, Tbool tb2)
        {
            return EqualTo(tb1,tb2);
        }
        
        /// <summary>
        /// Overloaded boolean operator: Not equal.
        /// </summary>
        public static Tbool operator != (Tbool tb1, Tbool tb2)
        {
            return NotEqualTo(tb1,tb2);
        }

        /// <summary>
        /// Given a Tbool and an index date, returns the date of the next change date of the Tbool.
        /// </summary>
        public DateTime NextChangeDate(DateTime indexDate)
        {
            for (int j=0; j < this.IntervalValues.Count; j++)
            {
                if (indexDate <= this.IntervalValues.Keys[j])
                {
                    return this.IntervalValues.Keys[j];
                }
            }

            return Time.EndOf;
        }

        /// <summary>
        /// Given a Tbool and an index date, returns the date the Tbool is next true.
        /// </summary>
        public DateTime DateNextTrue(DateTime indexDate)
        {
            for (int j=0; j < this.IntervalValues.Count; j++)
            {
                if (indexDate <= this.IntervalValues.Keys[j] && Convert.ToBoolean(this.IntervalValues.Values[j].Val) == true)
                {
                    return this.IntervalValues.Keys[j];
                }
            }

            return Time.EndOf;
        }
    }
    
    #pragma warning restore 660, 661
}