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
    /// <summary>
    /// Represents legal entities, i.e. things that are recognized by the law
    /// as having rights and/or obligations.
    /// This class and its inherited classes form something of an ontology of
    /// legal concepts that are passed as arguments in the law-related methods
    /// of the project.
    /// </summary>
    public class Thing
    {    
        /// <summary>
        /// Identifies each legal entity.
        /// The input value should be unique.
        /// </summary>
        public string Id;

//        public Thing(string name)
//        {
//            Id = name;
//        }

//        public Thing(Tset theMembers)
//        {
//            Members = theMembers;
//        }

        /// <summary>
        /// The set of legal entities that compose the (parent) entity.
        /// </summary>
        public Tset Constituents
        {
            get;
            set;
        }
    }
    
    /// <summary>
    /// Represents natural persons (i.e. humans).
    /// </summary>
    public class Person : Thing
    {
        public Person(string name)
        {
            Id = name;
        }
    }

    /// <summary>
    /// Represents a corporate entity.
    /// Includes corporations, partnerships, sole props, gov't agencies, etc.
    /// </summary>
    public class Corp : Thing
    {
        public Corp(string identifier)
        {
            Id = identifier;
        }
    }

    /// <summary>
    /// Represents tangible or intangible property, i.e. something that can
    /// be owned.  Synonymous with "asset."
    /// </summary>
    public class Property : Thing
    {
        public Property(string identifier)
        {
            Id = identifier;
        }
    }
}
