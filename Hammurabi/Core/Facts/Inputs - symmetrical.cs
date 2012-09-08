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

namespace Hammurabi
{
    public partial class Facts
    {
        /// <summary>
        /// Returns a symmetrical input boolean fact.  
        /// For example, A is married to B if B is married to A.
        /// </summary>
        /// <remarks>
        /// See unit tests for the truth table, which ensures that the existence
        /// of one false causes a false to be returned.
        /// Note that the Sym() functions are also designed to add facts to the
        /// Facts.Unknowns list in the proper order and with proper short-
        /// circuiting.
        /// </remarks>
        public static Tbool Sym(Thing subj, string rel, Thing directObj)
        {
            if (Facts.HasBeenAsserted(rel, subj, directObj))
            {
                return QueryTvar<Tbool>(rel, subj, directObj);
            }
            
            return QueryTvar<Tbool>(rel, directObj, subj);
        }

        /// <summary>
        /// Returns either of the two Tbools.
        /// </summary>
        /// <remarks>
        /// This function is needed because if either A or B is false, the
        /// funtion should return false.  If, instead, A || B were used to
        /// analyze input facts, and if A were false and B were unknown, the 
        /// function would erroneously return unknown.
        /// </remarks>
        public static Tbool Either(Tbool A, Tbool B)
        {
            if (!A.IsEternallyUnknown)
            {
                return A;
            }
            
            return B;
        }   
    }
}