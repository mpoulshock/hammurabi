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
    public class TransformMethod
    {
        // Regex parts
        public static string wrd = @"[a-zA-Z0-9_]+";
        public static string typs = @"(Tbool|Tnum|Tstr|Tset|Tdate)";

        /// <summary>
        /// Handles fact inputs
        /// </summary>
        public static string QueryTvarTransform(string line)
        {
            // Rule conclusion line - no conditions in rule
            if (Util.IsInputRule(line))  // Starts w/ TvarIn at indent 0...
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, 
                          @"TboolInSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                          "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n        {\r\n            return Facts.Sym(${arg1}, \"${fcn}\", ${arg2});");  
    
                // Functions with 1-3 arguments
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\)",
                          "        public static ${type} ${fcn}(${argtyp1} ${arg1}${comma1}${argtyp2} ${arg2}${comma2}${argtyp3} ${arg3})\r\n        {\r\n" +
                          "        return Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}${comma1}${arg2}${comma2}${arg3});");  
            }

            // Is rule condition line, not rule conclusion
            else                
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, @"TboolInSym (?<fcn>"+wrd+@")\((?<arg1>"+wrd+@"),(?<arg2>"+wrd+@")\)", 
                                     "Facts.Sym(${arg1}, \"${fcn}\", ${arg2})");  

                // Functions with 1-3 arguments
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg1>"+wrd+@")(?<comma1>, ?)?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<arg3>"+wrd+@")?\)", 
                                     "Facts.QueryTvar<${type}>(\"${fcn}\", ${arg1}${comma1}${arg2}${comma2}${arg3})"); 
            }
            
            return line;
        }
        
        /// <summary>
        /// Handles intermediate facts (facts asserted mid-tree)
        /// </summary>
        public static string CreateIntermediateAssertion(string line)
        {
            line = Regex.Replace(line, 
                @"(?<typ>"+typs+@")In(?<sym>Sym)?(?<quest>\?)? (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\) =",
                "        public static ${typ} ${fcn}(${argtyp1} ${arg1}${comma1} ${argtyp2} ${arg2}${comma2} ${argtyp3} ${arg3})\r\n" +
                "        {\r\n" +
                "            RulePreCheckResponse r = ShortCircuitValue<${typ}>(\"${fcn}\",\"${sym}\",\"${quest}\",${arg1}${comma1} ${arg2}${comma2} ${arg3});\r\n" +
                "            if (r.shouldShortCircuit) return (${typ})r.val;\r\n\r\n");

            return line;
        }

        /// <summary>
        /// Handles ordinary rules
        /// </summary>
        public static string CreateMainRule(string line)
        {
            // Functions with 1-3 arguments
            line = Regex.Replace(line, 
                @"(?<typ>"+typs+@") (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\) =",
                 "        public static ${typ} ${fcn}(${argtyp1} ${arg1}${comma1} ${argtyp2} ${arg2}${comma2} ${argtyp3} ${arg3})\r\n" +
                 "        {\r\n" +
                 "            Hstate h = EntityArgIsUnknown(${arg1}${comma1}${arg2}${comma2}${arg3});\r\n" +
                 "            if (h != Hstate.Known) return new ${typ}(h);\r\n\r\n");

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