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
            if (qType == "bool")
            {
                Tbool result = new Tbool(Convert.ToBoolean(val));
                if (obj == null) { Facts.Assert(subj, rel, result); }
                else { Facts.Assert(subj, rel, (Thing)obj, result); }
                AkkTest.testStr += val + "\r\n";
            }
            else if (qType == "string")
            {
                Tstr result = new Tstr(Convert.ToString(val));
                if (obj == null) { Facts.Assert(subj, rel, result); }
                else { Facts.Assert(subj, rel, (Thing)obj, result); }
                AkkTest.testStr += "\"" + val + "\"\r\n";
            }
            else if (qType == "numvar" || qType == "dollars")
            {
                Tnum result = new Tnum(Convert.ToDouble(val));
                if (obj == null) { Facts.Assert(subj, rel, result); }
                else { Facts.Assert(subj, rel, (Thing)obj, result); }
                AkkTest.testStr += val + "\r\n";
            }
            else if (qType == "date")
            {
                Tdate result = new Tdate(DateTime.Parse(val));
                if (obj == null) { Facts.Assert(subj, rel, result); }
                else { Facts.Assert(subj, rel, (Thing)obj, result); }
                AkkTest.testStr += val + "\r\n";
            }
            else if (qType == "set")
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
