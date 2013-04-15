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
using System.Text.RegularExpressions;

namespace Akkadian
{
    /// <summary>
    /// Generates the unit test code from .akk.
    /// </summary>
    public class Tests
    { 
        private const string word = @"[-!\+\*/A-Za-z0-9\.;:\(\),""'_<>=&|\[\]\?{} ]+";

        // Opens the testing namespace/class
        public const string unitSpaceOpen =
            "\r\nnamespace Hammurabi.UnitTests\r\n" +
            "{\r\n" +
            "    [TestFixture]\r\n" +
            "    public partial class Experimental : H\r\n" +
            "    {\r\n";
        
        public static string unitSpaceOpen2(string spce)
        {
            return 
            "\r\nnamespace Hammurabi.UnitTests\r\n" +
            "{\r\n" +
            "    [TestFixture]\r\n" +
            "    public partial class Test_" + spce + " : H\r\n" +
            "    {\r\n";
        }
        
        // Accumulates lines as they are processed
        public static string unitTests = "";
        
        // Closes the namespace/class
        public const string unitSpaceClose =
            "    }\r\n" +
            "}\r\n";
        
        // Unit test output
        public static string UnitTestText =
            unitSpaceOpen +
            unitTests +
            unitSpaceClose;

        
        /// <summary>
        /// Converts the .akk unit test to C#.
        /// </summary>
        public static void ProcessTestLine(string line)
        {
            // Do some initial text conversions
            line = line.TrimStart('-',' ');
            line = Util.ConvertDate(line);
            line = Util.RemoveCurrencyStyling(line);

            // Handle assertions with uncertain values
            line = Regex.Replace(line, @"(?<typ>(Tbool|Tstr|Tnum|Tdate|Tset))\(\?\)", 
                                       "new ${typ}(Hstate.Uncertain)");

            // Test declaration (and method open)
            if (line.StartsWith("Test: "))
                unitTests += Regex.Replace(line, @"Test: (?<id>"+word+")", 
                                       "        [Test]\r\n" +
                                       "        public void Test_${id}()\r\n" +
                                       "        {\r\n" +
                                       "            Facts.Clear();\r\n");
                   
            // Thing declarations (one)         
            else if (line.StartsWith("Thing "))
                unitTests += Regex.Replace(line, @"Thing (?<id>[a-zA-Z0-9_]+)", 
                                       "            Thing ${id} = new Thing(\"${id}\");\r\n");

            // Thing declarations (multiple)         
            else if (line.StartsWith("Things "))
                unitTests += DeclareThings(line);

            // Test assertion (and method close)
            else if (line.Contains(" =?= "))
                unitTests += Regex.Replace(line, @"(?<actual>"+word+") =\\?= (?<expected>"+word+")", 
                                       "            Assert.AreEqual(${expected}, ${actual});\r\n" +
                                       "        }\r\n\r\n");
            
            // Factual assertions
            else unitTests += ConvertFact(line) + "\r\n";
        }

        /// <summary>
        /// Converts an .akk fact expression into C#
        /// </summary>
        public static string ConvertFact(string fact)
        {
            const string wrd = @"[a-zA-Z0-9\.]+";

            // Convert a temporal fact to the proper C# expression
            if (fact.Contains("{") && fact.Contains("}"))
            {
                int open = fact.IndexOf("{");
                int close = fact.IndexOf("}");
                string brack = fact.Substring(open, close-open+1);
                fact = fact.Replace(brack, ConvertTemporalFact(brack));
            }

            // Convert the assertion from Akkadian to C#
            fact = Regex.Replace(fact, 
                @"(?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+)(?<comma1>,)?(?<arg2>[a-zA-Z0-9 ]+)?(?<comma2>,)?(?<arg3>[a-zA-Z0-9 ]+)?\) = (?<val>"+word+@")", 
                "            Facts.Assert(${arg1}, \"${fcn}\"${comma1}${arg2}${comma2}${arg3}, ${val});");  

            // Convert lists of Tset members to C#
            fact = Regex.Replace(fact, @"\[\[(?<list>[a-zA-Z0-9,\. ]+)\]\]", @"new Tset(new List<Thing>(){${list}})");

            return fact;
        }

        /// <summary>
        /// Converts a temporal fact value into an expression using MakeTvar().
        /// </summary>
        public static string ConvertTemporalFact(string fact)
        {
            // Parse string
            fact = fact.Trim('{','}',' ');
            string[] timepts = fact.Split(';');
            Array.Reverse(timepts);     // In a Switch statement, later time-value pairs must come first

            // Detect Tbool type by looking at the value in the first time-value pair
            string[] firstPair = timepts[0].Split(':');
            string type = DetectType(firstPair[1].Trim());

            // Generate MakeTvar() expression
            string result = "Switch<" + type + ">(";
            foreach (string s in timepts)
            {
                string[] pair = s.Split(':');
                result += "()=> TheTime.IsAtOrAfter(" + pair[0].Replace("Dawn","Time.DawnOf") + "), ";   // Add the date
                result += "()=> " + pair[1] + ", ";   // Add the value
            }

            result = result.TrimEnd(',',' ');
            return result + ")";
        }

        /// <summary>
        /// Given a string, detects the kind of Tvar it represents.
        /// </summary>
        private static string DetectType(string input)
        {
            // Doesn't handle Tsets yet...
            if (input == "true" || input == "false") return "Tbool";
            else if (input.StartsWith("Date(")) return "Tdate";
            else if (Util.IsNumber(input)) return "Tnum";
            else return "Tstr";
        }

        /// <summary>
        /// Determines whether a line is that of an .akk unit test.
        /// </summary>
        public static bool IsTestLine(string line)
        {
            return Util.Depth(line) == 0 &&
                   (line.StartsWith("Test: ") ||
                    line.StartsWith("- "));
        }

        /// <summary>
        /// Parse a line declaring multiple Things in .akk, and declare them in C#.
        /// </summary>
        public static string DeclareThings(string line)
        {
            string result = "";
            line = line.Replace("Things ","");
            string[] things = line.Split(',');
            foreach (string s in things)
            {
                string t = s.Trim();
                result += "            Thing " + t + " = new Thing(\"" + t + "\");\r\n";
            }
            return result;
        }

        /// <summary>
        /// Writes the unit tests to the C# file.
        /// </summary>
        public static string WriteUnitTests(string nameSpace)
        {
            if (unitTests.Trim() != "")
            {
                return unitSpaceOpen2(nameSpace) +
                          unitTests +
                          unitSpaceClose;
            }

            return "";
        }
    }
}