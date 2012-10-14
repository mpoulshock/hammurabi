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
using System.Text.RegularExpressions;

namespace Akkadian
{
    class MainClass
    {
        private static string mainRuleType = "";        // For main rules
        private static string currentRuleType = "";     // For main rules and subrules
        private static string methodCacheLine = "";     // Creates line that caches method results
        private static bool cacheRule = false;          // Should method results be cached?
        private static string tableMatchLine = "";      // Rule input that must be matched in a table
        public static int totalRuleCount = 0;           // Total rules in project
        public static string unitTestNameSpace = "";    // Namespace of unit tests
        public static string docNameSpace = "";         // For question metadata
        public static string akkDoc = "";               // For question metadata
     
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
                // Keep track of current rule document - for question metadata
                akkDoc = f;

                // Do the compilation
                Console.WriteLine(" * " + f.Replace(sourcePath,""));
                Compile(f, targetPath);
                docCount++;
            }

            // Generate metadata files
            Questions.GenerateMetadataFile(targetPath);
            Assumptions.GenerateAssumptionFile(targetPath);

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
        public static string CompilerOutput(string file)
        {
            // First pass: get the file metadata
            string result = Boilerplate.InitialBoilerplate(file);
         
            // Second pass: parse the rules, line by line
            StreamReader stream = new StreamReader(file);
            string line;
         
            // Mutable variables
            int ruleCount = 0;
            int lastLineDepth = 0;
            int parenCount = 0;
            bool isCommentBlock = false;
            bool isRulePart = false;
            string rulePart = "";
            Tests.unitTests = "";
            List<string> subrules = new List<string>();
            string previousLine = "";
         
            // Read the stream a line at a time
            while( (line = stream.ReadLine()) != null )
            {
                line = line.Replace("\t","    ");
                int depth = Util.Depth(line);

                // Handle commented lines, comment blocks, and mid-line comments
                if (!isCommentBlock && Util.IsCommentBlockLine(line)) { isCommentBlock = true; }
                else if (isCommentBlock && Util.IsCommentBlockLine(line)) { isCommentBlock = false; continue; }
                if (isCommentBlock || Util.IsComment(line)) 
                {
                    previousLine = line;    // Get previous line, to capture any question text
                    continue;
                }
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
                    if (ruleCount != 0)
                    {
                        result += Util.ReorderSubrules(subrules, mainRuleType, cacheRule, methodCacheLine); 
                        result += Util.EndRule;
                    }
                    subrules.Clear(); 

                    // Process current line
                    cacheRule = false;
                    result += Convert(line, previousLine);
                 
                    // Set flag variables
                    ruleCount++;
                    parenCount = 0;
                    currentRuleType = Util.ExtractRuleType(line);
                    mainRuleType = currentRuleType;
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
                    snippet += "        " + Convert(line, previousLine) + "\r\n";
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
            stream.Close();

            // Close the final method
            result += Util.ReorderSubrules(subrules, mainRuleType, cacheRule, methodCacheLine); 
            result += Util.EndRule;

            // Close the class/namespace, and add the unit tests
            result += Boilerplate.ClassAndNamespaceClose;
            result += Tests.WriteUnitTests(unitTestNameSpace);

            return result;
        }
     
        /// <summary>
        /// Applies transformation rules to the input line.
        /// </summary>
        private static string Convert(string line, string previousLine)
        {
            // Perform general syntactic replacements
            line = line.Replace("|~","^");
            line = line.Replace("&","&&").Replace("|","||");
            line = line.Replace("...","(");
            line = line.Replace("<>","!=");
             
            // Regex part  
            string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| ]+";
             
            // Stub()
            line = Regex.Replace(line, @"Stub\(\)", "new " + currentRuleType + "(Hstate.Stub)");    

            // Process question-related metadata and declared assumptions
            Questions.GatherMetadata(line, previousLine);
            line = Assumptions.Process(line);

            // Convert rules and dates
            line = ConvertRegularRules(line);
            line = ConvertRuleTables(line, currentRuleType, tableMatchLine, word);

            // Facts.QueryTvar<Tvar>()
            line = TransformMethod.QueryTvarTransform(line);
    
            // IfThen() 
            line = Regex.Replace(line, @"if (?<txt>"+word+@") then (?<txt2>[-!\+\*/A-Za-z0-9\.;\(\),""'_<>= ]+)", "IfThen(${txt}, ${txt2})");  

            // Exists/ForAll/Filter
            line = Regex.Replace(line, @"\.(?<quant>(Exists|ForAll|Filter|Sum|Min|Max))\((?<fcn>[a-zA-Z0-9\(\)\._,\! ]+)\)", ".${quant}( _ => ${fcn})");  
                
            return line;
        }

        /// <summary>
        /// Converts "standard" rules from .akk to .cs.
        /// </summary>
        /// <remarks>
        /// This method must come before the conversion to Facts.QueryTvar<Tvar>().
        /// </remarks>
        private static string ConvertRegularRules(string line)
        {
            // First, look for rules that require intermediate assertion checks
            if (Util.IsInputRule(line) && line.TrimEnd().EndsWith("="))
            {
                // Note: These set class-level variables!
                cacheRule = true;
                methodCacheLine = TransformMethod.MethodCacheLine(line); 

                // Do the conversion
                line = TransformMethod.CreateIntermediateAssertion(line);  
            }
            // Else, process all other rules
            else if (Util.IsMainRule(line))
            {
                line = TransformMethod.CreateMainRule(line);
            }

            return line;
        }

        /// <summary>
        /// Converts various types of rule tables (and dates) from .akk to .cs.
        /// </summary>
        private static string ConvertRuleTables(string line, string ruleType, string matchLine, string word)
        {
            // Switch(condition, value, ..., default) - must come before "rule tables" (b/c rule tables look for "->"
            line = Regex.Replace(line, @"set:", "Switch<" + ruleType + ">(");    
            line = Regex.Replace(line, @"if (?<condition>"+word+") -> (?<value>"+word+")", "()=> ${condition}, ()=> ${value},");    
            line = Regex.Replace(line, @"else (?<default>"+word+")", "()=> ${default})");  
             
            // Temporal tables - must come before "rule tables"
            line = Regex.Replace(line.Replace("\t","    "), @"temporal:", ruleType + ".Make" + ruleType + "(");  
            line = Regex.Replace(line, @"from (?<condition>"+word+") -> (?<value>"+word+")", "${condition}, ${value},");  
            line = Regex.Replace(line, "endtemporal", "new " + ruleType + "(Hstate.Stub))");    // Value here is filler; needed due to extra comma in previous line

            // Rule tables - must come before "dates"
            line = Regex.Replace(line, @"match "+word, "Switch<" + ruleType + ">(");    
            if (line.Contains("->")) line = Util.RuleTableMatch(matchLine, line);

            // yyyy-mm-dd -> Date(yyyy,mm,dd)
            line = Util.ConvertDate(line); 

            return line;
        }
    }       
}