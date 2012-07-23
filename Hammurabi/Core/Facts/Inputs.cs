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
        /// Queries the fact base - 1 entity.
        /// </summary>
        public static T QueryTvar<T>(string rel, LegalEntity e1) where T : Tvar
        {
            T defaultVal = (T)Auxiliary.ReturnProperTvar<T>(Hstate.Unstated);

            return (T)QueryTvar<T>(rel, e1, null, null, defaultVal);
        }

        /// <summary>
        /// Queries the fact base - 2 entities.
        /// </summary>
        public static T QueryTvar<T>(string rel, LegalEntity e1, LegalEntity e2) where T : Tvar
        {
            T defaultVal = (T)Auxiliary.ReturnProperTvar<T>(Hstate.Unstated);

            return QueryTvar<T>(rel, e1, e2, null, defaultVal);
        }

        /// <summary>
        /// Queries the fact base - 3 entities.
        /// </summary>
        public static T QueryTvar<T>(string rel, LegalEntity e1, LegalEntity e2, LegalEntity e3) where T : Tvar
        {
            T defaultVal = (T)Auxiliary.ReturnProperTvar<T>(Hstate.Unstated);

            return QueryTvar<T>(rel, e1, e2, e3, defaultVal);
        }

        /// <summary>
        /// Queries the fact base for a temporal relationship (fact) among three legal entities.
        /// </summary>
        public static T QueryTvar<T>(string rel, LegalEntity e1, LegalEntity e2, LegalEntity e3, T defaultValue) where T : Tvar
        {
            // Look up fact in table of facts
            foreach (Fact f in FactBase)
            {
                if (f.subject == e1 && f.relationship == rel && f.directObject1 == e2 && f.directObject2 == e3)
                {
                    return (T)f.v;
                }
            }

            // Add the fact to the list of unknown facts
            if (GetUnknowns)
            {
                AddUnknown(rel, e1, e2, e3);
            }

            // If fact is not found, return a default value (usually "unstated")
            return defaultValue;
        }

        /// <summary>
        /// Queries the fact base for a person instance.
        /// </summary> 
        public static Person QueryPerson(string rel, LegalEntity e1)
        {
            foreach (Fact f in FactBase)
            {
                if (f.subject == e1 && f.relationship == rel)
                {
                    return (Person)f.directObject1;
                }
            }

            if (GetUnknowns)
            {
                AddUnknown(rel, e1, null, null);
            }
            
            return new Person("");
        }
    }
}