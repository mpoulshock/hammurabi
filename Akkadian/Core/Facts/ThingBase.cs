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
        /// Keeps track of Things that have been asserted during the user session.
        /// </summary>
        private static List<Thing> ThingBase = new List<Thing>();

        /// <summary>
        /// Adds a thing to the ThingBase, if it's not already there.  Returns the Thing itself.
        /// </summary>
        public static Thing AddThing(Thing thing)
        {
            // If a Thing with that name exists, return it
            foreach (Thing t in ThingBase) 
            {
                if (IsAddable(thing) && t.Id == thing.Id) 
                {
                    return thing;
                }
            }
            
            // Else (if thing does not exist), add and return it
            if (IsAddable(thing)) 
            {
                ThingBase.Add(thing);
            }
            return thing;
        }

        /// <summary>
        /// Adds a thing to the ThingBase, based on the uniqueness of its name.  Returns the Thing itself.
        /// </summary>
        new public static Thing AddThing(string thingName)
        {
            Thing thing = new Thing(thingName);
            
            // If a Thing with that name exists, return it
            foreach (Thing t in ThingBase) 
            {
                if (IsAddable(thing) && t.Id == thingName) 
                {
                    return t;
                }
            }
            
            // Else, if thing does not exist, create and assert a new one
            
            if (IsAddable(thing)) 
            {
                ThingBase.Add(thing);
            }
            return thing;
        }

        /// <summary>
        /// Only known Things can be added.
        /// </summary>
        private static bool IsAddable(Thing t)
        {
            if (t == null) return false;
            if (t.Id == "") return false;
            return true;
        }

        /// <summary>
        /// Retracts all Things in the ThingBase
        /// </summary>
        public static void ClearThings()
        {
            ThingBase.Clear();
        }

        /// <summary>
        /// Counts how many Things there are in the user session. 
        /// </summary>
        public static int ThingCount()
        {
            return ThingBase.Count;
        }
    }
}