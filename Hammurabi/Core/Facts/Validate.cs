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
    public partial class Facts : H
    {
        /// <summary>
        /// One of the few global variables: a list of possible input errors.
        /// This list is generated when Validate() is run.
        /// </summary>
        public static List<string> ErrorList = new List<string>();
        
        /// <summary>
        /// Checks input facts for logical and commonsense related inconsistencies.
        /// For each error that is detected, a message is written to the ErrorList.
        /// </summary>
        public static void Validate()
        {
            List<Thing> allPeople = Facts.AllKnownPeople().DistinctEntities();
            
            foreach(Person p in allPeople)
            {
                if (DoB(p) < new DateTime(1895,1,1)) 
                {
                    ErrorList.Add(p + "'s date of birth is too far in the past.");
                }
                
                // TODO: Develop data validation logic
            }

        }
    }
}

