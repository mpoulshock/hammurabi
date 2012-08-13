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
            Thing subj = response.NextFact.subject;
            Thing obj = response.NextFact.object1;
            string rel = response.NextFact.relationship;
            string qType = Templates.GetQ(rel).questionType;

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
                    Facts.Assert(subj, rel, obj, new Tbool(Convert.ToBoolean(val)));
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
                    Facts.Assert(subj, rel, obj, new Tstr(Convert.ToString(val)));
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
                    Facts.Assert(subj, rel, obj, new Tnum(Convert.ToDouble(val)));
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
                    Facts.Assert(subj, rel, obj, new Tdate(DateTime.Parse(val)));
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
                    string[] items = val.Split(new char[] {';'});
                    List<Thing> list = new List<Thing>();

                    foreach (string i in items)
                    {
                        list.Add(new Thing(i.Trim()));
                    }

                    Tset result = new Tset(list);
                    Facts.Assert(subj, rel, obj, result);
                    AkkTest.testStr += "[[" + val + "]]\r\n";
                }
            }
        }
    }
}