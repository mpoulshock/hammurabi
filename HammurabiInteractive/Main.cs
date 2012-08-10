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

namespace Interactive
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
            // Main loop - runs the interview multiple times
            while (true)
            {
                // Run the interview
                Interview.ProcessRequest();

                // Repeat?
                Console.WriteLine("Repeat the interview?");
                string next = Console.ReadLine().ToLower();

                // Save interview as an .akk test
                if (next == "save") 
                {
                    AkkTest.WriteToFile();
                    Console.WriteLine("\r\nTest case saved.  Repeat the interview?");
                    next = Console.ReadLine().ToLower();
                }

                // Repeat or quit
                if (next == "y" || next == "yes") {}
                else break;
            }
        }
    }
}