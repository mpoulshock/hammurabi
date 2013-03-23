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
using Hammurabi;

namespace Interactive
{
    /// <summary>
    /// Generates a DOS-based, utterly primitive interview designed
    /// to illustrate how Hammurabi can simulate backwards chaining.
    /// </summary>
    public static partial class Interview 
    {    
        /// <summary>
        /// Asserts a fact.
        /// </summary>
        public static void AssertAnswer(Engine.Response response, string val)
        {
            // Get the data pertaining to the current question
            Thing subj = (Thing)response.NextFact.Arg1;
            Thing obj = (Thing)response.NextFact.Arg2;
            string rel = response.NextFact.Relationship;
            string qType = Templates.GetQ(rel).questionType;

            // Create the fact/relationship text for the .akk unit test
            if (qType != "Tset")  AkkTest.testStr += AkkTest.assertedRelationship;

            // Assert the fact (converted to the proper type of Tvar)
            if (qType == "Tbool")
            {
                // Asserts the fact (handling uncertainty) 
                // Also, creates part of the .akk unit test string
                if (val == "?") 
                {
                    Facts.Assert(subj, rel, obj, new Tbool(Hstate.Uncertain));
                    AkkTest.testStr += "Tbool(?)\r\n";
                }
                else 
                {
                    Facts.Assert(subj, rel, obj, TboolFromTemporalString(val));
                    AkkTest.testStr += val + "\r\n";
                }
            }
            else if (qType == "Tstr")
            {
                if (val == "?") 
                {
                    Facts.Assert(subj, rel, obj, new Tstr(Hstate.Uncertain));
                    AkkTest.testStr += "Tstr(?)\r\n";
                }
                else 
                {
                    Facts.Assert(subj, rel, obj, TstrFromTemporalString(val));
                    AkkTest.testStr += "\"" + val + "\"\r\n";
                }
            }
            else if (qType == "Tnum")
            {
                if (val == "?") 
                {
                    Facts.Assert(subj, rel, obj, new Tnum(Hstate.Uncertain));
                    AkkTest.testStr += "Tnum(?)\r\n";
                }
                else 
                {
                    Facts.Assert(subj, rel, obj, TnumFromTemporalString(val));
                    AkkTest.testStr += val + "\r\n";
                }
            }
            else if (qType == "Tdate")
            {
                if (val == "?") 
                {
                    Facts.Assert(subj, rel, obj, new Tdate(Hstate.Uncertain));
                    AkkTest.testStr += "Tdate(?)\r\n";
                }
                else 
                {
                    Facts.Assert(subj, rel, obj, TdateFromTemporalString(val));
                    AkkTest.testStr += val + "\r\n";
                }
            }
            else if (qType == "Tset")
            {
                if (val == "?") 
                {
                    Facts.Assert(subj, rel, obj, new Tset(Hstate.Uncertain));
                    AkkTest.testStr += "Tset(?)\r\n";
                }
                else
                {
                    // Assert an empty set
                    if (val == "[]")
                    {
                        Tset result = new Tset();
                        result.SetEternally();
                        Facts.Assert(subj, rel, obj, result);
                    }
                    else
                    {
                        // Create a list of Things
                        string[] items = val.Split(new char[] {';'});
                        List<Thing> list = new List<Thing>();
                        string thingList = "";      // for .akk unit tests
                        foreach (string i in items)
                        {
                            string name = i.Trim();
                            list.Add(Facts.AddThing(name));
                            thingList += name + ",";
                        }

                        // Assert the Tset
                        Facts.Assert(subj, rel, obj, new Tset(list));

                        // Build the .akk unit test string
                        AkkTest.testStr += "- Things " + thingList.TrimEnd(',') + "\r\n";
                        AkkTest.testStr += AkkTest.assertedRelationship;
                        AkkTest.testStr += "[[" + val.Replace(";",",") + "]]\r\n";
                    }
                }
            }
        }

        /// <summary>
        /// Creates a Tbool from a string representing a time-varying value.
        /// </summary>
        /// <remarks>
        /// Sample input: {2012-01-01: true; Time.DawnOf: false}
        /// Note the reverse chronological order.
        /// </remarks>
        private static Tbool TboolFromTemporalString(string val)
        {
            if (val.StartsWith("{"))
            {
                // Assert each of the time-value pairs
                Tbool result = new Tbool();
                foreach (string s in TimeValuePairs(val))
                {
                    string[] parts = s.Split(new char[] {':'});
                    DateTime datePart = Convert.ToDateTime(parts[0].Trim().Replace("Time.DawnOf","1800-01-01"));
                    bool valPart = Convert.ToBoolean(parts[1].Trim());
                    result.AddState(datePart, valPart);
                }
                return result;
            }

            return new Tbool(Convert.ToBoolean(val));
        }

        /// <summary>
        /// Creates a Tstr from a string representing a time-varying value.
        /// </summary>
        /// <remarks>
        /// Sample input: {2012-01-01: "Hello"; Time.DawnOf: "world"}
        /// </remarks>
        private static Tstr TstrFromTemporalString(string val)
        {
            if (val.StartsWith("{"))
            {
                Tstr result = new Tstr();
                foreach (string s in TimeValuePairs(val))
                {
                    string[] parts = s.Split(new char[] {':'});
                    DateTime datePart = Convert.ToDateTime(parts[0].Trim().Replace("Time.DawnOf","1800-01-01"));
                    result.AddState(datePart, parts[1].Trim());
                }
                return result;
            }
            
            return new Tstr(val);
        }

        /// <summary>
        /// Creates a Tnum from a string representing a time-varying value.
        /// </summary>
        /// <remarks>
        /// Sample input: {2012-01-01: 5; Time.DawnOf: $55,000.01}
        /// </remarks>
        private static Tnum TnumFromTemporalString(string val)
        {
            if (val.StartsWith("{"))
            {
                Tnum result = new Tnum();
                foreach (string s in TimeValuePairs(val))
                {
                    string[] parts = s.Split(new char[] {':'});
                    DateTime datePart = Convert.ToDateTime(parts[0].Trim().Replace("Time.DawnOf","1800-01-01"));
                    decimal valPart = Convert.ToDecimal(parts[1].Trim(' ','$').Replace(",",""));
                    result.AddState(datePart, valPart);
                }
                return result;
            }
            
            return new Tnum(val);
        }

        /// <summary>
        /// Creates a Tdate from a string representing a time-varying value.
        /// </summary>
        /// <remarks>
        /// Sample input: {2012-01-01: 2012-12-31; Time.DawnOf: 2000-12-31}
        /// </remarks>
        private static Tdate TdateFromTemporalString(string val)
        {
            if (val.StartsWith("{"))
            {
                Tdate result = new Tdate();
                foreach (string s in TimeValuePairs(val))
                {
                    string[] parts = s.Split(new char[] {':'});
                    DateTime datePart = Convert.ToDateTime(parts[0].Trim().Replace("Time.DawnOf","1800-01-01"));
                    DateTime valPart = Convert.ToDateTime(parts[1].Trim());
                    result.AddState(datePart, valPart);
                }
                return result;
            }
            
            return new Tdate(val);
        }

        /// <summary>
        /// Converts timeline string into array of time-value pairs.
        /// </summary>
        private static string[] TimeValuePairs(string val)
        {
            val = val.Trim('{','}');
            string[] pairs = val.Split(new char[] {';'});
            return pairs;
        }
    }
}