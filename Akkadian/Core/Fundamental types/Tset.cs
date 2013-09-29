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
using System.Linq;

namespace Akkadian
{
    #pragma warning disable 660, 661
    
    /// <summary>
    /// A set of legal entities whose membership varies over time.
    /// Example: a family that is composed of different members at various
    /// points in time.
    /// </summary>
    public partial class Tset : Tvar
    {
        /// <summary>
        /// Constructs an unknown Tset (one with no states). 
        /// </summary>
        public Tset()
        {
        }

        public Tset(Hstate state)
        {
            this.SetEternally(state);
        }

        public Tset(Hval val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Constructs a Tset consisting eternally of a list of members. 
        /// </summary>
        public Tset(params Thing[] list)
        {
            this.SetEternally(list);
        }
        
        /// <summary>
        /// Constructs a Tset consisting eternally of a list of members. 
        /// </summary>
        public Tset(List<Thing> list)
        {
            this.SetEternally(new Hval(list));
        }
  
        /// <summary>
        /// Constructs a Tset from an existing Tset. 
        /// </summary>
        public Tset(Tset s)
        {
            for (int i=0; i<s.TimeLine.Count; i++)
            {
                this.AddState(s.TimeLine.Keys[i], s.TimeLine.Values[i]);
            }
        }
        
        /// <summary>
        /// Implicitly converts a legal entity into a Tset
        /// </summary>
        public static implicit operator Tset(Thing e) 
        {
            return new Tset(e);
        }
        
        /// <summary>
        /// Adds a time interval and list of set members to the timeline. 
        /// </summary>
        public void AddState(DateTime dt, params Thing[] list)
        {
            List<Thing> entities = new List<Thing>();
            
            foreach (Thing le in list)
                entities.Add(le);

            TimeLine.Add(dt, new Hval(entities));    
        }
        
        /// <summary>
        /// Sets the Tset to eternally have a given member. 
        /// </summary>
        public void SetEternally(Thing val)
        {
            TimeLine.Add(Time.DawnOf, new Hval(val));    
        }
        
        /// <summary>
        /// Sets the Tset to eternally have a list of members. 
        /// </summary>
        public void SetEternally(params Thing[] list)
        {
            AddState(Time.DawnOf,list);
        }
        
        /// <summary>
        /// Eliminates redundant intervals in the Tset. 
        /// </summary>
        public Tset Lean
        {
            get
            {
                Tset n = this;
            
                // Identify redundant intervals
                List<DateTime> dupes = new List<DateTime>();
                
                if (TimeLine.Count > 0)
                {
                    for (int i=0; i < TimeLine.Count-1; i++ ) 
                    {
                        if (AreEquivalentSets((List<Thing>)TimeLine.Values[i+1].Val,(List<Thing>)TimeLine.Values[i].Val))
                        {
                            dupes.Add(TimeLine.Keys[i+1]);
                        }
                    }
                }
                
                // Remove redundant intervals
                foreach (DateTime d in dupes) TimeLine.Remove(d);
                    
                return n;
            }
        }
        
        /// <summary>
        /// Determines whether two lists of legal entities are equivalent (ignoring order)
        /// </summary>
        public static bool AreEquivalentSets(List<Thing> L1, List<Thing> L2)
        {
            if (L1.Count != L2.Count) return false;
            
            foreach(Thing i in L1)
            {
                if (!L2.Contains(i)) return false;
            }
            
            return true;
        }                             
        
        /// <summary>
        /// Converts a Tset containing a single member into a (nullable) 
        /// LegalEntity.  Returns null if the Tset is unknown, if it's 
        /// value changes over time, or if it has more than one member.
        /// </summary>
        public Thing ToThing
        {
            // TODO: Handle exceptions...(e.g. empty and uncertain sets)
            get
            {
                if (this.FirstValue.IsUnstated) { return new Thing(""); }

                List<Thing> list = (List<Thing>)this.TimeLine.Values[0].Obj;
                return (Thing)list[0];
            }
        }

        /// <summary>
        /// Returns the members of the set at a specified point in time. 
        /// </summary>
        public Tset AsOf(Tdate dt)
        {
            return this.AsOf<Tset>(dt);
        }

        /// <summary>
        /// Returns a Tset in which the values are shifted in time relative to
        /// the dates.
        /// </summary>
        public Tset Shift(int offset, Tnum temporalPeriod)
        {
            return this.Shift<Tset>(offset, temporalPeriod);
        }

        /// <summary>
        /// Returns a Tset in which the last value in a time period is the
        /// final value.
        /// </summary>
        public Tset PeriodEndVal(Tnum temporalPeriod)
        {
            return this.PeriodEndVal<Tset>(temporalPeriod).Lean;
        }

        /// <summary>
        /// Counts the number of set members at each time interval. 
        /// </summary>
        public Tnum Count
        {
            get
            {
                return ApplyFcnToTimeline<Tnum>(x => CoreTsetCount(x), this);
            }
        }
        private static Hval CoreTsetCount(Hval h)
        {
            return ((List<Thing>)h.Val).Count;
        }

        /// <summary>
        /// Returns true when the set has no members.
        /// </summary>
        public Tbool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }    
        
        /// <summary>
        /// Returns true when one Tset is a subset of another. 
        /// </summary>
        public Tbool IsSubsetOf(Tset super)
        {
            return ApplyFcnToTimeline<Tbool>(x => CoreSubset(x), this, super);
        }
        private static Hval CoreSubset(List<Hval> list)
        {
            List<Thing> s1 = (List<Thing>)list[0].Val;
            List<Thing> s2 = (List<Thing>)list[1].Val;
            return new Hval(!s1.Except(s2).Any());
        }

        /// <summary>
        /// Returns true when the Tset contains a given legal entity. 
        /// </summary>
        public Tbool Contains(Thing e)
        {
            return ApplyFcnToTimeline<Tbool>(x => CoreSubset(x), new Tset(e), this);
        }

        /// <summary>
        /// Returns the temporal union of two Tsets.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        /// <remarks>
        /// This method provides a second operator denoting
        /// the union of a set.  Though different than standard
        /// programming usage, + reflects the common sense 
        /// notion that you can add two sets together to get the
        /// sum of the parts.
        /// </remarks>            
        public static Tset operator + (Tset set1, Tset set2)    
        {
            return set1 | set2;
        }
        
        /// <summary>
        /// Returns the temporal union of two Tsets.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        public static Tset operator | (Tset set1, Tset set2)
        {
            return ApplyFcnToTimeline<Tset>(x => CoreTsetUnion(x), set1, set2);
        }
        private static Hval CoreTsetUnion(List<Hval> list)
        {
            List<Thing> s1 = (List<Thing>)list[0].Val;
            List<Thing> s2 = (List<Thing>)list[1].Val;
            return new Hval(s1.Union(s2).ToList());
        }

        /// <summary>
        /// Returns the temporal intersection of two Tsets.
        /// This is equivalent to a logical AND of two sets.
        /// </summary>
        public static Tset operator & (Tset set1, Tset set2)
        {
            return ApplyFcnToTimeline<Tset>(x => CoreTsetIntersection(x), set1, set2);
        }
        private static Hval CoreTsetIntersection(List<Hval> list)
        {
            List<Thing> s1 = (List<Thing>)list[0].Val;
            List<Thing> s2 = (List<Thing>)list[1].Val;
            return new Hval(s1.Intersect(s2).ToList());
        }
        
        /// <summary>
        /// Returns the relative complement (set difference) of two Tsets.
        /// This is equivalent to subtracting the members of the second
        /// Tset from those of the first (Tset1 - Tset2).
        /// Example: theAdults = thePeople - theKids.
        /// </summary>
        public static Tset operator - (Tset set1, Tset set2)
        {
            return ApplyFcnToTimeline<Tset>(x => CoreTsetRC(x), set1, set2);
        }
        private static Hval CoreTsetRC(List<Hval> list)
        {
            List<Thing> s1 = (List<Thing>)list[0].Val;
            List<Thing> s2 = (List<Thing>)list[1].Val;
            return new Hval(s1.Except(s2).ToList());
        }

        /// <summary>
        /// Returns true when two sets are equal (have the same members). 
        /// </summary>
        public static Tbool operator == (Tset set1, Tset set2)    
        {
            return set1.IsSubsetOf(set2) && set2.IsSubsetOf(set1);
        }
        
        /// <summary>
        /// Returns true when two sets are not equal (i.e. when they do 
        /// not have the same members). 
        /// </summary>
        public static Tbool operator != (Tset set1, Tset set2)    
        {
            return !(set1 == set2);
        }

        /// <summary>
        /// Reverses the order of the members of a Tset. 
        /// </summary>
        public Tset Reverse
        {
            get
            {
                return ApplyFcnToTimeline<Tset>(x => CoreReverse(x), this);
            }
        }
        private static Hval CoreReverse(Hval h)
        {
            List<Thing> list = (List<Thing>)h.Val;
            list.Reverse();
            return new Hval(list);
        }
    }

    #pragma warning restore 660, 661
}