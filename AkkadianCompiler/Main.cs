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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Build.Evaluation;

namespace AkkadianCompiler
{
    class MainClass
    {
        // Global counters
        public static int SlocCount = 0;                // Total Akkadian lines in project
        public static int totalRuleCount = 0;           // Total rules in project
        public static int totalInputRuleCount = 0;      // Total input functions in project

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        public static void Main(string[] args)
        {
            // Gets the file path of the high-level Hammurabi folder
            string currentDir = Environment.CurrentDirectory;
            string projectPath = currentDir.Replace(@"AkkadianCompiler\bin\Debug", "");

            // Top-level folder where the .akk files live
            string sourcePath = projectPath + @"Hammurabi";

            // Create the older where the generated C# files should be put
            string targetPath = projectPath + @"Akkadian\Generated source\";
            System.IO.Directory.CreateDirectory(targetPath);

            // Go
            CompileAll(sourcePath, targetPath);

            // Build Hammurabi.dll
            Console.WriteLine("\nRebuilding Something.dll...");
            var projectCollection = ProjectCollection.GlobalProjectCollection;
            var project = projectCollection.LoadProject(projectPath + @"\Akkadian\Akkadian.csproj");
            if (project.Build())
            {
                Console.WriteLine("Build succeeded.\n");
            }
            else
            {
                // TODO: Show reasons for failure in the console.
                Console.WriteLine("Build failed.\n");
            }
        }
     
        /// <summary>
        /// Compiles all .akk files in a given folder and puts compiled version in 
        /// a parallel folder structure elsewhere.
        /// </summary>
        public static void CompileAll(string sourcePath, string targetPath)
        {
            // Start timer 
            DateTime startTime = DateTime.Now;
         
            // Delete C# files generated on a prior run (to avoid cruft)
            Console.WriteLine("Deleting obsolete C# files...\n");
            System.IO.DirectoryInfo generatedFileInfo = new DirectoryInfo(targetPath);
            foreach (FileInfo file in generatedFileInfo.GetFiles())
            {
                file.Delete(); 
            }

            // Compile all files in .akk directory (in parallel)
            Console.WriteLine("Compiling .akk source files...\n");
            string[] akkFiles = Directory.GetFiles(sourcePath, "*.akk", SearchOption.AllDirectories);
            Parallel.ForEach(akkFiles, f => 
            {
                Compile(f, targetPath);
            } );

            // Generate metadata files
            // The next two steps consume 10% of the compilation time...
            Questions.GenerateMetadataFile(targetPath);
            Assumptions.GenerateAssumptionFile(targetPath);

            // Determine elapsed time
            TimeSpan duration = DateTime.Now - startTime;
            
            // Display compilation stats
            Console.WriteLine("\nStats...\n");
            Console.WriteLine(" * Time:   " + Math.Round(duration.TotalSeconds, 3) + "s");
            Console.WriteLine(" * Files:  " + akkFiles.Length);
            Console.WriteLine(" * Rules:  " + (totalRuleCount - totalInputRuleCount));
            Console.WriteLine(" * Inputs: " + totalInputRuleCount);
            Console.WriteLine(" * SLOC:   " + SlocCount);
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
            Console.WriteLine(" * " + docName.TrimStart('\\'));
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
            // Mutable variables
            int ruleCount = 0;
            int lastLineDepth = 0;
            int parenCount = 0;
            bool isCommentBlock = false;
            bool isRulePart = false;
            string rulePart = "";
            List<string> subrules = new List<string>();
            string previousLine = "";       // Keeps track of the previous .akk line
            string tableMatchLine = "";     // Rule input that must be matched in a table
            string currentRuleType = "";    // For main rules and subrule
            string mainRuleType = "";       // For main rules
            string methodCacheLine = "";    // Creates line that caches method results
            bool cacheRule = false;         // Should method results be cached?
            string unitTests = "";          // Accumulates lines as they are processed

            // First pass: get the document namespace
            string docNameSpace = Boilerplate.GetDocNameSpace(file);
            string unitTestNameSpace = docNameSpace.Replace(".","");

            // Second pass: get the file metadata
            string result = Boilerplate.InitialBoilerplate(file, docNameSpace);
           
            // Third pass: parse the rules, line by line
            StreamReader stream = new StreamReader(file);
            string line;
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
                    unitTests += Tests.ProcessTestLine(line);
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
                    result += Convert(line, previousLine, file, tableMatchLine, currentRuleType, docNameSpace);
                 
                    // Detect whether this is a function that needs to be cached
                    const string wrd = TransformMethod.wrd;
                    const string typs = TransformMethod.typs;
                    if (Regex.Match(line, @"(?<typ>"+typs+@")(?<sym>Sym)?(?<quest>\?)? (?<fcn>"+wrd+@")\((?<argtyp1>"+wrd+@" )(?<arg1>"+wrd+@")(?<comma1>, ?)?(?<argtyp2>"+wrd+@" )?(?<arg2>"+wrd+@")?(?<comma2>, ?)?(?<argtyp3>"+wrd+@" )?(?<arg3>"+wrd+@")?\) =").Success)
                    {
                        cacheRule = true;
                        methodCacheLine = TransformMethod.MethodCacheLine(line, docNameSpace);
                    }

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
                    snippet += "        " + Convert(line, previousLine, file, tableMatchLine, currentRuleType, docNameSpace) + "\r\n";
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
                else
                {
                    SlocCount++;    // Doesn't count test case lines (by design)
                }
             
                lastLineDepth = depth;
            }
            stream.Close();

            // Close the final method
            result += Util.ReorderSubrules(subrules, mainRuleType, cacheRule, methodCacheLine); 
            result += Util.EndRule;

            // Close the class/namespace, and add the unit tests
            result += Boilerplate.ClassAndNamespaceClose;
            result += Tests.WriteUnitTests(unitTests, unitTestNameSpace);

            return result;
        }
     
        /// <summary>
        /// Applies transformation rules to the input line.
        /// </summary>
        private static string Convert(string line, string previousLine, string fileName, string tableMatchLine, string currentRuleType, string docNameSpace)
        {
            // Perform general syntactic replacements
            line = line.Replace("|~", "^");
            line = line.Replace("&", "&&").Replace("|", "||");
            line = line.Replace("...", "(");
            line = line.Replace("<>", "!=");

            // Currency values
            line = Util.RemoveCurrencyStyling(line);

            // Stub()
            line = Regex.Replace(line, @"Stub\(\)", "new " + currentRuleType + "(Hstate.Stub)");    

            // Process question-related metadata and declared assumptions
            Questions.GatherMetadata(line, previousLine, fileName, docNameSpace);
            line = Assumptions.Process(line, docNameSpace);
            
            // Regex part  
            const string word = @"[-!\+\*/A-Za-z0-9\.;\(\),""'_<>=&| %]+";

            // Convert rules and dates
            line = ConvertRegularRules(line, docNameSpace);
            line = ConvertRuleTables(line, currentRuleType, tableMatchLine, word);

            // Facts.QueryTvar<Tvar>()
//            line = TransformMethod.QueryTvarTransform(line, docNameSpace, previousLine);  // can be used to generate c# custom attributes
            line = TransformMethod.QueryTvarTransform(line, docNameSpace);
    
            // IfThen() 
            line = Regex.Replace(line, @"if (?<txt>"+word+@") then (?<txt2>[-!\+\*/A-Za-z0-9\.;\(\),""'_<>= ]+)", "IfThen(${txt}, ${txt2})");  

            // Higher-order set functions
            line = Regex.Replace(line, @"\.(?<quant>(Exists|ForAll|Filter|Sum|Min|Max|OptimalSubset))\((?<fcn>[a-zA-Z0-9\(\)\._,\! ]+)\)", ".${quant}( _ => ${fcn})");  
                
            return line;
        }

        /// <summary>
        /// Converts "standard" rules from .akk to .cs.
        /// </summary>
        /// <remarks>
        /// This method must come before the conversion to Facts.QueryTvar<Tvar>().
        /// </remarks>
        private static string ConvertRegularRules(string line, string space)
        {
            if (Util.Depth(line) == 0)
            {
                line = TransformMethod.CreateMainRule(line, space);
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
            line = Regex.Replace(line, @"from (?<condition>"+word+") -> (?<value>"+word+")", "()=> Time.IsAtOrAfter(${condition}), ()=> ${value},");  
            line = Regex.Replace(line, @"else (?<default>"+word+")", "()=> ${default})");  

            // Rule tables - must come before "dates"
            line = Regex.Replace(line, @"match "+word, "Switch<" + ruleType + ">(");    
            if (line.Contains("->")) line = Util.RuleTableMatch(matchLine, line);

            // yyyy-mm-dd -> Date(yyyy,mm,dd)
            line = Util.ConvertDate(line); 

            return line;
        }
    }       
}