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

namespace Akkadian
{
    public class Boilerplate
    {
         public static string InitialBoilerplate(string file)
         {
             // Read the file and extract the "namespace" and "using" data
             WebRequest req = WebRequest.Create(file);
             StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
             string line = "", space = "", refs = "";
             while( (line = stream.ReadLine()) != null )
             {
                 if (line.Contains("Namespace:")) space = Util.Clean(line, "Namespace:");
                 else if (line.Contains("References:")) refs = Util.Clean(line, "References:");
             }
             stream.Close();
             
             // Get namespace to be used in unit tests
             MainClass.unitTestNameSpace = space.Replace(".","");

             // Assemble the result
             string nspace = GetNamespace(space);
             return Boilerplate.license +
                    Boilerplate.Using(refs, nspace) +
                    "namespace " + nspace + "\r\n{\r\n" +
                    ClassDecl(GetClass(space));
         }
            
         private static string ClassDecl(string className)
         {
              if (className == "H")
                  return "    public partial class H \r\n" + "    { \r\n";
              else
                  return "    public partial class " + className + " : H \r\n" + "    { \r\n";
         }
         
         private static string license = 
             "// Copyright (c) " + DateTime.Now.Year.ToString() + " Hammura.bi LLC \r\n\r\n";
         
         private static string Using(string references, string nspace)
         {
             string result = "using Hammurabi; \r\n" + 
                         "using System;\r\n" +
                         "using System.Collections.Generic;\r\n" +
                         "using NUnit.Framework;\r\n" +
                         "using USC;\r\n";
            
             // Add the namespace itself in order to accomodate 
             // unit test class.
             if (nspace != "Hammurabi")
                 result += "using " + nspace + ";\r\n";
             
             string[] refs = references.Split(',');
             foreach (string s in refs)
             {
                 if (s.Trim() != "") result += "using " + s.Trim() + ";\r\n";
             }
    
             return result += "\r\n";
         }
         
         private static string GetNamespace(string s)
         {
             int i = s.LastIndexOf(".");
             if (i >= 0) return s.Substring(0,i);
             return "";
         }
         
         private static string GetClass(string s)
         {
             int i = s.LastIndexOf(".") + 1;
             if (i >= 0) return s.Substring(i,s.Length-i);
             return "";
         }
         
         public static string ClassAndNamespaceClose = "    } \r\n}\r\n";
    }
}
