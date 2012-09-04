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
    /// <summary>
    /// A class of utility functions for the Akkadian-C# compiler.
    /// </summary>
    public class Util
    {
        private static string[] RuleTypes = new string[8]{"Tbool","Tnum","Tdate","Tstr","Tset","DateTime","bool","Thing"};
        public static string EndRule = "        }\r\n\r\n";
        public static string wrd = @"[a-zA-Z0-9_]+";
     
        /// <summary>
        /// Creates closing parentheses for a nested Boolean expression.
        /// </summary>
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

        /// <summary>
        /// Creates a string of N blank spaces.
        /// </summary>
        public static string NSpaces(int count)
        {
            string result = "";
            for (int i=0; i<count; i++)
                result += " ";
            return result;
        }
     
        /// <summary>
        /// Determines line depth, based on the number of blank spaces.
        /// </summary>
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
     
        /// <summary>
        /// Determines whether a line is a blank line.
        /// </summary>
        public static bool IsBlank(string line)
        {
            return line.Trim() == "";
        }
     
        /// <summary>
        /// Determines whether a line is a comment line.
        /// </summary>
        public static bool IsComment(string line)
        {
            return line.TrimStart().StartsWith("#");
        }
     
        /// <summary>
        /// Determines whether a line is a line opening or closing a comment block.
        /// </summary>
        public static bool IsCommentBlockLine(string line)
        {
            return line.TrimStart().StartsWith("##");
        }
     
        /// <summary>
        /// Determines whether a line indicates the start of a rule.
        /// </summary>
        public static bool IsMainRule(string line)
        {
            return Depth(line) == 0 && IsDeclaration(line);
        }
     
        /// <summary>
        /// Determines whether a line declares a rule or subrule.
        /// </summary>
        public static bool IsDeclaration(string line)
        {
            return StartsWithAny(line.TrimStart(), RuleTypes) != "";
        }
  
        /// <summary>
        /// Determines whether a line requires the rule to first look for the
        /// fact in the factbase.
        /// </summary>
        public static bool IsInputRule(string line)
        {
            return Depth(line) == 0 &&
                   StartsWithAny(line, "TboolIn","TnumIn","TstrIn","TsetIn","TdateIn") != "";
        }

        /// <summary>
        /// Determines the type of rule.
        /// </summary>
        public static string ExtractRuleType(string line)
        {
            // remove whitespace
            line = line.TrimStart(' ');
            return StartsWithAny(line, RuleTypes);
        }
     
        /// <summary>
        /// Determines whether a string starts with any of a set of substrings.
        /// </summary>
        public static string StartsWithAny(string s, params string[] list)
        {
            foreach(string sub in list)
            {
                if (s.StartsWith(sub))
                    return sub;
            }
            return "";
        }
        
        /// <summary>
        /// Strips comment tags, declarations, whitespace, and a given substring.
        /// </summary>
        public static string Clean(string s, string sub)
        {
            s = s.TrimStart('#',' ');
            return s.Replace(sub, "").Trim();
        }

        /// <summary>
        /// Remove the comments from a line.
        /// </summary>
        public static string DeComment(string line)
        {
            if (line.Contains("#"))
            {
                return line.Substring(0,line.IndexOf("#") - 1);
            }
            return line;
        }
     
        /// <summary>
        /// Converts a yyyy-mm-dd date to Date(yyyy,mm,dd).
        /// </summary>
        public static string ConvertDate(string line)
        {
            return Regex.Replace(line, @"(?<year>[0-9]{4})-(?<mo>[0-9]{2})-(?<day>[0-9]{2})", "Date(${year},${mo},${day})");           
        }

        /// <summary>
        /// Puts the subrules in the order required by C#.
        /// </summary>
        /// <remarks>
        /// If the results of the rule need to be cached/memoized, this method implements it.
        /// </remarks>
        public static string ReorderSubrules(List<string> subrules, string ruleType, bool cache, string methodCacheLine)
        {
            if (subrules.Count == 0) return "";

            subrules.Reverse();
         
            string result = "";
            for (int i=0; i<subrules.Count; i++)
            {
                // Main rule
                if (i == subrules.Count-1) 
                {
                    if (cache) result += "            " + ruleType + " RESULT =\r\n";
                    else result += "            return\r\n";
                }
             
                // Subrules
                string s = subrules[i];
                s = s.TrimEnd();
                if (s.EndsWith("\r\n"))     // put semicolon at end of expression
                {
                    s = s.Substring(0,s.LastIndexOf("\r\n")-1);
                }
                result += s + ";\r\n\r\n";
            }

            // Assemble cache line
            if (cache)
            {
                result += methodCacheLine;
                result += "            return RESULT;\r\n";
            }

            return result;
        }
     
        /// <summary>
        /// Converts the part of a "match" rule to C# boolean expressions.
        /// </summary>
        /// <example>
        ///   ' match a, b '
        //    ' Alabama,   9 ' 
        //     -> 'a == "Alabama" && b == 9'
        /// </example>
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
     
        /// <summary>
        /// Converts the ending of a clause in a rule table to valid C#.
        /// </summary>
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