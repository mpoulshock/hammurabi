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

using System.Text.RegularExpressions;

namespace Akkadian
{
    public class Meth
    {
        // Regex for detecting function names
        public static string wrd = @"[a-zA-Z0-9_]+";

        // Pieces of code that determine whether a method's input entities are unknown
        public static string entArg1 = "Hstate h = EntityArgIsUnknown(";
        public static string entArg2 = ");\r\n            if (h != Hstate.Known) return new ";
        public static string entArg3 = "(h);\r\n\r\n";

        public static string QueryTvarTransform(string line)
        {
            string typs = @"(Tbool|Tnum|Tstr|Tset|Tdate|Date|Thing)";

            // Rule conclusion line 
            if (Util.IsInputRule(line))  // Starts w/ TvarIn at indent 0...
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, 
                          @"TboolInSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                          "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n        {\r\n            return Facts.Sym(${arg1}, \"${fcn}\", ${arg2});");  
    
                // Functions with three arguments
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@"), ?(?<argtyp3>"+wrd+@" )(?<arg3>"+wrd+@")\)",
                          "        public static ${type} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2}, ${argtyp3} ${arg3})\r\n        {\r\n            return Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}, ${arg2}, ${arg3});");  


                // Functions with two arguments
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                          "        public static ${type} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n        {\r\n            return Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}, ${arg2});");  

                // Functions with one argument
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp>"+wrd+@" )(?<arg>"+wrd+@")\)",
                          "        public static ${type} ${fcn}(${argtyp} ${arg})\r\n        {\r\n            return Facts.QueryTvar<${type}>(\"${fcn}\", ${arg});");  
            }

            // Is rule condition line, not rule conclusion
            else                
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, @"TboolInSym (?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+)\)", 
                                     "Facts.Sym(${arg1}, \"${fcn}\", ${arg2})");  

                // Functions with three arguments
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+),(?<arg3>[a-zA-Z0-9 ]+)\)", 
                                     "Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}, ${arg2}, ${arg3})", RegexOptions.IgnoreCase);  

                // Functions with two arguments
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+)\)", 
                                     "Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}, ${arg2})", RegexOptions.IgnoreCase);  

                // Functions with one argument
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg>"+wrd+@")\)", "Facts.QueryTvar<${type}>(\"${fcn}\", ${arg})", RegexOptions.IgnoreCase);  
            }
            
            return line;
        }
        
        /// <summary>
        /// Handles intermediate facts (facts asserted mid-tree)
        /// </summary>
        public static string CreateIntermediateAssertion(string line)
        {
            // Symmetrical facts, e.g. TboolInSym Fcn(Thing p1, Thing p2) =
            if (line.StartsWith("TboolInSym"))
            {
                line = Regex.Replace(line, 
                    @"TboolInSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\) =",
                     "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                     "        {\r\n" +
                     "            " + entArg1 + "${arg1},${arg2}" + entArg2 + "Tbool" + entArg3 +
                     "            if (Facts.HasBeenAssertedSym(${arg1}, \"${fcn}\", ${arg2}))\r\n" +
                     "            {\r\n" +
                     "                return Facts.Sym(${arg1}, \"${fcn}\", ${arg2});\r\n" +
                     "            }\r\n\r\n");
            }
            else
            {
                // Three arguments
                line = Regex.Replace(line, 
                    @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing))In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@"), ?(?<argtyp3>"+wrd+@" )(?<arg3>"+wrd+@")\) =",
                     "        public static ${typ} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2}, ${argtyp3} ${arg3})\r\n" +
                     "        {\r\n" +
                     "            " + entArg1 + "${arg1},${arg2},${arg3}" + entArg2 + "${typ}" + entArg3 +
                     "            if (Facts.HasBeenAsserted(\"${fcn}\", ${arg1}, ${arg2}, ${arg3}))\r\n" +
                     "            {\r\n" +
                     "                return Facts.QueryTvar<${typ}>(\"${fcn}\", ${arg1}, ${arg2}, ${arg3});\r\n" +
                     "            }\r\n\r\n");

                // Two arguments
                line = Regex.Replace(line, 
                    @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing))In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\) =",
                     "        public static ${typ} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                     "        {\r\n" +
                     "            " + entArg1 + "${arg1},${arg2}" + entArg2 + "${typ}" + entArg3 +
                     "            if (Facts.HasBeenAsserted(\"${fcn}\", ${arg1}, ${arg2}))\r\n" +
                     "            {\r\n" +
                     "                return Facts.QueryTvar<${typ}>(\"${fcn}\", ${arg1}, ${arg2});\r\n" +
                     "            }\r\n\r\n");

                // One argument - FAIL
                line = Regex.Replace(line, 
                    @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing))In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")\) =",
                     "        public static ${typ} ${fcn}(${argtyp1} ${arg1})\r\n" +
                     "        {\r\n" +
                     "            " + entArg1 + "${arg1}" + entArg2 + "${typ}" + entArg3 +
                     "            if (Facts.HasBeenAsserted(\"${fcn}\", ${arg1}))\r\n" +
                     "            {\r\n" +
                     "                return Facts.QueryTvar<${typ}>(\"${fcn}\", ${arg1});\r\n" +
                     "            }\r\n\r\n");
            }

            return line;
        }

        /// <summary>
        /// Handles ordinary rules
        /// </summary>
        public static string CreateMainRule(string line)
        {
            // Three arguments
            line = Regex.Replace(line, 
                @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing)) (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@"), ?(?<argtyp3>"+wrd+@" )(?<arg3>"+wrd+@")\) =",
                 "        public static ${typ} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2}, ${argtyp3} ${arg3})\r\n" +
                 "        {\r\n" +
                 "            " + entArg1 + "${arg1},${arg2},${arg3}" + entArg2 + "${typ}" + entArg3);

            // Two arguments
            line = Regex.Replace(line, 
                @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing)) (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\) =",
                 "        public static ${typ} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                 "        {\r\n" +
                 "            " + entArg1 + "${arg1},${arg2}" + entArg2 + "${typ}" + entArg3);

            // One argument
            line = Regex.Replace(line, 
                @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Thing)) (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")\) =",
                 "        public static ${typ} ${fcn}(${argtyp1} ${arg1})\r\n" +
                 "        {\r\n" +
                 "            " + entArg1 + "${arg1}" + entArg2 + "${typ}" + entArg3);

            // Otherwise, no need to check the arguments for uncertainty...
            string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| ]+";
            line = Regex.Replace(line, @"(?<dec>(Tbool|Tnum|Tstr|Tdate|Tset|Thing|DateTime|bool)"+word+") =",
                                      "        public static ${dec}\r\n        {\r\n");  

            return line;
        }

        /// <summary>
        /// Determines whether a string is a valid number
        /// </summary>
        public static bool IsNumber(string s)
        {
            try
            {
                double.Parse(s);
                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}