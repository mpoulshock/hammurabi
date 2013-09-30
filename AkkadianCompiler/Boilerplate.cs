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
using System.IO;

namespace AkkadianCompiler
{
    public class Boilerplate
    {
        /// <summary>
        /// Extracts the document namespace from the .akk file.
        /// </summary>
        public static string GetDocNameSpace(string file)
        {
            StreamReader stream = new StreamReader(file);
            string line = "", space = "";

            for (int i=0; i<5; i++)
            {
                line = stream.ReadLine();
                if (line.Contains("Namespace:")) 
                {
                    space = Util.Clean(line, "Namespace:");
                }
            }

            stream.Close();
            return space;
        }

        /// <summary>
        /// Assembles the text that goes at the top of the .cs file
        /// </summary>
        public static string InitialBoilerplate(string file, string space)
        {
            // Read the file and extract the "namespace" and "using" data
            StreamReader stream = new StreamReader(file);
            string line = "", refs = "";

            for (int i=0; i<5; i++)
            {
                line = stream.ReadLine();
                if (line.Contains("References:"))
                {
                    refs = Util.Clean(line, "References:");
                }
            }

            // Assemble the result
            string nspace = GetNamespace(space);
            return Boilerplate.license +
                   Boilerplate.Using(refs, nspace) +
                   "namespace " + nspace + "\r\n{\r\n" +
                   ClassDecl(GetClass(space));
        }

        /// <summary>
        /// Declares the class in the .cs file.
        /// </summary>
        private static string ClassDecl(string className)
        {
            return "    public partial class " + className + " : H \r\n" + "    { \r\n";
        }
         
        /// <summary>
        /// Inserts the license terms in the .cs file.
        /// </summary>
        private static string license = 
            "// Copyright (c) " + DateTime.Now.Year.ToString() + " Hammura.bi LLC \r\n\r\n";
         
        /// <summary>
        /// Inserts the "using" statements in the .cs file.
        /// </summary>
        private static string Using(string references, string nspace)
        {
            string result = 
                "using System;\r\n" +
                "using Akkadian;\r\n" +
                "using System.Collections.Generic;\r\n" +
                "using NUnit.Framework;\r\n"; 

             string[] refs = references.Split(',');
             foreach (string s in refs)
             {
                 if (s.Trim() != "") result += "using " + s.Trim() + ";\r\n";
             }
    
             return result += "\r\n";
        }
         
        /// <summary>
        /// Extracts the namespace from a string.
        /// </summary>
        public static string GetNamespace(string s)
        {
             int i = s.LastIndexOf(".");
             if (i >= 0) return s.Substring(0,i);
             return "";
        }
         
        /// <summary>
        /// Extracts the class name from a string.
        /// </summary>
        public static string GetClass(string s)
        {
             int i = s.LastIndexOf(".") + 1;
             if (i >= 0) return s.Substring(i,s.Length-i);
             return "";
        }

        /// <summary>
        /// Closes the .cs class and namespace.
        /// </summary>
        public static string ClassAndNamespaceClose = "    } \r\n}\r\n";
    }
}
