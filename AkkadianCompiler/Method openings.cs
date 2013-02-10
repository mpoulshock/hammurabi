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
        public static string QueryTvarTransform(string line, string space)
        {
            // Rule conclusion line - no conditions in rule
            if (line.StartsWith("T"))
            {
                space = Util.NamespaceConvert(space);

                if (line.StartsWith("TboolSym"))    // this check included for performance reasons
                {
                    // TboolInSym (always has two arguments)
                    line = Regex.Replace(line, 
                                     @"TboolSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                                     "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                                     "        {\r\n" +
                                     "            return Facts.Sym(${arg1}, \""+space+"${fcn}\", ${arg2});");  
                }
                else
                {
                    // Functions with 1-3 arguments
                    line = Regex.Replace(line, 
                                     @"(?<type>"+typs+@") (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\)",
                                     "        public static ${type} ${fcn}(${argtyp1} ${arg1}${comma1}${argtyp2} ${arg2}${comma2}${argtyp3} ${arg3})\r\n        {\r\n" +
                                     "        return Facts.QueryTvar<${type}>(\""+space+"${fcn}\", ${arg1}${comma1}${arg2}${comma2}${arg3});");  
                }
            }

            return line;
        }

        /// <summary>
        /// Generates the c# line that caches the result of the method (comes at the end of the method).
        /// </summary>
        public static string MethodCacheLine(string line, string space)
        {
            space = Util.NamespaceConvert(space).Replace("..","."); // .Replace 

            return Regex.Replace(line, 
                                 @"(?<typ>"+typs+@")(?<sym>Sym)?(?<quest>\?)? (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\) =",
                                 "            if (!RESULT.IsEverUnstated) Facts.Assert" +
                                 "(${arg1}, \""+space+"${fcn}\"${comma1}${arg2}${comma2}${arg3}, RESULT);\r\n");
        }

        /// <summary>
        /// Handles ordinary rules
        /// </summary>
        public static string CreateMainRule(string line, string space)
        {
            space = Util.NamespaceConvert(space);
        
            // Functions with 1-3 arguments
            string lineafter = Regex.Replace(line, 
                                 @"(?<typ>"+typs+@")(?<sym>Sym)?(?<quest>\?)? (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\) =",
                                 "        public static ${typ} ${fcn}(${argtyp1} ${arg1}${comma1} ${argtyp2} ${arg2}${comma2} ${argtyp3} ${arg3})\r\n" +
                                 "        {\r\n" +
                                 "            RulePreCheckResponse RESPONSE = ShortCircuitValue<${typ}>(\""+space+"${fcn}\",\"${sym}\",\"${quest}\",${arg1}${comma1} ${arg2}${comma2} ${arg3});\r\n" +
                                 "            if (RESPONSE.shouldShortCircuit) return (${typ})RESPONSE.val;\r\n\r\n");

            // Add code that caches the results of the method, and the function opening
            if (line != lineafter)
            {
                MainClass.cacheRule = true;
                MainClass.methodCacheLine = TransformMethod.MethodCacheLine(line, space);
                line = lineafter;
            }

            // Otherwise, no need to check the arguments for uncertainty...
            string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| ]+";
            line = Regex.Replace(line, @"(?<dec>(Tbool|Tnum|Tstr|Tdate|Tset|Thing|DateTime|bool)"+word+") =",
                                      "        public static ${dec}\r\n        {\r\n");  

            return line;
        }
    }
}