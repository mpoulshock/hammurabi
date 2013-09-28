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
    public partial class Facts
    {
        /// <summary>
        /// Saves cached function results.
        /// </summary>
        private static Dictionary<string, Tvar> CacheBase = new Dictionary<string, Tvar>();
        
        /// <summary>
        /// Return a cached value of a function, if it has been cached.
        /// </summary>
        /// <example>
        /// Tbool t = (Tbool)Memo<Tbool>(()=> Fam.AreMarried(A,B) && H.IsMale(A), "1", A, B);
        /// </example>            
        public static Tvar Memo<T> (Func<Tvar> fcn, string nodeID, params object[] args) where T : Tvar
        {
            Tvar result = default(Tvar);
            
            // Assemble the unique key for the function
            string key = nodeID + "_";
            foreach (object o in args) 
            {
                key += o.GetHashCode().ToString() + "_";
            }
            
            // If fcn has been cached, return the cached value
            if (CacheBase.TryGetValue(key, out result)) 
            {
                return (T)result;
            }
            
            // Evaluate fcn
            result = fcn.Invoke();
            
            // If the result is never unstated, add it to the cache
            if (!result.IsEverUnstated)
            {
                CacheBase.Add (key, result);
            }
            
            return result;
        }
    }
}