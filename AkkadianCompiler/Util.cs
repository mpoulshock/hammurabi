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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Akkadian
{
    public class Util
    {
        private static string[] RuleTypes = new string[10]{"mod ","Tbool","Tnum","Tdate","Tstr","Tset","DateTime","Person","Entity","bool"};
        private static string[] DocHeaders = new string[5]{"Citation:","Namespace:","Summary:","Remarks:","References:"};    
        public static string EndRule = "        }\r\n\r\n";
        public static string wrd = @"[a-zA-Z0-9]+";
     
        public static string ClosingParens(int closingParensNeeded, int lastLineDepth)
        {
            string result = "";
            int lineDepth = lastLineDepth - closingParensNeeded;
            if (closingParensNeeded > 0)
            {
                for (int i=lastLineDepth; i>lineDepth; i--)
                {
                   result += NSpaces(4*(i+1)) + ")\r\n";
                }
            }
            return result;
        }

        public static string NSpaces(int count)
        {
            string result = "";
            for (int i=0; i<count; i++)
                result += " ";
            return result;
        }
     
        public static int Depth(string line)
        {
            if (IsBlank(line)) return 0;
            else if (line.StartsWith("                            ")) return 7;
            else if (line.StartsWith("                        ")) return 6;
            else if (line.StartsWith("                    ")) return 5;
            else if (line.StartsWith("                ")) return 4;
            else if (line.StartsWith("            ")) return 3;
            else if (line.StartsWith("        ")) return 2;
            else if (line.StartsWith("    ")) return 1;
            else return 0;
        }
     
        public static bool IsBlank(string line)
        {
            line = line.Trim();
            if (line == "" || line == null)
                return true;
            return false;
        }
     
        public static bool IsComment(string line)
        {
            line = line.TrimStart();
            if (line.StartsWith("#"))
                return true;
            return false;
        }
     
        public static bool IsCommentBlockLine(string line)
        {
            line = line.TrimStart();
            if (line.StartsWith("##"))
                return true;
            return false;
        }
     
        public static bool IsMainRule(string line)
        {
            if (Depth(line) == 0 && IsDeclaration(line))
                return true;
            return false;
        }
     
        public static bool IsDeclaration(string line)
        {
            line = line.TrimStart();
            string swa = StartsWithAny(line, RuleTypes);
         
            if (swa != "")
                return true;
            return false;
        }
  
        public static bool IsInputRule(string line)
        {
            return Depth(line) == 0 &&
                   StartsWithAny(line, "TboolIn","TnumIn","TstrIn","TsetIn","TdateIn","DateIn","PersonIn") != "";
        }
        
        public static string ExtractRuleType(string line)
        {
            // remove whitespace
            line = line.TrimStart(' ');
            if (line.StartsWith("mod "))
                line = line.Substring(4, line.Length-4);
            return StartsWithAny(line, RuleTypes);
        }
     
        public static string StartsWithAny(string s, params string[] list)
        {
            foreach(string sub in list)
            {
                if (s.StartsWith(sub))
                    return sub;
            }
            return "";
        }
        
        // Strip comment tag, declarations, whitespace, and a substring
        public static string Clean(string s, string sub)
        {
            s = s.TrimStart('#',' ');
            s = s.Replace(sub, "");
            return s.Trim();
        }

        public static string DeComment(string line)
        {
            if (line.Contains("#"))
            {
                return line.Substring(0,line.IndexOf("#")-1);
            }
            return line;
        }
     
        public static string ConvertDate(string line)
        {
            return Regex.Replace(line, @"(?<year>[0-9]{4})-(?<mo>[0-9]{2})-(?<day>[0-9]{2})", "Date(${year},${mo},${day})", RegexOptions.IgnoreCase);           
        }
        
        public static string ReorderSubrules(List<string> subrules)
        {
            subrules.Reverse();
         
            string result = "";
            for (int i=0; i<subrules.Count; i++)
            {
                if (i == subrules.Count-1) result += "            return\r\n";
             
                string s = subrules[i];
                s = s.TrimEnd();
                if (s.EndsWith("\r\n"))     // put semicolon at end of expression
                {
                    s = s.Substring(0,s.LastIndexOf("\r\n")-1);
                }
                result += s + ";\r\n\r\n";
            }
            return result;
        }
     
        // ' match a, b '
        // ' Alabama,   9 ' 
        // -> 'a == "Alabama" && b == 9'
        public static string RuleTableMatch(string template, string line)
        {
            string result = "    ()=> ";
         
            template = template.Replace("match","");
            int arrow = line.IndexOf("->");
            string cond = line.Substring(0, arrow);
            string rest = line.Substring(arrow+2, line.Length-arrow-2);
         
            string[] headings = template.Split(',');
            string[] values = cond.Split(',');
            int count = 0;
         
            for (int i=0; i<headings.Length; i++)
            {
                string val = values[i].ToString().Trim();
             
                if (val != "")  // Condition is not relevant
                {
                    if (count > 0) result += " && ";
                    string variable = headings[i].ToString().Trim();
                    result +=  variable + " " + RuleTableClauseEnd(val);
                    count++;
                }
            }
            
            result += ", ()=> " + rest + ",";
            return result;
        }
     
        private static string RuleTableClauseEnd(string val)
        {
            // If comparison...
            if (val.StartsWith(">=")) return val;
            else if (val.StartsWith(">")) return val;
            else if (val.StartsWith("<=")) return val;
            else if (val.StartsWith("<")) return val;
         
            // If boolean
            else if (val.ToLower() == "true" || val.ToLower() == "false") return " == " + val;
         
            // If date...
            else if (Regex.Match(val, @"[0-9]{4}-[0-9]{2}-[0-9]{2}").Success)
                return " == " + val;
         
            // If number...
            else if (Regex.Match(val, @"[0-9]([0-9\.]+)?").Success)
                return " == " + val;
         
            // Else assumed to be string literal
            else return " == \"" + val + "\"";
        }
     
        public static string TvarInTransform(string line)
        {
            string typs = @"(Tbool|Tnum|Tstr|Tset|Tdate|Date|Person)";
            
            // Input.Tvar()
            if (Util.IsInputRule(line))  // Starts w/ TvarIn at indent 0...
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, 
                          @"TboolInSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                          "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n        {\r\n            return Facts.Sym(${arg1}, \"${fcn}\", ${arg2});");  
    
                // Functions with two arguments
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\)",
                          "        public static ${type} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n        {\r\n            return Input.${type}(${arg1}, \"${fcn}\", ${arg2});");  

                // Functions with one argument
                line = Regex.Replace(line, 
                          @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<argtyp>"+wrd+@" )(?<arg>"+wrd+@")\)",
                          "        public static ${type} ${fcn}(${argtyp} ${arg})\r\n        {\r\n            return Input.${type}(${arg}, \"${fcn}\");");  
            }
            else                // Is rule condition line, not rule conclusion
            {
                // TboolInSym (always has two arguments)
                line = Regex.Replace(line, @"TboolInSym (?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+)\)", 
                                     "Facts.Sym(${arg1}, \"${fcn}\", ${arg2})");  

                // Functions with two arguments
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg1>[a-zA-Z0-9 ]+),(?<arg2>[a-zA-Z0-9 ]+)\)", "Input.${type}(${arg1}, \"${fcn}\", ${arg2})", RegexOptions.IgnoreCase);  

                // Functions with one argument
                line = Regex.Replace(line, @"(?<type>"+typs+@")In (?<fcn>"+wrd+@")\((?<arg>"+wrd+@")\)", "Input.${type}(${arg}, \"${fcn}\")", RegexOptions.IgnoreCase);  
            }
            
            return line;
        }
        
        /// <summary>
        /// Handles intermediate facts (facts asserted mid-tree)
        /// </summary>
        public static string CreateIntermediateAssertion(string line)
        {
            // Symmetrical facts, e.g. TboolInSym Fcn(Person p1, Person p2) =
            if (line.StartsWith("TboolInSym"))
            {
                line = Regex.Replace(line, 
                    @"TboolInSym (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\) =",
                     "        public static Tbool ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                     "        {\r\n" +
                     "            if (Facts.HasBeenAssertedSym(${arg1}, \"${fcn}\", ${arg2}))\r\n" +
                     "            {\r\n" +
                     "                return Input.Sym(${arg1}, \"${fcn}\", ${arg2});\r\n" +
                     "            }\r\n\r\n");
            }
            else
            {
                // Two arguments
                line = Regex.Replace(line, 
                    @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Person))In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@"), ?(?<argtyp2>"+wrd+@" )(?<arg2>"+wrd+@")\) =",
                     "        public static ${typ} ${fcn}(${argtyp1} ${arg1}, ${argtyp2} ${arg2})\r\n" +
                     "        {\r\n" +
                     "            if (Facts.HasBeenAsserted(${arg1}, \"${fcn}\", ${arg2}))\r\n" +
                     "            {\r\n" +
                     "                return Input.${typ}(${arg1}, \"${fcn}\", ${arg2});\r\n" +
                     "            }\r\n\r\n");

                // One argument
                line = Regex.Replace(line, 
                    @"(?<typ>(Tbool|Tnum|Tstr|Tdate|Tset|Person))In (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")\) =",
                     "        public static ${typ} ${fcn}(${argtyp1} ${arg1})\r\n" +
                     "        {\r\n" +
                     "            if (Facts.HasBeenAsserted(${arg1}, \"${fcn}\"))\r\n" +
                     "            {\r\n" +
                     "                return Input.${typ}(${arg1}, \"${fcn}\");\r\n" +
                     "            }\r\n\r\n");
            }

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
