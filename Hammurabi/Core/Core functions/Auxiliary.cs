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
using System.IO;
using System.Xml.Serialization;

namespace Hammurabi
{
    public static class Auxiliary
    {
        /// <summary>
        /// Returns the maximum value of a list of input values
        /// </summary>
        public static decimal Maximum(List<object> list)
        {
            decimal max = Convert.ToDecimal(list[0]);
            
            foreach (object v in list) 
            {
                if (Convert.ToDecimal(v) > max)
                {
                    max = Convert.ToDecimal(v); 
                }
            }
            
            return max;
        }
        
        /// <summary>
        /// Returns the minimum value of a list of input values
        /// </summary>
        public static decimal Minimum(List<object> list)
        {
            decimal min = Convert.ToDecimal(list[0]);
            
            foreach (object v in list) 
            {
                if (Convert.ToDecimal(v) < min)
                {
                    min = Convert.ToDecimal(v); 
                }
            }
            
            return min;
        }
        
        /// <summary>
        /// Determines whether a legal entity is a member of a Tset
        /// </summary>
        public static Tbool IsMemberOfSet(LegalEntity entity, Tset theSet)
        {
            // TODO: Implement unknown for LegalEntities?
            
            if (theSet.IsUnknown) { return new Tbool(); }
            
            Tbool result = new Tbool();
            
            foreach (KeyValuePair<DateTime,object> slice in theSet.IntervalValues)
            {
                List<LegalEntity> entities = (List<LegalEntity>)slice.Value;
                
                result.AddState(slice.Key, entities.Contains(entity));
            }
            
            return result.Lean;
        }
        
        /// <summary>
        /// Returns a Tvar object of the appropriate type.
        /// </summary>
        public static object ReturnProperTvar<T>()
        {
            if (typeof(T) == new Tbool().GetType()) { return new Tbool(); }
            if (typeof(T) == new Tnum().GetType())  { return new Tnum(); }
            if (typeof(T) == new Tstr().GetType())  { return new Tstr(); }
            if (typeof(T) == new Tdate().GetType()) { return new Tdate(); }
            if (typeof(T) == new Tset().GetType())  { return new Tset(); }
            
            // If all else fails (which it better not)...
            return new Tbool();
        }

        /// <summary>
        /// Returns the proper type of Tvar, set eternally to a given value 
        /// </summary>
        public static object ReturnProperTvar<T>(object val)
        {
            if (typeof(T) == new Tbool().GetType())
            {
                return new Tbool((bool?)val);
            }
            if (typeof(T) == new Tnum().GetType())
            {
                return new Tnum(val);
            }
            if (typeof(T) == new Tstr().GetType())
            {
                return new Tstr(Convert.ToString(val));
            }
            if (typeof(T) == new Tdate().GetType())
            {
                return new Tdate(Convert.ToDateTime(val));
            }
            if (typeof(T) == new Tset().GetType())
            {
                return new Tset((List<LegalEntity>)val);
            }
            // If all else fails return default...
            return default(T);
        }
        
//        /// <summary>
//        /// Serialize an object to a string.
//        /// </summary>
//        public static string SerializeToStr<T>(T obj)
//        {
//              StringWriter sw = new StringWriter();
//              XmlSerializer serializer = new XmlSerializer(typeof(T));
//              serializer.Serialize(sw, obj);
//              return sw.ToString();
//        }
//        
//        /// <summary>
//        /// Deserialize a string to an object.
//        /// </summary>
//        public static T DeserializeFromStr<T>(string s)
//        {
//            StringReader sr = new StringReader(s);
//            XmlSerializer serializer = new XmlSerializer(typeof(T));
//            return (T)serializer.Deserialize(sr); //new System.Text.UTF8Encoding(false)
//        }
    }
    
}