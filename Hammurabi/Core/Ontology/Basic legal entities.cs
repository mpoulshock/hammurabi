// Copyright (c) 2011 The Hammurabi Project
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
        
        /// <summary>
        /// Indicates when a legal entity is a member of a Tset.
        /// </summary>
        public Tbool IsMemberOf(Tset s)
        {
            return Auxiliary.IsMemberOfSet(this,s);
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
        
        /// <summary>
        /// Citizenship/immigration-related characteristics (temporal).
        /// </summary>
        public Tstr USImmStatus
        {
            get { return Facts.InputTstr(this, r.USImmigrationStatus); }
        }
        public Tbool IsUSResident
        {
            get { return this.CountryOfResidence == "United States"; }
        }
        public Tbool IsResidentOf(string country)
        {
            return this.CountryOfResidence == country;
        }
        public Tstr CountryOfResidence
        {
            get { return Facts.InputTstr(this, r.CountryOfResidence); }
        }
        public Tbool IsUSCitizen
        {
            get 
            { 
                // Too many options here?
                return this.CountryOfCitizenship == "United States" |
                       Facts.InputTbool(this, "IsUSCitizen") |
                       this.USImmStatus == "Citizen";
            }
        }
        public Tbool IsUSNational
        {
            get { return this.USImmStatus == "National"; }
        }
        public Tstr CountryOfCitizenship
        {
            get { return Facts.InputTstr(this, r.CountryOfCitizenship); }
        }
        
        /// <summary>
        /// Various personal characteristics (temporal).
        /// </summary>
        public Tstr Gender
        {
            get { return Facts.InputTstr(this, "Gender"); }
        }
        public Tbool IsFemale
        {
            get { return Gender == "Female"; }
        }
        public Tbool IsMale
        {
            get { return Gender == "Male"; }
        }
        public Tnum Age     // in years
        {
            get { return Time.IntervalsSince(DateOfBirth, DateOfBirth.AddYears(110), Time.IntervalType.Year); }
        }
        public DateTime DateOfBirth
        {
            get { return Facts.InputDate(this, r.DateOfBirth); }
        }
        public DateTime DateOfDeath
        {
            get { return Facts.InputDate(this, r.DateOfBirth); }
        }
        public Tbool IsMarried
        {
            get { return Facts.InputTstr(this, r.MaritalStatus) == "Married" ; }
        }
        public Tbool IsDisabled
        {
            // How should this relate to 29 CFR Part 1630?
            get { return Facts.InputTbool(this, r.IsDisabled); }
        }
        public Tbool IsIncapableOfSelfCare
        {
            get { return Facts.InputTbool(this, r.IsIncapableOfSelfCare); }
        }
        
//        /// <summary>
//        /// Returns true when the person is alive.
//        /// </summary>
//        public Tbool IsAlive()
//        {
//            Tbool result = new Tbool();
//            result.AddState(Time.DawnOf,false);
//            
//            if (DateOfDeath == null)
//            {
//                result.AddState(DateOfBirth,true);
//                result.AddState(DateTime.Now,null);
//            }
//            else
//            {
//                result.AddState(DateOfBirth,true);
//                result.AddState(DateOfDeath,false);
//            }
//            
//            return result;
//        }

    }
    
    /// <summary>
    /// Represents tangible or intangible property, i.e. something that can
    /// be owned.
    /// </summary>
    public class Property : LegalEntity
    {
        public Tnum ValueInDollars;
        
        /// <summary>
        /// Constructs a Property object with a given name.
        /// </summary>
        public Property(string identifier)
        {
            Id = identifier;
        }
    }
    
    /// <summary>
    /// Represents a corporate entity.
    /// </summary>
    public class CorporateEntity : LegalEntity
    {
        public string Name;
        
        public Tnum NumberOfEmployees
        {
            get { return Facts.InputTnum(this, r.NumberOfEmployees); }
        }
        
        public Tbool IsPublicAgency
        {
            get { return new Tbool(Object.ReferenceEquals(this.GetType(), new Govt().GetType())); }
        }
    }
    
    /// <summary>
    /// Represents a governmental entity.
    /// </summary>
    public class Govt : CorporateEntity
    {
        public string Jurisdiction;
        public string JurisdictionType;
        
        /// <summary>
        /// Constructs a government/agency object with a given name.
        /// </summary>
        public Govt(string identifier)
        {
            Id = identifier;
        }
        /// <summary>
        /// Constructs a nameless government/agency object.
        /// </summary>
        public Govt()
        {
        }
        
    }
    
    /// <summary>
    /// Represents a non-governmental entity (usually a private
    /// corporation).
    /// </summary>
    public class Corp : CorporateEntity
    {
        /// <summary>
        /// Constructs a corporation object with a given name.
        /// </summary>
        public Corp(string identifier)
        {
            Id = identifier;
        }
    }
    
    
    
}



