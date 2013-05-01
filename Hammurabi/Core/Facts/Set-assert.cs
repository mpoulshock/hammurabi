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

namespace Hammurabi
{
    public partial class Facts
    {
        // TODO: Implement data validation of some sort (e.g. consistency checking)

        /// <summary>
        /// Assert a temporal relation between legal entities (3)
        /// </summary>
        public static void Assert(object e1, string rel, object e2, object e3, Tvar val)
        { 
            CoreAssert(rel, e1, e2, e3, val);
        }

        /// <summary>
        /// Assert a temporal relation between legal entities (2)
        /// </summary>
        public static void Assert(object e1, string rel, object e2, Tvar val)
        { 
            CoreAssert(rel, e1, e2, null, val);
        }

        /// <summary>
        /// Assert a temporal property of one legal entity
        /// </summary>
        public static void Assert(object e1, string rel, Tvar val)
        {
            CoreAssert(rel, e1, null, null, val);
        }

        /// <summary>
        /// Assert a fact to the FactBase.
        /// </summary>
        private static void CoreAssert(string rel, object e1, object e2, object e3, Tvar val)
        {
            // Don't assert a fact that's already been asserted
            if (!HasBeenAsserted(rel, e1, e2, e3))
            {
                // Assert the fact
                Fact f = new Fact(rel, e1, e2, e3, val);
                FactBase.Add(f);

                // TODO: This breaks when the objects are not Things (hence the try-catch)
                try
                {
                    // Add Things to the ThingBase
                    AddThing((Thing)e1);
                    AddThing((Thing)e2);
                    AddThing((Thing)e3);

                    // Look for additional inferences that can be drawn, based on assumptions.
                    Assumptions.TriggerInferences(rel, (Thing)e1, (Thing)e2, (Thing)e3, val);
                }
                catch
                {
                }
            }
        }
    }
}