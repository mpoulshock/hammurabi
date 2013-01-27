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
using Hammurabi;
    
namespace Interactive
{
    /// <summary>
    /// Converts an interview session to an Akkadian test case that is
    /// then written into the proper .akk file.
    /// </summary>
    public static class AkkTest 
    {
        // Build up a string representing an .akk test case
        public static string testStr = "";

        // Relationship that is asserted on each line of the .akk unit test
        public static string assertedRelationship = "";

        // Generates an identifier for each test case
        public static Random random = new Random();


        /// <summary>
        /// Starts the .akk unit test text.
        /// </summary>
        public static void InitializeUnitTest()
        {
            // First line of test case (declaration)
            testStr += "Test: " + RandomNumber() + "\r\n";

            string t1 = Engine.Thing1.Id;
            string t2 = Engine.Thing2.Id;
            string t3 = Engine.Thing3.Id;

            // Line declaring the Things used
            if (t3 != "")      testStr += "- Things " + t1 + ", " + t2 + ", " + t3 + "\r\n";
            else if (t2 != "") testStr += "- Things " + t1 + ", " + t2 + "\r\n";
            else               testStr += "- Thing " + t1 + "\r\n";
        }

        /// <summary>
        /// Adds the assertion relationship to the .akk unit test text.
        /// </summary>
        public static string AddUnitTestAssertRel(Facts.Fact theF, Question theQ)
        {
            string result = "- " + theQ.relationship + "(" + ((Thing)theF.Arg1).Id;

            if (theF.Arg2 == null)
            {
                // e.g. Rel(p) =
                result += ") = ";
            }
            else
            {
                // e.g. Rel(p,q) =
                result += "," + ((Thing)theF.Arg2).Id + ") = ";
            }

            return result;
        }

        /// <summary>
        /// Ends the .akk unit test text.
        /// </summary>
        public static void CloseUnitTest(Tvar val, string goal)
        {
            string result = Convert.ToString(val.TestOutput);
            testStr += "- " + TestGoal(goal) + ".TestOutput =?= \"" + result + "\"";
        }
        
        /// <summary>
        /// Builds the string of the goal being tested in .akk test case.
        /// </summary>
        public static string TestGoal(string goal)
        {
            Question q = Templates.GetQ(goal);

            // Note: for now, this only handles methods with Things as arguments
            string result = q.fullMethod + "(" + Engine.Thing1.Id;
            if (q.arg2Type != "") result += ", " + Engine.Thing2.Id;
            if (q.arg3Type != "") result += ", " + Engine.Thing3.Id;
            return result + ")";
        }

        /// <summary>
        /// Writes the .akk test case string to a file.
        /// </summary>
        public static void WriteToFile(string filePath)
        {
            string result = "";
            string line;
            bool success = false;

            // Read each line of the .akk file, looking for the place to insert the unit test text.
            StreamReader stream = new StreamReader(filePath);
            while( (line = stream.ReadLine()) != null )
            {
                result += line + "\r\n";

                if (line.StartsWith("# UNIT TESTS"))
                {
                    result += "\r\n" + testStr + "\r\n";
                    success = true;
                }
            }
            stream.Close();

            // If file doesn't have # UNIT TEST block, add one
            if (!success)
            {
                result += "\r\n# UNIT TESTS\r\n";
                result += "\r\n" + testStr + "\r\n";
            }

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