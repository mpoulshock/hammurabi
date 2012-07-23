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
using System.Text.RegularExpressions;

namespace Akkadian
{
    /// <summary>
    /// Processes metadata about questions.
    /// </summary>
    public class Questions
    {
        // List of all questions with metadata defined in the .akk files.
        public static List<Qdata> questionData = new List<Qdata>();

        /// <summary>
        /// Class for keeping track of question metadata.
        /// </summary>
        public class Qdata
        {
            public string relationship;
            public string questionType;
            public string questionText;

            public Qdata(string rel, string type, string text)
            {
                relationship = rel;
                questionType = type;
                questionText = text;
            }
        }

        /// <summary>
        /// Extracts the question metadata from the lines.
        /// </summary>
        public static void GatherMetadata(string line, string previousLine)
        {
            if (line.Contains("In"))
            {
                // Identify lines that contain question metadata
                string[] ruleTypes = new string[7]{"TboolIn","TboolInSym","TnumIn","TdateIn","TstrIn","TsetIn","PersonIn"};
                string inputType = Util.StartsWithAny(line.Trim(), ruleTypes);

                // If the line has metadata, parse it
                if (inputType != "")
                {
                    string rel = GetRelationship(line);
                    string type = GetQuestionType(inputType);
                    string questionText = "";

                    if (previousLine.Contains("# >>"))
                    {
                        questionText = previousLine.Trim().Replace("# >>","").Trim();
                    }

                    // Add metadata to the question list
                    Qdata newQ = new Qdata(rel, type, questionText);
                    questionData.Add(newQ);
                }
            }
        }

        /// <summary>
        /// Extracts the question type.
        /// </summary>
        private static string GetQuestionType(string tvarType)
        {
            if (tvarType == "TboolIn" | tvarType == "TboolInSym") return "bool";
            else if (tvarType == "TnumIn") return "numvar";
            else if (tvarType == "TdateIn") return "date";
            else if (tvarType == "TsetIn") return "set";
            else if (tvarType == "PersonIn") return "person";
            else return "string";
        }

        /// <summary>
        /// Extracts the relationship.
        /// </summary>
        private static string GetRelationship(string line)
        {
            // Use a regex to identify the function name
            Match match = Regex.Match(line, 
                           @"(TboolIn|TboolInSym|TnumIn|TdateIn|TstrIn|TsetIn|PersonIn) ([a-zA-Z0-9]+)");
            if (match.Success)
            {
                return match.Groups[2].Value.Trim();
            }

            return "";
        }

        /// <summary>
        /// Write the metadata to .../Generated source/Question metadata.cs.
        /// </summary>
        public static void GenerateMetadataFile(string filePath)
        {
            // Open the question template class
            string result = 
                "using System; \r\nusing System.Collections.Generic; \r\nnamespace Interactive { \r\npublic class Templates {" +
                "\r\npublic static Question GetQ(string rel) { \r\nswitch(rel) { \r\n";

            // Keep track of what has already been added to the file
            string questionsSoFar = ",";

            // Write the metadata for each question
            foreach(Questions.Qdata q in Questions.questionData)
            {
                // Determine if question has already been written
                bool alreadyHave = questionsSoFar.Contains("," + q.relationship + ",");

                // Write it...
                if (!alreadyHave)
                    result += "case \"" + q.relationship + "\": return new Question(rel, \"" + q.questionType + "\", \"" + q.questionText + "\", \"\");\r\n";
            
                // Add it to the list of questions that have already been written
                questionsSoFar += q.relationship + ",";
            }

            // Close out the question template class
            result += "\r\ndefault: return new Question(rel, \"bool\", rel, \"\"); }}}}";

            // Write the string to a file
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath + "Question metadata.cs");
            file.WriteLine(result);
            file.Close();
        }
    }
}