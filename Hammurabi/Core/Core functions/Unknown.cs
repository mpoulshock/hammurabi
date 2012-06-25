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

using System.Collections.Generic;

namespace Hammurabi
{
    public partial class H
    {
        /// <summary>
        /// Given a group of Hvals, returns Hstate that trumps the others.
        /// </summary>       
        public static Hstate PrecedingState(List<Hval> list) 
        {
            return PrecedingState(Auxiliary.ListToArray<Hval>(list));
        }
        protected static Hstate PrecedingState(params Hval[] list) 
        {
            // Where there is a stub, there's no need to consider uncertain or unstated facts
            if (AnyHvalsAre(Hstate.Stub, list)) return Hstate.Stub;

            // Where a fact is uncertain, there's no need to query for more information
            if (AnyHvalsAre(Hstate.Uncertain, list)) return Hstate.Uncertain;

            // If one fact is known and the other unstated, the conclusion can't be reached 
            // and is unstated
            if (AnyHvalsAre(Hstate.Unstated, list)) return Hstate.Unstated;
            
            return Hstate.Known;
        }


        /// <summary>
        /// Returns true if any of the input Hvals are unstated.
        /// </summary>       
        protected static bool AnyHvalsAre(Hstate state, params Hval[] list) 
        {
            foreach (Hval h in list)
            {
                if (h.State == state) return true;
            }
            
            return false;
        }
    }
}