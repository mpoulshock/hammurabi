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
    /// <summary>
    /// The (abstract) base class of all temporal variables (Tvars).
    /// Ordinarily, the functions in this class should only be called by core
    /// functions, not law-related ones.
    /// </summary>
    public abstract partial class Tvar : H 
    {
        /// <summary>
        /// The core Tvar data structure: a timeline of dates and associated values.
        /// </summary>
        protected SortedList<DateTime, Hval> TimeLine = new SortedList<DateTime, Hval>();
        
        /// <summary>
        /// The accessor for TimeLine.
        /// </summary>
        public SortedList<DateTime, Hval> IntervalValues
        {
            get
            {
                return TimeLine;
            }
        }
        
        /// <summary>
        /// Implicitly converts ints to Tvars.
        /// </summary>
        public static implicit operator Tvar(bool b) 
        {
            return new Tbool(b);
        }
        
        /// <summary>
        /// Implicitly converts ints to Tnums.
        /// </summary>
        public static implicit operator Tvar(int i) 
        {
            return new Tnum(i);
        }
        
        /// <summary>
        /// Implicitly converts decimals to Tvars.
        /// </summary>
        public static implicit operator Tvar(decimal d) 
        {
            return new Tnum(d);
        }
        
        /// <summary>
        /// Implicitly converts doubles to Tvars.
        /// </summary>
        public static implicit operator Tvar(double d) 
        {
           return new Tnum(d);
        }
        
        /// <summary>
        /// Implicitly converts strings to Tvars.
        /// </summary>
        public static implicit operator Tvar(string s) 
        {
            return new Tstr(s);
        }
        
        /// <summary>
        /// Implicitly converts DateTimes to Tvars.
        /// </summary>
        public static implicit operator Tvar(DateTime d) 
        {
            return new Tdate(d);
        }
        
        /// <summary>
        /// Implicitly converts a legal entity into a Tvar
        /// </summary>
        public static implicit operator Tvar(LegalEntity e) 
        {
            return new Tset(e);
        }
        
        /// <summary>
        /// Indicates whether a (non-unknown) value has been determined for the Tvar 
        /// </summary>
        public bool IsKnown
        {
            get
            {
                return false;  // TODO: Deal with this (relates to facts.either)
            }
        }

        /// <summary>
        /// Adds an time-value state to the TimeLine. 
        /// </summary>
        public void AddState(DateTime dt, Hval hval)
        {
            // If a state (DateTime) is added to a Tvar that already has that 
            // state, an error occurs.  We could check for duplicates here, but
            // this function is used extremely frequently and doing so would
            // probably affect performance.
            TimeLine.Add(dt, hval);
        }

        /// <summary>
        /// Sets a Tvar to an "eternal" value (the same at all points in time). 
        /// </summary>
        public void SetEternally(Hval hval)
        {
            AddState(Time.DawnOf, hval);
        }

        /// <summary>
        /// Displays a timeline showing the state of the object at various
        /// points in time.
        /// </summary>
        public string Timeline
        {
            get 
            {  
                string result = "";
                foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
                {
                    // Show the value as an element on the timeline
                    result += de.Key + " " + de.Value.ToString + "\n"; 
                }
                return result;
            }
        }

        /// <summary>
        /// Displays a timeline indicating the state of the object at various
        /// points in time.  Same as .Timeline but without line breaks.
        /// Used for test cases only.
        /// </summary>
        public string TestOutput
        {
            get
            {
                return Timeline.Replace("\n"," ");
            }
        }
        
        /// <summary>
        /// Displays an output object.
        /// </summary>
        //  TODO: Handle temporal outputs.
        public object Out
        {
            get 
            {  
                if (this.TimeLine.Count == 1)
                {
                    Hval v = this.FirstValue;
                    if (v.IsKnown)
                    {
                        return v.Obj;
                    }
                    else
                    {
                        return v.ToString;
                    }
                }
                else
                {
                    return this.TestOutput;
                }
              }
        }

        /// <summary>
        /// Returns the value of the Tvar at the first time interval.
        /// </summary>
        public Hval FirstValue
        {
            get
            {
                return this.TimeLine.Values[0];
            }
        }

        /// <summary>
        /// Indicates whether the Tvar has the same value at all time intervals.
        /// </summary>
        public bool IsEternal
        {
            get
            {
                return this.TimeLine.Count == 1;
            }
        }

        /// <summary>
        /// Indicates whether the Tvar is eternally unknown.
        /// </summary>
        public bool IsEternallyUnknown
        {
            get
            {
                return this.IsEternal &&
                        (this.FirstValue.IsStub ||
                         this.FirstValue.IsUncertain ||
                         this.FirstValue.IsUnstated);
            }
        }

        /// <summary>
        /// Returns the value of a Tvar at a specified point in time. 
        /// </summary>
        /// <remarks>
        /// If the Tdate varies over time, only the first value is used.
        /// </remarks>
        public T AsOf<T>(Tdate date) where T : Tvar
        {
            Hval result;

            // If base Tvar has eternal, known value, return that.
            // (In these cases, the Tdate is irrelevant.)
            if (this.IsEternal && !this.IsEternallyUnknown)
            {
                result = this.FirstValue;
            }
            // If either the base Tvar or Tdate are unknown...
            else if (!date.FirstValue.IsKnown || this.IsEternallyUnknown) 
            {
                Hstate top = PrecedingState(this.FirstValue, date.FirstValue);
                result = new Hval(null,top);
            }
            else
            {
                result = this.ObjectAsOf(date.ToDateTime);
            }

            return (T)Auxiliary.ReturnProperTvar<T>(result);
        }

        /// <summary>
        /// Returns an object value of the Tvar at a specified point in time.
        /// </summary>
        public Hval ObjectAsOf(DateTime dt)
        {
            Hval result = TimeLine.Values[TimeLine.Count-1];
            
            for (int i = 0; i < TimeLine.Count-1; i++ ) 
            {
                // If value is between two adjacent points on the timeline...
                if (dt >= TimeLine.Keys[i])
                {
                    if (dt < TimeLine.Keys[i+1])
                    {
                        result = TimeLine.Values[i];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Removes redundant intervals from the Tvar. 
        /// </summary>
        public T LeanTvar<T>() where T : Tvar
        {
            T n = (T)this;
            
            // Identify redundant intervals
            List<DateTime> dupes = new List<DateTime>();
            
            if (TimeLine.Count > 0)
            {
                for (int i=0; i < TimeLine.Count-1; i++ ) 
                {
                    if (object.Equals(TimeLine.Values[i+1].Val, TimeLine.Values[i].Val))
                    {
                        dupes.Add(TimeLine.Keys[i+1]);
                    }
                }
            }
            
            // Remove redundant intervals
            foreach (DateTime d in dupes)
            {
                TimeLine.Remove(d);    
            }
                
            return n;
        }    

        /// <summary>
        /// Gets all points in time at which value of the Tvar changes.
        /// </summary>
        public IList<DateTime> TimePoints()
        {
            return (IList<DateTime>)TimeLine.Keys;
        }

        /// <summary>
        /// Returns true when two Tvar values are equal. 
        /// </summary>
        public static Tbool EqualTo(Tvar tb1, Tvar tb2)
        {
            Tbool result = new Tbool();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tb1,tb2))
            {    
                // Deal with unknowns
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    result.AddState(slice.Key, object.Equals(slice.Value[0].Val, slice.Value[1].Val));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Returns true whenever the Tvar has a value of "unstated."
        /// </summary>
        //  TODO: Why does this exist?
        public Tbool IsUnstated
        {
            get
            {
                Tbool result = new Tbool();

                for (int i = 0; i < TimeLine.Count; i++ ) 
                {
                    if (TimeLine.Values[i].IsUnstated)
                    {
                        result.AddState(TimeLine.Keys[i],true);
                    }
                    else
                    {
                        result.AddState(TimeLine.Keys[i],false);
                    }
                }
    
                return result.Lean;
            }
        }

        /// <summary>
        /// Indicates whether the value of a Tvar has been determined.
        /// </summary>
        public bool HasBeenDetermined
        {
            get
            {
                bool? everUnstated = this.IsUnstated.IsEverTrue().ToBool;

                return Convert.ToBoolean(!everUnstated);
            }
        }

        /// <summary>
        /// Hstate precedences for missing time periods.
        /// </summary>
        public static Hstate PrecedenceForMissingTimePeriods(Tvar t)
        {
            // If the base Tvar is ever unstated, return Unstated
            // b/c user could provide answer that resolves the question
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsUnstated) return Hstate.Unstated;
            }

            // If the base Tvar is ever uncertain, return uncertain
            // b/c if user changes answer, this function might 
            // resolve the question
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsUncertain) return Hstate.Uncertain;
            }

            // If the base Tvar is ever uncertain, return uncertain
            // b/c if the rule logic were complete it might resolve
            // the question.
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsStub) return Hstate.Stub;
            }

            return Hstate.Known;
        }
    }    
}
