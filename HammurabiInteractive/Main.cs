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
using Interactive;

namespace Akkadian.DosInteractive
{
    /// <summary>
    /// Class that runs a DOS-based interview of pre-set Hammurabi goals.  
    /// </summary>
    /// <remarks>
    /// The interview has two purposes: 
    ///     (1) Serve as a prototype for a more sophisticated interactive 
    ///         shell to Hammurabi.
    ///     (2) Provide a convenient way to test Hammurabi and save the
    ///         test cases to the Akkadian rule files.
    /// </remarks>
    class MainClass
    {
        public static void Main (string[] args)
        {
            // Just for fun...
//            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Hammura.bi Interactive";
//            Console.WindowWidth = 100;
//            Console.WindowHeight = 40;
//            Console.WriteLine("Welcome to the Hammura.bi knowledge base.\n\n");

            // Ask the user what goal they want to investigate
            Console.WriteLine("Enter a goal to test:\n");
            string goal = Console.ReadLine();

            // Main loop - runs the interview multiple times
            while (true)
            {
                // If invalid goal, prompt for new goal
//                try
//                {
                    // Run the interview
                    Interview.ProcessRequest(goal);

                    // Next action
                    Console.WriteLine("Repeat?    r");
                    Console.WriteLine("Save?      s");
                    Console.WriteLine("New topic? n");
                    string next = Console.ReadLine();

                    // Save interview as an .akk test
                    if (next == "s") 
                    {
                        string file = Interactive.Templates.GetQ(goal.Split(' ')[0]).filePath;
                        AkkTest.WriteToFile(file);

                        // Next action
                        Console.WriteLine("\nTest case saved.\n");
                        Console.WriteLine("Repeat?    r");
                        Console.WriteLine("New topic? n");
                        next = Console.ReadLine();
                    }

                    // Switch to a new interview topic
                    if (next == "n")
                    {
                        Console.WriteLine("\nEnter a goal to test:\n");
                        goal = Console.ReadLine();
                    }

                    // Continue session or quit
                    if (next == "r" || next == "n") {}
                    else break;
//                }
//                catch
//                {
//                    Console.WriteLine("\nInvalid request. Enter goal:");
//                    goal = Console.ReadLine();
//                }
            }
        }
    }
}