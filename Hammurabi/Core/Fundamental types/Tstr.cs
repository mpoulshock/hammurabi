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
    /// A string object whose value changes at various  points in time. 
    /// </summary>
    public partial class Tstr : Tvar
    {
        
        /// <summary>
        /// Constructs an unknown Tstr. 
        /// </summary>
        public Tstr()
        {
        }
        
        /// <summary>
        /// Constructs a Tstr that is eternally set to a given value. 
        /// </summary>
        public Tstr(string val)
        {
            this.SetEternally(val);
        }

        public Tstr(Hval val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Initializes a new instance of the Tstr class and loads
        /// a list of date-value pairs.
        /// </summary>
        public static Tstr MakeTstr(params object[] list)
        {
            Tstr result = new Tstr();
            for(int i=0; i < list.Length - 1; i+=2)  
            {
                try
                {
                    Hstate h = (Hstate)list[i+1];
                    if (h != Hstate.Known)
                    {
                        result.AddState(Convert.ToDateTime(list[i]),
                                    new Hval(null,h));
                    }
                }
                catch
                {
                    string s = Convert.ToString(list[i+1]);
                    result.AddState(Convert.ToDateTime(list[i]),
                                new Hval(s));
                }
            }
            return result;
        }
        
        /// <summary>
        /// Implicitly converts strings to Tstrs.
        /// </summary>
        public static implicit operator Tstr(string s) 
        {
            return new Tstr(s);
        }
        
        /// <summary>
        /// Removes redundant intervals from a Tstr. 
        /// </summary>
        public Tstr Lean
        {
            get
            {
                return this.LeanTvar<Tstr>();
            }
        }
        
        /// <summary>
        /// Converts a Tstr to a string.
        /// Returns null if the Tstr is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        new public string ToString
        {
            get
            {
                if (TimeLine.Count > 1) return null;

                if (!this.FirstValue.IsKnown) return null;
                
                return (Convert.ToString(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Returns the value of a Tstr at a specified point in time. 
        /// </summary>
        public Tstr AsOf(Tdate dt)
        {
            return this.AsOf<Tstr>(dt);
        }

        /// <summary>
        /// Returns true when two Tstrs are equal. 
        /// </summary>
        public static Tbool operator == (Tstr ts1, Tstr ts2)
        {
            return EqualTo(ts1,ts2);
        }
        
        /// <summary>
        /// Returns true when two Tstrs are not equal. 
        /// </summary>
        public static Tbool operator != (Tstr ts1, Tstr ts2)
        {
            return !EqualTo(ts1,ts2);
        }

        /// <summary>
        /// Concatenates two or more Tstrs. 
        /// </summary>
        public static Tstr operator + (Tstr ts1, Tstr ts2)    
        {
            Tstr result = new Tstr();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(ts1, ts2))
            {    
                // Check for unknowns
                Hstate top = PrecedingState(slice.Value);
                if (top != Hstate.Known) 
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else
                {
                    // Concatenate
                    string str = "";
                    foreach (Hval v in slice.Value) 
                    {
                        str += Convert.ToString(v.Val); 
                    }
                    result.AddState(slice.Key, new Hval(str));
                }
            }
            
            return result.Lean; 
        }
    }
    
    #pragma warning restore 660, 661
}
