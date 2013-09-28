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
    /// States that a Tvar can assume (in addition to its native value).
    /// </summary>
    public enum Hstate
    {
        Stub = 1,       // Value depends upon rule logic that has not yet been completed.
        Uncertain = 2,  // Value depends upon a fact not known by the user.
        Unstated = 3,   // Value depends upon a fact not yet queried or asked of the user.
        Known = 4,      // Value is a valid bool, decimal, string, DateTime, etc.
        Null = 5        // Indicates no value whatsoever (used in Switch statement to idenfity undefined intervals)
    }


    /// <summary>
    /// Value of a Tvar at a given point in time.
    /// </summary>
    public class Hval 
    {
        public object Obj    { get; set; }  // Bool, decimal, string, DateTime, etc. value of the object.
        public Hstate State   { get; set; } // State of knowledge of the object.
        
        /// <summary>
        /// Empty or unstated values.
        /// </summary>
        public Hval()
        {
            Obj = null;
            State = Hstate.Unstated;
        }

        /// <summary>
        /// Known object values and null values.
        /// </summary>
        public Hval(object val)
        {
            if (val == null)
            {
                Obj = null;
                State = Hstate.Uncertain;
            }
            else
            {
                Obj = val;
                State = Hstate.Known;
            }
        }
        
        /// <summary>
        /// States.
        /// </summary>
        public Hval(object ignored, Hstate state)
        {
            if (state == Hstate.Known)
            {
                // Should not happen?
                Obj = null;
                State = Hstate.Known;
            }
            else
            {
                Obj = null;
                State = state;
            }
        }

        /// <summary>
        /// Implicit conversions...
        /// </summary>
        public static implicit operator Hval(bool b) 
        {
            return new Hval(b);
        }
        public static implicit operator Hval(string s) 
        {
            return new Hval(s);
        }
        public static implicit operator Hval(DateTime d) 
        {
            return new Hval(d);
        }
        public static implicit operator Hval(int n) 
        {
            return new Hval(n);
        }
        public static implicit operator Hval(decimal n) 
        {
            return new Hval(n);
        }
        public static implicit operator Hval(double n) 
        {
            return new Hval(n);
        }
        public static implicit operator Hval(Hstate s)  // Does not seem to work properly
        {
            return new Hval(null,s);
        }

        /// <summary>
        /// Returns the value of the Hobj.
        /// </summary>
        /// <value>
        public object Val
        {
            get
            {
                if (this.State == Hstate.Known)
                {
                    return this.Obj;
                }
                else
                {
                    return this.State;
                }
            }
        }

        /// <summary>
        /// Displays the value of the Hval as a string.
        /// </summary>
        /// <value>
        new public string ToString
        {
            get
            {
                if (this.IsStub) return "Stub";
                else if (this.IsUncertain) return "Uncertain";   
                else if (this.IsUnstated) return "Unstated"; 
                else if (this.State == Hstate.Null) return "Null";
                else if (this.IsSet()) return this.ToSerializedSet();
                else return Convert.ToString(this.Obj);
            }
        }

        /// <summary>
        /// Serializes a set of Things.
        /// </summary>
        public string ToSerializedSet()
        {
            string result = "";
            foreach(Thing t in (List<Thing>)this.Obj)
            {
                result += t.Id + ", ";
            }
            return result.TrimEnd(',',' ');
        }

        /// <summary>
        /// Determines whether this instance is a set of Things.
        /// </summary>
        public bool IsSet()
        {
            return  this.Obj.GetType() == (new List<Thing>()).GetType();
        }

        /// <summary>
        /// Indicates whether the value is a boolean "true."
        /// </summary>
        public bool IsTrue
        {
            get
            {
                if (this.State == Hstate.Known)
                {
                    if (Convert.ToBoolean(this.Obj)) return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates whether the value is a boolean "false."
        /// </summary>
        public bool IsFalse
        {
            get
            {
                if (this.State == Hstate.Known)
                {
                    if (Convert.ToBoolean(this.Obj) == false) return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates whether the value is known.
        /// </summary>
        public bool IsKnown
        {
            get
            {
                return this.State == Hstate.Known;
            }
        }

        /// <summary>
        /// Indicates whether the value is a stub.
        /// </summary>
        public bool IsStub
        {
            get
            {
                return this.State == Hstate.Stub;
            }
        }

        /// <summary>
        /// Indicates whether the value is uncertain.
        /// </summary>
        public bool IsUncertain
        {
            get
            {
                return this.State == Hstate.Uncertain;
            }
        }

        /// <summary>
        /// Indicates whether the value is unstated.
        /// </summary>
        public bool IsUnstated
        {
            get
            {
                return this.State == Hstate.Unstated;
            }
        }

        /// <summary>
        /// I'm not thrilled about this.
        /// </summary>
        public bool IsEqualTo(Hval h2)
        { 
            if (this.IsKnown && h2.IsKnown)
            {
                // Note: Won't work for List<LegalEntity>
                if (object.Equals(this.Obj, h2.Obj)) return true;
            }

            return false;
        }
    }
}
