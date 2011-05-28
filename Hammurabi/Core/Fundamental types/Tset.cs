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
    /// A set of legal entities whose membership varies over time.
    /// Example: a family that is composed of different members at various
    /// points in time.
    /// </summary>
    public class Tset : Tvar
    {
        /// <summary>
        /// Constructs an unknown Tset (one with no states). 
        /// </summary>
        public Tset()
        {
        }
        
        /// <summary>
        /// Constructs a Tset consisting eternally of a list of members. 
        /// </summary>
        public Tset(params LegalEntity[] list)
        {
            this.SetEternally(list);
        }
        
        /// <summary>
        /// Constructs a Tset consisting eternally of a list of members. 
        /// </summary>
        public Tset(List<LegalEntity> list)
        {
            this.SetEternally(list);
        }
        
        /// <summary>
        /// Adds a time interval and list of set members to the timeline. 
        /// </summary>
        public void AddState (DateTime dt, params LegalEntity[] list)
        {
            List<LegalEntity> entities = new List<LegalEntity>();
            
            foreach (LegalEntity le in list)
                entities.Add(le);
            
            TimeLine.Add(dt,entities);    
        }
        
        /// <summary>
        /// Sets the Tset to eternally have a given member. 
        /// </summary>
        public void SetEternally(LegalEntity val)
        {
            TimeLine.Add(Time.DawnOf,val);    
        }
        
        /// <summary>
        /// Sets the Tset to eternally have a list of members. 
        /// </summary>
        public void SetEternally(params LegalEntity[] list)
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
                return this.LeanTvar<Tset>();
            }
        }
        
        /// <summary>
        /// Converts a Tset containing a single member into a (nullable) 
        /// LegalEntity.  Returns null if the Tset is unknown, if it's 
        /// value changes over time, or if it has more than one member.
        /// </summary>
        public Person ToPerson  // ToLegalEntity?
        {
            get
            {
                if (this.IsUnknown || TimeLine.Count > 1) { return null; }

                return (Person)TimeLine.Values[0];
            }
        }
        
        /// <summary>
        /// Returns the members of the set at a specified point in time. 
        /// </summary>
        public Tset AsOf(DateTime dt)
        {
            if (this.IsUnknown) { return new Tset(); }
            
            return (Tset)this.AsOf<Tset>(dt);
        }

        /// <summary>
        /// Displays a timeline indicating the members of the set at various
        /// points in time.
        /// </summary>
        new public string Timeline
        {
            get
            {
                string result = "";
                
                if (IsUnknown)
                {
                    result = "Unknown";
                }
                else
                {
                    foreach(KeyValuePair<DateTime,object> de in TimeLine )
                    {
                        result += de.Key + " ";
                        
                        foreach(LegalEntity le in (List<LegalEntity>)de.Value)
                        {
                            result += le.Id + ", ";
                        }
                        result = result.TrimEnd(',',' ');
                        
                        result += "\n";    
                    }
                }
                return result;
            }
        }
        
        /// <summary>
        /// Displays a timeline indicating the members of the set at various
        /// points in time.  Same as .Timeline but without line breaks.
        /// Used for test cases only.
        /// </summary>
        new public string TestOutput
        {
            get
            {
                return Timeline.Replace("\n"," ");
            }
        }

        /// <summary>
        /// Counts the number of set members at each time interval. 
        /// </summary>
        public Tnum Count
        {
            get
            {
                if (this.IsUnknown) { return new Tnum(); }
                
                Tnum result = new Tnum();
                
                foreach(KeyValuePair<DateTime,object> de in TimeLine )
                {
                    List<LegalEntity> entities = (List<LegalEntity>)de.Value;
                    
                    result.AddState(de.Key, Convert.ToString(entities.Count));
                }
                
                return result;
            }
        }

        /// <summary>
        /// Returns true when the set has no members.
        /// </summary>
        public Tbool IsEmpty
        {
            get
            {
                if (this.IsUnknown) { return new Tbool(); }
                
                Tbool result = new Tbool();
                
                foreach(KeyValuePair<DateTime,object> de in TimeLine )
                {
                    bool intervalIsEmpty = true;
                    
                    List<LegalEntity> entities = (List<LegalEntity>)de.Value;
                    
                    if (entities.Count > 0)
                    {
                        intervalIsEmpty = false;
                    }
                    
                    result.AddState(de.Key, intervalIsEmpty);
                }
                
                return result;
            }
        }    
        
        /// <summary>
        /// Returns true when one Tset is a subset of another. 
        /// </summary>
        public Tbool IsSubsetOf(Tset set2)
        {
            return IsSubsetOf(this,set2);
        }
        
        private static Tbool IsSubsetOf(Tset sub, Tset super)
        {
            if (AnyAreUnknown(sub, super)) { return new Tbool(); }
            
            Tbool result = new Tbool();
            
            List<LegalEntity> entitiesInSub = new List<LegalEntity>();
            List<LegalEntity> entitiesInSuper = new List<LegalEntity>();
                        
            foreach (DateTime d in TimePoints(sub, super))
            {
                bool isSubset = true;
                
                entitiesInSub   = sub.EntitiesAsOf(d);
                entitiesInSuper = super.EntitiesAsOf(d);
                    
                foreach (LegalEntity le in entitiesInSub)
                {
                    if (!entitiesInSuper.Contains(le))
                    {
                        isSubset = false;    
                    }
                }
                
                result.AddState(d,isSubset);
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when the Tset contains a given legal entity. 
        /// </summary>
        public Tbool Contains(LegalEntity e)
        {
            return Auxiliary.IsMemberOfSet(e,this);
        }
        
        /// <summary>
        /// Returns the temporal union of two Tsets.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        public static Tset operator | (Tset ts1, Tset ts2)    
        {
            return Union(ts1,ts2);
        }
        public static Tset operator | (Tset ts1, LegalEntity le)    
        {
            return Union(ts1, new Tset(le));
        }
        
        private static Tset Union(params Tset[] sets)
        {
            if (AnyAreUnknown(sets)) { return new Tset(); }
            
            Tset result = new Tset();
            
            foreach(KeyValuePair<DateTime,List<object>> slice in TimePointValues(sets))
            {    
                List<LegalEntity> intervalUnion = new List<LegalEntity>();
                
                foreach (List<LegalEntity> entities in slice.Value)
                {
                    foreach (LegalEntity le in entities)
                    {
                        if (!intervalUnion.Contains(le))
                        {
                            intervalUnion.Add(le);
                        }
                    }
                    
                }
                
                result.AddState(slice.Key, intervalUnion);
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns the temporal intersection of two Tsets.
        /// This is equivalent to a logical AND of two sets.
        /// </summary>
        public static Tset operator & (Tset ts1, Tset ts2)    
        {
            return Intersection(ts1,ts2);
        }
        public static Tset operator & (Tset ts1, LegalEntity le)    
        {
            return Intersection(ts1, new Tset(le));
        }
        
        private static Tset Intersection(Tset set1, Tset set2)
        {
            if (AnyAreUnknown(set1, set2)) { return new Tset(); }
            
            Tset result = new Tset();
                        
            foreach (DateTime d in TimePoints(set1, set2))
            {
                List<LegalEntity> entitiesInSet1 = set1.EntitiesAsOf(d);
                List<LegalEntity> entitiesInSet2 = set2.EntitiesAsOf(d);            
                List<LegalEntity> intersect = new List<LegalEntity>();
                
                // Members of set 1 in set 2
                foreach (LegalEntity le in entitiesInSet1)
                {
                    if (entitiesInSet2.Contains(le))
                    {
                        intersect.Add(le);
                    }
                }
                
                result.AddState(d,intersect);
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Returns the relative complement of two Tsets.
        /// This is equivalent to subtracting the members of the second
        /// Tset from those of the first (Tset1 - Tset2).
        /// Example: theAdults = thePeople - theKids.
        /// </summary>
        public static Tset operator - (Tset ts1, Tset ts2)    
        {
            return RelativeComplement(ts1,ts2);
        }
        public static Tset operator - (Tset ts1, LegalEntity le)    
        {
            return RelativeComplement(ts1, new Tset(le));
        }
        
        private static Tset RelativeComplement(Tset set1, Tset set2)
        {
            if (AnyAreUnknown(set1, set2)) { return new Tset(); }
            
            Tset result = new Tset();
                        
            foreach (DateTime d in TimePoints(set1, set2))
            {
                List<LegalEntity> entitiesInSet1 = set1.EntitiesAsOf(d);
                List<LegalEntity> entitiesInSet2 = set2.EntitiesAsOf(d);            
                List<LegalEntity> complement = new List<LegalEntity>();
                
                // Members of set 1 not in set 2
                foreach (LegalEntity le in entitiesInSet1)
                {
                    if (!entitiesInSet2.Contains(le))
                    {
                        complement.Add(le);
                    }
                }
                
                result.AddState(d,complement);
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when two sets are equal (have the same members). 
        /// </summary>
        public static Tbool operator == (Tset ts1, Tset ts2)    
        {
            return Equals(ts1,ts2);
        }
        public static Tbool operator == (Tset ts1, LegalEntity le)    
        {
            return Equals(ts1, new Tset(le));
        }
        
        private static Tbool Equals(Tset set1, Tset set2)
        {
            return set1.IsSubsetOf(set2) & set2.IsSubsetOf(set1);
        }
        
        /// <summary>
        /// Returns true when two sets are not equal (i.e. when they do 
        /// not have the same members). 
        /// </summary>
        public static Tbool operator != (Tset ts1, Tset ts2)    
        {
            return !Equals(ts1,ts2);
        }
        public static Tbool operator != (Tset ts1, LegalEntity le)    
        {
            return !Equals(ts1, new Tset(le));
        }
        
        // ********************************************************************
        // Should not be used in legal functions
        // ********************************************************************
        
        /// <summary>
        /// Returns a list of the members of the set at a given point in time. 
        /// </summary>
        public List<LegalEntity> EntitiesAsOf(DateTime dt)
        {            
            for (int i = 0; i < TimeLine.Count-1; i++ ) 
            {
                // If value is between two adjacent points on the timeline...
                if (dt >= TimeLine.Keys[i])
                {
                    if (dt < TimeLine.Keys[i+1])
                    {
                        return (List<LegalEntity>)TimeLine.Values[i];
                    }
                }
            }
            
            // If value is on or after last point on timeline...
            if (dt >= TimeLine.Keys[TimeLine.Count-1])
            {
                return (List<LegalEntity>)TimeLine.Values[TimeLine.Count-1];
            }
            
            List<LegalEntity> defaultResult = new List<LegalEntity>();
            return defaultResult;
        }
        
        /// <summary>
        /// Returns a list of all legal entities that were ever members of the 
        /// set. 
        /// </summary>
        public List<LegalEntity> DistinctEntities()
        {
            List<LegalEntity> result = new List<LegalEntity>();
            
            foreach(KeyValuePair<DateTime,object> de in TimeLine )
            {            
                foreach(LegalEntity le in (List<LegalEntity>)de.Value)
                {
                    if (!result.Contains(le))
                    {
                        result.Add(le);    
                    }
                }
            }
            
            return result;
        }

    }
    
    #pragma warning restore 660, 661
}