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

namespace Akkadian
{ 
    /// <summary>
    /// Represents legal entities, i.e. things that are recognized by the law.
    /// </summary>
    public class Thing
    {    
        /// <summary>
        /// Identifies each legal entity.
        /// The input value should be unique.
        /// </summary>
        public string Id;

        /// <summary>
        /// Named (stated) Things.
        /// </summary>
        public Thing(string name)
        {
            Id = name;
        }

        /// <summary>
        /// Unstated Things.
        /// </summary>
        public Thing(Hstate h)
        {
            if (h == Hstate.Unstated)  Id = "#Unstated#";
            else if (h == Hstate.Uncertain) Id = "#Uncertain#";
            else if (h == Hstate.Stub)      Id = "#Stub#";
            else Id = "";
        }
    }
}
