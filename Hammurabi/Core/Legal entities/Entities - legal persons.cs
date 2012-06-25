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
    public abstract class LegalEntity
    {    
        /// <summary>
        /// Identifies each legal entity.
        /// The input value should be unique.
        /// </summary>
        public string Id;
        
        public Tstr Location;
        
        /// <summary>
        /// Indicates when a legal entity is a member of a Tset.
        /// </summary>
        public Tbool IsMemberOf(Tset s)
        {
            return Auxiliary.IsMemberOfSet(this,s);
        }
        
        public Tstr EntityType
        {
            get 
            {
                if (Object.ReferenceEquals(this.GetType(), new Person().GetType())) return "Person";
                if (Object.ReferenceEquals(this.GetType(), new Corp().GetType())) return "Corp";
                return "?";
            }
        }
    }
    
    /// <summary>
    /// Represents natural persons (i.e. humans).
    /// </summary>
    public class Person : LegalEntity
    {
        // Characteristics - non-temporal
        public string Name;
        
        /// <summary>
        /// Constructs a person with a given name.
        /// </summary>
        public Person(string name)
        {
            Id = name;
        }
        
        public Person()
        {
        }
    }
    
    /// <summary>
    /// A catch-all type of legal entity for objects that don't fit other categories.
    /// Could include, for example, households, married couples, estates, etc.
    /// </summary>
    public class Entity : LegalEntity
    {      
        /// <summary>
        /// The set of legal entities that compose the (parent) entity.
        /// </summary>
        public Tset Members
        {
            get;
            set;
        }
        
        public Entity()
        {
        }
        
        public Entity(Tset theMembers)
        {
            Members = theMembers;
        }
    }
    
    /// <summary>
    /// Represents a corporate entity.
    /// Includes corporations, partnerships, sole props, gov't agencies, etc.
    /// </summary>
    public class Corp : LegalEntity
    {
        public string Name;
        
        public Corp()
        {
        }
        
        public Corp(string identifier)
        {
            Id = identifier;
        }
    }
}
