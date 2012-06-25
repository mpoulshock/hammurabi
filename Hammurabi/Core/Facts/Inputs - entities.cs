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
using System.Collections.Generic;

namespace Hammurabi
{
    public partial class Facts
    {
        /// <summary>
        /// Queries the fact base to get all objects in a symmetrical
        /// relationship with a given object.
        /// For example: spousesOfBill = AllXSymmetrical(Bill,"IsMarriedTo");
        /// </summary>
//        public static Tset AllXSymmetrical(LegalEntity entity, string relationship)
//        {
//            return Facts.AllXSuchThatX(relationship, entity) |
//                   Facts.AllXThat(entity, relationship);
//        }


        /// <summary>
        /// Returns a set of all known people except a given person.
        /// </summary>
        public static Tset EveryoneExcept(Person p)
        {
            return AllKnownPeople() - p;
        }

        /// <summary>
        /// Queries the fact base to get all known people and returns
        /// the result in an eternal Tset.
        /// </summary>
        public static Tset AllKnownPeople()
        {
            Tset result = new Tset();
            List<LegalEntity> thePeople = new List<LegalEntity>();
            
            foreach (Fact f in FactBase)
            {
                if (f.subject.GetType() == new Person("").GetType())
                    if (!thePeople.Contains(f.subject))
                        thePeople.Add(f.subject);

                if(f.directObject != null && f.directObject.GetType() == new Person("").GetType())
                    if (!thePeople.Contains(f.directObject))
                        thePeople.Add(f.directObject);
            }
            
            result.AddState(Time.DawnOf,new Hval(thePeople));
            return result;
        }
            
        /// <summary>
        /// Queries the fact base to get all known items of property and returns
        /// the result in an eternal Tset.
        /// </summary>
        /// TODO: Refactor with AllKnownPeople
        public static Tset AllKnownProperty()
        {
            Tset result = new Tset();
            List<LegalEntity> theProp = new List<LegalEntity>();
            
            foreach (Fact f in FactBase)
            {
                if (f.subject.GetType() == new Property("").GetType())
                    if (!theProp.Contains(f.subject))
                        theProp.Add(f.subject);

                if(f.directObject != null && f.directObject.GetType() == new Property("").GetType())
                    if (!theProp.Contains(f.directObject))
                        theProp.Add(f.directObject);
            }
            
            result.AddState(Time.DawnOf,new Hval(theProp));
            return result;
        }
    }
}