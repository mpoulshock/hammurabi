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
using System.Text.RegularExpressions;

namespace Akkadian
{
    /// <summary>
    /// Generates the unit test code from .akk.
    /// </summary>
    public class Tests
    { 
        private static string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| ]+";

        // Opens the testing namespace/class
        public static string unitSpaceOpen =
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
        public static string unitSpaceClose =
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
            line = line.TrimStart('-',' ');
            line = Util.ConvertDate(line); 

            // Test declaration (and method open)
            if (line.StartsWith("Test: "))
                unitTests += Regex.Replace(line, @"Test: (?<id>"+word+")", 
                                       "        [Test]\r\n" +
                                       "        public void Test_${id}()\r\n" +
                                       "        {\r\n" +
                                       "            Facts.Clear();\r\n");
                   
            // Entity declarations
            else if (Util.StartsWithAny(line,"Person","Property","Corp","LegalEntity") != "")
                unitTests += Regex.Replace(line, @"(?<ent>(Person|Property|Corp|LegalEntity)) (?<id>[a-zA-Z0-9_]+)", 
                                       "            ${ent} ${id} = new ${ent}(\"${id}\");\r\n");
                        
            // Test assertion (and method close)
            else if (line.Contains(" =?= "))
                unitTests += Regex.Replace(line, @"(?<actual>"+word+") =\\?= (?<expected>"+word+")", 
                                       "            Assert.AreEqual(${expected}, ${actual});\r\n" +
                                       "        }\r\n\r\n");
            
            // Factual assertions
            else
                unitTests += ConvertFact(line) + "\r\n";
        }
        
        /// <summary>
        /// Converts an .akk fact expression into C#
        /// </summary>
        private static string ConvertFact(string fact)
        {
            string wrd = @"[a-zA-Z0-9]+";
            
            fact = Regex.Replace(fact, 
                                 @"(?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+)\) = (?<val>"+word+@")", 
                                 "            Facts.Assert(${arg1}, \"${fcn}\", ${arg2}, ${val});", RegexOptions.IgnoreCase);  
            
            fact = Regex.Replace(fact, 
                                 @"(?<fcn>"+wrd+@")\((?<arg>"+wrd+@")\) = (?<val>"+word+@")", 
                                 "            Facts.Assert(${arg}, \"${fcn}\", ${val});", RegexOptions.IgnoreCase);  
            
            return fact;
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
    }
}

