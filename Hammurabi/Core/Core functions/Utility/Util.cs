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
    public static partial class Util
    {
        /// <summary>
        /// Returns the maximum value of a list of input values
        /// </summary>
        public static Hval Maximum(List<Hval> list)
        {
            decimal max = Convert.ToDecimal(list[0].Val);
            foreach (Hval v in list) 
            {
                if (Convert.ToDecimal(v.Val) > max)
                {
                    max = Convert.ToDecimal(v.Val); 
                }
            }
            
            return new Hval(max);
        }

        /// <summary>
        /// Returns the minimum value of a list of input values
        /// </summary>
        public static Hval Minimum(List<Hval> list)
        {
            decimal min = Convert.ToDecimal(list[0].Val);
            foreach (Hval v in list) 
            {
                if (Convert.ToDecimal(v.Val) < min)
                {
                    min = Convert.ToDecimal(v.Val); 
                }
            }
            
            return new Hval(min);
        }

        /// <summary>
        /// Returns a Tvar object of the appropriate type.
        /// </summary>
        public static object ReturnProperTvar<T>()
        {
            if (typeof(T) == typeof(Tbool)) { return new Tbool(); }
            if (typeof(T) == typeof(Tnum))  { return new Tnum(); }
            if (typeof(T) == typeof(Tstr))  { return new Tstr(); }
            if (typeof(T) == typeof(Tdate)) { return new Tdate(); }
            if (typeof(T) == typeof(Tset))  { return new Tset(); }

            // If all else fails (which it better not)...
            return new Tbool();
        }

        /// <summary>
        /// Returns the proper type of Tvar, set eternally to a given value 
        /// </summary>
        public static object ReturnProperTvar<T>(object val)
        {
            if (typeof(T) == typeof(Tbool)) 
            {
                return new Tbool(Convert.ToBoolean(val)); 
            }
            if (typeof(T) == typeof(Tnum))
            {
                return new Tnum(Convert.ToDecimal(val));
            }
            if (typeof(T) == typeof(Tstr))
            {
                return new Tstr(Convert.ToString(val));
            }
            if (typeof(T) == typeof(Tdate))
            {
                return new Tdate(Convert.ToDateTime(val));
            }
            if (typeof(T) == typeof(Tset))
            {
                return new Tset((List<Thing>)val);
            }
            // If all else fails return default...
            return default(T);
        }

        public static object ReturnProperTvar<T>(Hval val)
        {
            if (typeof(T) == new Tbool().GetType())
            {
                return new Tbool(val); 
            }
            if (typeof(T) == new Tnum().GetType())
            {
                return new Tnum(val);
            }
            if (typeof(T) == new Tstr().GetType())
            {
                return new Tstr(val);
            }
            if (typeof(T) == new Tdate().GetType())
            {
                return new Tdate(val);
            }
            if (typeof(T) == new Tset().GetType())
            {
                return new Tset(val);
            }
            // If all else fails return default...
            return default(T);
        }

        /// <summary>
        /// Converts an object to the proper Tvar, but only if it is not already a Tvar.
        /// </summary>
        public static T ConvertToTvar<T>(object t) where T : Tvar
        {
            if (t == null) return (T)ReturnProperTvar<T>();
            if (IsType<T>(t)) return (T)t;
            return (T)ReturnProperTvar<T>(t);
        }
        
        /// <summary>
        /// Determines whether a given Tvar is a specified type.
        /// </summary>
        public static bool IsType<T>(object t)
        {
            if (typeof(T) == t.GetType()) return true;
            return false;
        }

        /// <summary>
        /// Converts a generic list into a generic array.
        /// </summary>
        public static T[] ListToArray<T>(List<T> list)
        {
            T[] result = new T[list.Count];
            for (int i=0; i<list.Count; i++)
            {
                result[i] = list[i];
            }
            return result;
        }
        
        /// <summary>
        /// Converts an object (which could be a Thing) into a string.
        /// </summary>
        public static string ArgToString(object o)
        {
            if (o == null) return "";
            if (o.GetType() == new Thing("").GetType()) return ((Thing)o).Id;
            if (o.GetType() == new Tbool().GetType()) return Convert.ToString(((Tbool)o).Out);
            if (o.GetType() == new Tnum(0).GetType()) return Convert.ToString(((Tnum)o).Out);
            if (o.GetType() == new Tdate().GetType()) return Convert.ToString(((Tdate)o).Out);
            if (o.GetType() == new Tstr().GetType()) return Convert.ToString(((Tstr)o).Out);
            if (o.GetType() == new Tset().GetType()) return Convert.ToString(((Tset)o).Out);
            return Convert.ToString(o);
        }

        /// <summary>
        /// Determine whether two objects have the same value.
        /// </summary>
        /// <remarks>
        /// This is used in Facts (to determine if a fact has been asserted).
        /// </remarks>
        public static bool AreEqual(object o1, object o2)
        {
            if (o1 == null && o2 == null) return true;
            if (o1.GetType() == new Thing("").GetType()) return o1 == o2;
            if (o1.GetType() == new Tbool(true).GetType()) { return ((Tbool)o1 == (Tbool)o2).IsTrue; }
            if (o1.GetType() == new Tnum(0).GetType()) { return ((Tnum)o1 == (Tnum)o2).IsTrue; }
            if (o1.GetType() == new Tstr("").GetType()) { return ((Tstr)o1 == (Tstr)o2).IsTrue; }
            if (o1.GetType() == new Tdate().GetType()) { return ((Tdate)o1 == (Tdate)o2).IsTrue; }
            if (o1.GetType() == new Tset().GetType()) { return ((Tset)o1 == (Tset)o2).IsTrue; }
            return false;
        }
    }
}