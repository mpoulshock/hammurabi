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
using System.IO;
using System.Net;
using Hammurabi;
    
namespace Interactive
{
    /// <summary>
    /// Converts an interview session to an Akkadian test case that is
    /// then written into the proper .akk file.
    /// </summary>
    public static class AkkTest 
    {
        // File to write generated test case to
        public static string filePath = Environment.CurrentDirectory.Replace(@"HammurabiInteractive\bin\Debug","") + @"Akkadian\" + Interview.akkFile;
    
        // Build up a string representing an .akk test case
        public static string testStr = "";

        // Generates an identifier for each test case
        public static Random random = new Random();


        /// <summary>
        /// Starts the .akk unit test text.
        /// </summary>
        public static void InitializeUnitTest()
        {
            // First line of test case (declaration)
            testStr += "Test: " + RandomNumber() + "\r\n";

            // Line declaring the Things used
            testStr += Interview.things + "\r\n";
        }

        /// <summary>
        /// Adds the assertion relationship to the .akk unit test text.
        /// </summary>
        public static void AddUnitTestAssertRel(Facts.Factlet theF, Question theQ)
        {
            testStr += "- " + theQ.relationship + "(" + theF.subject.Id;

            if (theF.object1 == null)
            {
                // e.g. Rel(p) =
                testStr += ") = ";
            }
            else
            {
                // e.g. Rel(p,q) =
                testStr += "," + theF.object1.Id + ") = ";
            }
        }

        /// <summary>
        /// Ends the .akk unit test text.
        /// </summary>
        public static void CloseUnitTest(Tvar val)
        {
            string result = Convert.ToString(val.TestOutput);
            testStr += "- " + Interview.goal + ".TestOutput =?= \"" + result + "\"";
        }

        /// <summary>
        /// Writes the .akk test case string to a file.
        /// </summary>
        public static void WriteToFile()
        {
            string result = "";
            string line;

            // Read each line of the .akk file, looking for the place to insert the unit test text.
            WebRequest req = WebRequest.Create(filePath);
            StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
            while( (line = stream.ReadLine()) != null )
            {
                result += line + "\r\n";

                if (line.StartsWith("# UNIT TESTS"))
                {
                    result += "\r\n" + testStr + "\r\n";
                }
            }
            stream.Close();

            // Write the updated Akkadian to a file
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);
            file.WriteLine(result);
            file.Close();
            
        }

        /// <summary>
        /// Generates a random number (to identify each test case).
        /// </summary>
        public static int RandomNumber()
        {
            return random.Next(100000, 999999999); 
        }
    }
}