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
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Akkadian
{
    class MainClass
    {
        private static string currentRuleType = "";
        private static string tableMatchLine = "";
        private static int totalRuleCount = 0;
        public static string unitTestNameSpace = "";
     
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        public static void Main (string[] args)
        {
            // Gets the file path of the high-level Hammurabi folder
            string currentDir = Environment.CurrentDirectory;
            string projectPath = currentDir.Replace(@"AkkadianCompiler\bin\Debug","");

            // Top-level folder where the .akk files live
            string sourcePath = projectPath + @"Akkadian";

            // Create the older where the generated C# files should be put
            string targetPath = projectPath + @"Hammurabi\Generated source\";
            System.IO.Directory.CreateDirectory(targetPath);

            // Go
            CompileAll(sourcePath, targetPath);
//            Console.ReadLine();
        }
     
        /// <summary>
        /// Compiles all .akk files in a given folder and puts compiled version in 
        /// a parallel folder structure elsewhere.
        /// </summary>
        public static void CompileAll(string sourcePath, string targetPath)
        {
            // Start timer 
            TimeSpan duration = new TimeSpan();
            DateTime startTime = DateTime.Now;
         
            // Delete C# files generated on a prior run (to avoid cruft)
            Console.WriteLine("Deleting obsolete C# files...\n");
            System.IO.DirectoryInfo generatedFileInfo = new DirectoryInfo(targetPath);
            foreach (FileInfo file in generatedFileInfo.GetFiles())
            {
                file.Delete(); 
            }

            // Compile all files in .akk directory
            Console.WriteLine("Compiling .akk source files...\n");
            int docCount = 0;
            foreach(string f in Directory.GetFiles(sourcePath, "*.akk", SearchOption.AllDirectories))
            {
                Console.WriteLine(" * " + f.Replace(sourcePath,""));
                Compile(f, targetPath);
                docCount++;
            }
            
            // Display elapsed time
            duration = DateTime.Now - startTime;
            
            // Display compilation stats
            Console.WriteLine("\nStats...\n");
            Console.WriteLine(" * Time:  " + duration);
            Console.WriteLine(" * Files: " + docCount);
            Console.WriteLine(" * Rules: " + totalRuleCount);
        }
     
        /// <summary>
        /// Compiles the specified .akk document.
        /// </summary>
        public static void Compile(string InputFileName, string targetPath)
        {
            // Save the .cs file in a "generated source" directory in the Hammurabi project
            string docName = InputFileName.Substring(InputFileName.LastIndexOf("\\")).Replace(".akk", ".cs");
            string OutputFileName = targetPath + docName;

            // Compile the document
            string snippet = CompilerOutput(InputFileName);
         
            // Write the string to a file
            System.IO.StreamWriter file = new System.IO.StreamWriter(OutputFileName);
            file.WriteLine(snippet);
            file.Close();
        }
     
        /// <summary>
        /// Iterates through the .akk file line by line and converts it to C#.
        /// </summary>
        public static string CompilerOutput(string url)
        {
            // First pass: get the file metadata
            string result = Boilerplate.InitialBoilerplate(url);
         
            // Second pass: parse the rules, line by line
            WebRequest req = WebRequest.Create(url);
            StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
            string line;
         
            int ruleCount = 0;
            int lastLineDepth = 0;
            int parenCount = 0;
            bool isCommentBlock = false;
            bool isRulePart = false;
            string rulePart = "";
            Tests.unitTests = "";
            List<string> subrules = new List<string>();
         
            // Read the stream a line at a time and place each one into the stringbuilder
            while( (line = stream.ReadLine()) != null )
            {
                line = line.Replace("\t","    ");
                int depth = Util.Depth(line);

                // Handle commented lines, comment blocks, and mid-line comments
                if (!isCommentBlock && Util.IsCommentBlockLine(line)) { isCommentBlock = true; }
                else if (isCommentBlock && Util.IsCommentBlockLine(line)) { isCommentBlock = false; continue; }
                if (isCommentBlock || Util.IsComment(line)) continue;
                line = Util.DeComment(line);
             
                // Create unit test from .akk
                if (Tests.IsTestLine(line)) 
                {
                    Tests.ProcessTestLine(line);
                    continue;
                }
                
                // Process the line
                if (Util.IsMainRule(line))          // Begin a new rule
                {
                    // Close previous rule
                    if (ruleCount != 0)  //
                    {
                        result += Util.ReorderSubrules(subrules); 
                        result += Util.EndRule;
                    }
                    subrules.Clear(); 

                    // Process current line
                    result += Convert(line);
                 
                    // Set flag variables
                    ruleCount++;
                    parenCount = 0;
                    currentRuleType = Util.ExtractRuleType(line);
                    totalRuleCount++;
                }
                else                     // Add ordinary rule conditions
                {
                    string snippet = "";
                 
                    if (depth > 0) isRulePart = true;
                 
                    // Identify return type of subrule
                    if (depth == 1 && Util.IsDeclaration(line))
                    {
                        currentRuleType = Util.ExtractRuleType(line);
                    }
                    
                    // Deal with parentheses
                    if (line.Trim() == "...") parenCount++;
                    int closingParensNeeded = Math.Min(lastLineDepth - depth, parenCount);
                    if (parenCount > 0 && closingParensNeeded > 0)
                    {
                        parenCount = parenCount - closingParensNeeded;
                        snippet += Util.ClosingParens(closingParensNeeded, lastLineDepth);
                    }
                 
                    // Handle rule tables
                    if (line.Trim().StartsWith("match")) tableMatchLine = line;
                 
                    // Convert the line items (to main rule or subrule)
                    snippet += "        " + Convert(line) + "\r\n";
                    if (isRulePart) rulePart += snippet;
                    else result += snippet;
                }
             
                if (Util.IsBlank(line))             // Blank lines
                {
                    // Reset subrule flag and add subrule to subrule list
                    isRulePart = false;
                    if (rulePart != "")
                        subrules.Add(rulePart);
                    rulePart = "";
                }
             
                lastLineDepth = depth;
            }
         
            // Close the final method
            result += Util.ReorderSubrules(subrules); 
            result += Util.EndRule;
         
            // Close stream and convert to string
            stream.Close();

            result += Boilerplate.ClassAndNamespaceClose;
            
            // Add unit test code
            if (Tests.unitTests.Trim() != "")
            {
                result += Tests.unitSpaceOpen2(unitTestNameSpace) +
                          Tests.unitTests +
                          Tests.unitSpaceClose;
            }
            
            return result;
        }
     
        /// <summary>
        /// Applies transformation rules to the input line.
        /// </summary>
        private static string Convert(string line)
        {
            // Perform general syntactic replacements
            line = line.Replace("|~","^");
            line = line.Replace("&","&&").Replace("|","||");
            line = line.Replace("...","(");
            line = line.Replace("<>","!=");
             
            // Convert specific syntactic elements from Akk to C#   
            string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| ]+";
             
            // Stub()
            line = Regex.Replace(line, @"Stub\(\)", "new " + currentRuleType + "(Hstate.Stub)");    

            // Start new rule / C# method (must come before TvarIn)
            // First, look for rules that require intermediate assertion checks
            if (Util.IsInputRule(line) && line.TrimEnd().EndsWith("="))
                line = Util.CreateIntermediateAssertion(line);  
            // Else, all other rules
            else if (Util.IsMainRule(line))                                   
                line = Regex.Replace(line, @"(?<dec>(Tbool|Tnum|Tstr|Tdate|Tset|Person|Entity|DateTime|bool)"+word+") =",
                                      "        public static ${dec}\r\n        {\r\n"); 
    
            // Switch(condition, value, ..., default) - must come before "rule tables" (b/c rule tables look for "->"
            line = Regex.Replace(line, @"set:", "Switch<" + currentRuleType + ">(", RegexOptions.IgnoreCase);    
            line = Regex.Replace(line, @"if (?<condition>"+word+") -> (?<value>"+word+")", "()=> ${condition}, ()=> ${value},", RegexOptions.IgnoreCase);    
            line = Regex.Replace(line, @"else (?<default>"+word+")", "()=> ${default})", RegexOptions.IgnoreCase);  
             
            // Temporal tables - must come before "rule tables"
            line = Regex.Replace(line.Replace("\t","    "), @"temporal:", currentRuleType + ".Make" + currentRuleType + "(");  
            line = Regex.Replace(line, @"from (?<condition>"+word+") -> (?<value>"+word+")", "${condition}, ${value},", RegexOptions.IgnoreCase);   
            line = Regex.Replace(line, "endtemporal", "null)");    
    
            // Rule tables - must come before "dates"
            line = Regex.Replace(line, @"match "+word, "Switch<" + currentRuleType + ">(");    
            if (line.Contains("->")) line = Util.RuleTableMatch(tableMatchLine, line);
             
            // yyyy-mm-dd -> Date(yyyy,mm,dd)
            line = Util.ConvertDate(line); 
            
            // Input.Tvar()
            line = Util.TvarInTransform(line);
    
            // IfThen() 
            line = Regex.Replace(line, @"if (?<txt>"+word+@") then (?<txt2>[-!\+\*/A-Za-z0-9\.;\(\),""'_<>= ]+)", "IfThen(${txt}, ${txt2})", RegexOptions.IgnoreCase);  
             
            // Facts.AllKnownPeople, Property, etc.
            line = Regex.Replace(line, @"AllPeopleExcept\(", "Facts.EveryoneExcept(", RegexOptions.IgnoreCase);  
            line = Regex.Replace(line, @"AllPeople", "Facts.AllKnownPeople()", RegexOptions.IgnoreCase);  
            line = Regex.Replace(line, @"AllProperty", "Facts.AllKnownProperty()", RegexOptions.IgnoreCase);  
            
            // Exists/ForAll/Filter
            line = Regex.Replace(line, @"\.(?<quant>(Exists|ForAll|Filter|Sum|Min|Max))\((?<fcn>[a-z0-9\(\)\._, ]+)\)", ".${quant}( _ => ${fcn})", RegexOptions.IgnoreCase);  
                
            return line;
        }
    }       
}