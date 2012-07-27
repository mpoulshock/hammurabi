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
        public void AddState (DateTime dt, params Thing[] list)
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
        public Person ToPerson  // ToThing
        {
            get
            {
                if (this.TimeLine.Values[0].IsUnstated) { return new Person(""); }

                if (TimeLine.Count != 1) { return new Person(""); }

                List<Thing> list = (List<Thing>)this.TimeLine.Values[0].Obj;

                if (list == null || list[0] == null) { return new Person(""); }

                return (Person)list[0];
            }
        }

        public Thing ToThing  // ToThing
        {
            get
            {
                if (this.TimeLine.Values[0].IsUnstated) { return new Thing(""); }

                if (TimeLine.Count != 1) { return new Thing(""); }

                List<Thing> list = (List<Thing>)this.TimeLine.Values[0].Obj;

                if (list == null || list[0] == null) { return new Thing(""); }

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
        /// Displays a timeline indicating the members of the set at various
        /// points in time.
        /// </summary>
        new public string Timeline
        {
            get 
            {  
                string result = "";
                foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
                {
                    // Show the interval start time
                    if (Convert.ToDateTime(de.Key) == Time.DawnOf)
                    {
                        result += "Time.DawnOf ";
                    }
                    else
                    {
                        result += de.Key + " ";  
                    }

                    // Show the value
                    if (!de.Value.IsKnown) result += de.Value.ToString;

                    else
                    {
                        foreach(Thing le in (List<Thing>)de.Value.Val)
                        {
                            result += le.Id + ", ";
                        }
                        result = result.TrimEnd(',',' ');
                    }
                    result += "\n"; 
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
                Tnum result = new Tnum();
                
                foreach(KeyValuePair<DateTime,Hval> de in TimeLine )
                {
                    if (!de.Value.IsKnown)
                    {
                        result.AddState(de.Key,de.Value);
                    }
                    else
                    {
                        List<Thing> entities = (List<Thing>)de.Value.Val;
                        result.AddState(de.Key, new Hval(Convert.ToDecimal(entities.Count)));
                    }
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
                return this.Count == 0;
            }
        }    
        
        /// <summary>
        /// Returns true when one Tset is a subset of another. 
        /// </summary>
        public Tbool IsSubsetOf(Tset super)
        {
            Tset sub = this;
            Tbool result = new Tbool();
            
            List<Thing> entitiesInSub = new List<Thing>();
            List<Thing> entitiesInSuper = new List<Thing>();
                        
            foreach (DateTime d in TimePoints(sub, super))
            {
                Hval subHval = sub.EntitiesAsOf(d);
                Hval superHval = super.EntitiesAsOf(d);

                // Handle unknowns
                Hstate top = PrecedingState(subHval.State, superHval.State);
                if (top != Hstate.Known) 
                {
                    result.AddState(d, new Hval(null,top));
                }

                // Determine subset
                else
                {
                    bool isSubset = true;
                    entitiesInSub   = (List<Thing>)subHval.Val;
                    entitiesInSuper = (List<Thing>)superHval.Val;
                        
                    foreach (Thing le in entitiesInSub)
                    {
                        if (!entitiesInSuper.Contains(le))
                        {
                            isSubset = false;    
                        }
                    }
                    
                    result.AddState(d,isSubset);
                }
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Returns true when the Tset contains a given legal entity. 
        /// </summary>
        public Tbool Contains(Thing e)
        {
            return Auxiliary.IsMemberOfSet(e,this);
        }

        
        /// <summary>
        /// Returns the temporal union of two Tsets.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        public static Tset operator | (Tset set1, Tset set2)    
        {
            Tset result = new Tset();

            // For each time period
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(set1, set2))
            {   
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                     result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    List<Thing> intervalUnion = new List<Thing>();
    
                    // For each list of entities
                    for (int i=0; i<slice.Value.Count; i++)
                    {
                        List<Thing> entities = (List<Thing>)slice.Value[i].Val;
    
                        foreach (Thing le in entities)
                        {
                            if (!intervalUnion.Contains(le))
                            {
                                intervalUnion.Add(le);
                            }
                        }
                    }
                    
                    result.AddState(slice.Key, new Hval(intervalUnion));
                }
            }
            
            return result.Lean;
        }


        /// <summary>
        /// Returns the temporal intersection of two Tsets.
        /// This is equivalent to a logical AND of two sets.
        /// </summary>
        public static Tset operator & (Tset set1, Tset set2)    
        {
            Tset result = new Tset();
                        
            foreach (DateTime d in TimePoints(set1, set2))
            {
                Hval set1Val = set1.EntitiesAsOf(d);
                Hval set2Val = set2.EntitiesAsOf(d); 

                Hstate top = PrecedingState(set1Val, set2Val);
                if (top != Hstate.Known) 
                {
                     result.AddState(d, new Hval(null,top));
                }
                else
                {
                    List<Thing> entitiesInSet2 = (List<Thing>)set2Val.Val;            
                    List<Thing> intersect = new List<Thing>();
                    
                    // Members of set 1 in set 2
                    foreach (Thing le in (List<Thing>)set1Val.Val)
                    {
                        if (entitiesInSet2.Contains(le))
                        {
                            intersect.Add(le);
                        }
                    }
                    
                    result.AddState(d,new Hval(intersect));
                }
            }

            return result.Lean;
        }
        
        /// <summary>
        /// Returns the relative complement of two Tsets.
        /// This is equivalent to subtracting the members of the second
        /// Tset from those of the first (Tset1 - Tset2).
        /// Example: theAdults = thePeople - theKids.
        /// </summary>
        public static Tset operator - (Tset set1, Tset set2)    
        {
            Tset result = new Tset();
                        
            foreach (DateTime d in TimePoints(set1, set2))
            {
                Hval set1Val = set1.EntitiesAsOf(d);
                Hval set2Val = set2.EntitiesAsOf(d); 

                Hstate top = PrecedingState(set1Val, set2Val);
                if (top != Hstate.Known) 
                {
                     result.AddState(d, new Hval(null,top));
                }
                else
                {
                    List<Thing> entitiesInSet2 = (List<Thing>)set2Val.Val;            
                    List<Thing> complement = new List<Thing>();
                    
                    // Members of set 1 not in set 2
                    foreach (Thing le in (List<Thing>)set1Val.Val)
                    {
                        if (!entitiesInSet2.Contains(le))
                        {
                            complement.Add(le);
                        }
                    }
                    
                    result.AddState(d,new Hval(complement));
                }
            }

            return result.Lean;
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
            return !set1.IsSubsetOf(set2) || !set2.IsSubsetOf(set1);
        }

        
        // ********************************************************************
        // Should not be used in legal functions
        // ********************************************************************
        
        /// <summary>
        /// Returns a list of the members of the set at a given point in time. 
        /// </summary>
        public Hval EntitiesAsOf(DateTime dt)
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
            
            // If value is on or after last point on timeline...
            if (dt >= TimeLine.Keys[TimeLine.Count-1])
            {
                return TimeLine.Values[TimeLine.Count-1];
            }
            
            return new Hval(new List<Thing>());
        }

        /// <summary>
        /// Returns a list of all legal entities that were ever members of the 
        /// set. 
        /// </summary>
        public List<Thing> DistinctEntities()
        {
            List<Thing> result = new List<Thing>();
            
            foreach(KeyValuePair<DateTime,Hval> de in TimeLine )
            {
                if (de.Value.IsKnown)
                {
                    foreach(Thing le in (List<Thing>)de.Value.Val)
                    {
                        if (!result.Contains(le))
                        {
                            result.Add(le);    
                        }
                    }
                }
            }
            
            return result;
        }
    }
    
    #pragma warning restore 660, 661
}