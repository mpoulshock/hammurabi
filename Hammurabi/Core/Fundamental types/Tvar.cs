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
        public static implicit operator Tvar(Thing e) 
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
            TimeLine.Add(dt, hval);
        }
        
        /// <summary>
        /// Removes redundant intervals from the Tvar. 
        /// </summary>
        public T LeanTvar<T>() where T : Tvar
        {
            T result = (T)Util.ReturnProperTvar<T>();
            result.AddState(TimeLine.Keys[0], TimeLine.Values[0]);

            if (TimeLine.Count > 0)
            {
                for (int i=0; i < TimeLine.Count-1; i++ ) 
                {
                    if (!object.Equals(TimeLine.Values[i].Val, TimeLine.Values[i+1].Val))
                    {
                        result.AddState(TimeLine.Keys[i+1], TimeLine.Values[i+1]);
                    }
                }
            }

            return result;
        }     

        /// <summary>
        /// Sets a Tvar to an "eternal" value (the same at all points in time). 
        /// </summary>
        public void SetEternally(Hval hval)
        {
            AddState(Time.DawnOf, hval);
        }

        /// <summary>
        /// Displays an output object.
        /// </summary>
        public object Out
        {
            get 
            {  
                if (this.TimeLine.Count == 1)
                {
                    Hval v = this.FirstValue;
                    if (v.IsKnown)
                    {
                        if (v.IsSet())   return v.ToSerializedSet();

                        return v.Obj;
                    }
                    else
                    {
                        return v.ToString;
                    }
                }
                else
                {
                    string result = "{";
                    
                    foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
                    {
                        string date = de.Key.ToString().Replace("1/1/1800", "Dawn");
                        date = date.Replace(" 12:00:00 AM", "");
                        string val = de.Value.ToString.Replace("True","true").Replace("False","false");
                        result += date + ": " + val + "; ";
                    }
                    return result.TrimEnd(' ', ';') + "}";
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
        /// Indicates whether the Tvar is eternally uncertain.
        /// </summary>
        public bool IsEternallyUncertain
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsUncertain;
            }
        }

        /// <summary>
        /// Indicates whether the Tvar is eternally unstated.
        /// </summary>
        public bool IsEternallyUnstated
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsUnstated;
            }
        }

        /// <summary>
        /// Returns true if the Tvar is ever unstated; otherwise false.
        /// </summary>
        public bool IsEverUnstated
        {
            get
            {
                return this.IsUnstated.IsEverTrue().ToBool == true ? true : false;
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

            return (T)Util.ReturnProperTvar<T>(result);
        }

        /// <summary>
        /// Returns an object value of the Tvar at a specified point in time.
        /// </summary>
        public Hval ObjectAsOf(DateTime dt)
        {
            for (int i = 0; i < TimeLine.Count-1; i++ ) 
            {
                // If value is between two adjacent points on the timeline...
                if (dt >= TimeLine.Keys[i])
                {
                    if (dt < TimeLine.Keys[i+1])
                    {
                        return TimeLine.Values[i];
                    }
                }
            }

            return TimeLine.Values[TimeLine.Count-1];
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
        public static Tbool EqualTo(Tvar tv1, Tvar tv2)
        {
            return ApplyFcnToTimeline<Tbool>(x => Eq(x), tv1, tv2);
        }
        private static Hval Eq(List<Hval> list)
        {
            return object.Equals(list[0].Val, list[1].Val);
        }

        /// <summary>
        /// Returns true when two Tvar values are not equal. 
        /// </summary>
        public static Tbool NotEqualTo(Tvar tv1, Tvar tv2)
        {
            return ApplyFcnToTimeline<Tbool>(x => NotEq(x), tv1, tv2);
        }
        private static Hval NotEq(List<Hval> list)
        {
            return !object.Equals(list[0].Val, list[1].Val);
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

        /// <summary>
        /// Returns a Tvar in which the values are shifted in time relative to
        /// the dates. A negative offset gets values from the past; a positive one
        /// gets them from the future.
        /// </summary>
        /// <remarks>
        /// Used, for example, to get the value of a Tvar during a prior or future
        /// time period.
        /// Note: Time points on both the base Tvar and the temporalPeriod Tnum 
        /// must line up in order for the method to work properly.
        /// </remarks>
        /// <example>
        ///                 N =  <--33--|--44--|--55--|--66--|--77-->
        ///              Year =  <-2010-|-2011-|-2012-|-2013-|-2014->
        ///  N.Shift(-2,Year) =  <---------33---------|--44--|--55--|--66--|--77-->
        /// </example>            
        public T Shift<T>(int offset, Tnum temporalPeriod) where T : Tvar
        {
            // TODO: Make "offset" a Tnum instead of an int

            T result = (T)Util.ReturnProperTvar<T>();
            result.AddState(this.TimeLine.Keys[0], this.TimeLine.Values[0]);

            // No need to handle uncertainty b/c this method just reuses the values in
            // the base Tvar.

            // Iterate through pairs in the base Tvar
            foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
            {
                // Extract parts of the date-value pair
                DateTime origDate = Convert.ToDateTime(de.Key);
                Hval val = de.Value;
                DateTime offsetDate = Time.EndOf;

                // Leave the value at Time.DawnOf alone
                if (origDate != Time.DawnOf)
                {
                    // Get the time point with the appropriate offset from the current time point
                    // First, look up the original date in temporalPeriod
                    for (int i=0; i<temporalPeriod.TimeLine.Values.Count; i++)
                    {
                        DateTime testDate = Convert.ToDateTime(temporalPeriod.TimeLine.Keys[i]);
                        if (testDate == origDate)
                        {
                            // Then get the date offset from the original date
                            int offsetIndex = i + (offset * -1);

                            // Don't overrun the temporalPeriod list
                            if (offsetIndex < temporalPeriod.TimeLine.Count &&
                                offsetIndex >= 0)
                            {
                                offsetDate = temporalPeriod.TimeLine.Keys[offsetIndex];

                                // Prevent overflowing the bounds of Time
                                if (offsetDate < Time.EndOf) 
                                {
                                    result.AddState(offsetDate, val);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Given a base Tvar and a period, returns a Tvar where the value of each period
        /// is the value of the base Tvar at the end of each period.
        /// </summary>
        /// <remarks>
        /// Example: marital status for tax purposes is the status as of the last day
        /// of the tax year.
        /// </remarks>
        /// <example>
        ///              Tvar =  <------1---|-----------2----------->
        ///              Year =  <-2010-|-2011-|-2012-|-2013-|-2014->
        ///    Tvar.PEV(Year) =  <--1---|----------2---------------->
        /// </example>            
        public T PeriodEndVal<T>(Tnum temporalPeriod) where T : Tvar
        {
            T result = (T)Util.ReturnProperTvar<T>();
            result.AddState(this.TimeLine.Keys[0], this.TimeLine.Values[0]);
            
            // No need to handle uncertainty b/c this method just reuses the values in
            // the base Tvar.

            // Iterate backwards through the timeline of the base Tvar
            for(int i=this.TimeLine.Count-1; i>0; i--)
            {
                // If the date is not lined up with a time point in temporalPeriod
                DateTime theDate = Convert.ToDateTime(this.TimeLine.Keys[i]);
                if (temporalPeriod.TimeLine.Keys.Contains(theDate))
                {
                    // If result does not already contain the date, add it to result
                    if (!result.TimeLine.Keys.Contains(theDate))
                    {
                        result.AddState(theDate, this.TimeLine.Values[i]);
                    }
                }
                else
                {
                    // Get the date in temporalPeriod that is immediately prior to theDate
                    // (i.e. the beginning of that period)
                    for (int j=temporalPeriod.TimeLine.Count-1; j>0; j--)
                    {
                        DateTime periodDate = temporalPeriod.TimeLine.Keys[j];
                        if (periodDate < theDate)
                        {
                            // If result does not already contain the new change date, add it to result
                            if (!result.TimeLine.Keys.Contains(periodDate))
                            {
                                result.AddState(periodDate, this.TimeLine.Values[i]);
                            }

                            break;
                        }
                    }
                }
            }
            
            return result;
        }
    }    
}