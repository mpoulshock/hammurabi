// Copyright (c) 2010-2013 Hammura.bi LLC
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
using System.Reflection;
using Interactive;

namespace Hammurabi
{
    // EXPERIMENTAL - C# custom attributes
    //    public class FunctionMetadata : Attribute
    //    {
    //        public string ReturnType { get; set; }
    //        public string Arg1Type { get; set; }
    //        public string Arg2Type { get; set; }
    //        public string Arg3Type { get; set; }
    //        public string QuestionText { get; set; }
    //        public string FullName { get; set; }
    //        public string AkkFile { get; set; }
    //        public Func<Tvar> TheFunc { get; set; }
    //    }

    /// <summary>
    /// A Fact object represents a fact that is stored in the knowledge
    /// base.
    /// </summary>
    public partial class Facts
    {
        /// <summary>
        /// A Fact object represents a fact that is stored in the knowledge
        /// base.
        /// </summary>
        public class Fact
        {
            public string Relationship;
            public object Arg1;
            public object Arg2;
            public object Arg3;
            public Tvar v;
            
            /// <summary>
            /// Sets a Tvar fact that establishes a relation between legal entities.
            /// </summary>
            public Fact(string rel, object arg1, object arg2, object arg3, Tvar val)
            {
                Relationship = rel;
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
                v = val;
            }
            
            public Fact(string rel, object arg1, object arg2, object arg3)
            {
                Relationship = rel;
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
                v = null;
            }
            
            /// <summary>
            /// Returns the value of the fact by invoking the function.
            /// </summary>
            public Tvar Value()
            {
                // Consider implementing caching
                return this.GetFunction().Invoke();
            }
            
            /// <summary>
            /// Returns a function representing the fact.
            /// </summary>
            public Func<Tvar> GetFunction()
            {
                // Get the template for the function, based on the relationship
                Question q = Interactive.Templates.GetQ(Relationship);
                
                // Set the function's arguments before invoking it
                
                // Convert first argument from string to proper type
                if (q.arg1Type == "Thing") Engine.Thing1 = Facts.AddThing(Convert.ToString(Arg1));
                else if (q.arg1Type == "Tbool") Engine.Tbool1 = Convert.ToBoolean(Arg1);
                else if (q.arg1Type == "Tnum")  Engine.Tnum1 = Convert.ToDecimal(Arg1);
                else if (q.arg1Type == "Tstr")  Engine.Tstr1 = Convert.ToString(Arg1);
                else if (q.arg1Type == "Tdate") Engine.Tdate1 = Convert.ToDateTime(Arg1);
                else if (q.arg1Type == "Tset")  Engine.Tset1 = (Tset)Arg1;   // ?
                
                // Second argument
                if (q.arg2Type == "Thing") Engine.Thing2 = Facts.AddThing(Convert.ToString(Arg2));
                else if (q.arg2Type == "Tbool") Engine.Tbool2 = Convert.ToBoolean(Arg2);
                else if (q.arg2Type == "Tnum")  Engine.Tnum2 = Convert.ToDecimal(Arg2);
                else if (q.arg2Type == "Tstr")  Engine.Tstr2 = Convert.ToString(Arg2);
                else if (q.arg2Type == "Tdate") Engine.Tdate2 = Convert.ToDateTime(Arg2);
                else if (q.arg2Type == "Tset")  Engine.Tset2 = (Tset)Arg2;
                
                // Third argument
                if (q.arg3Type == "Thing") Engine.Thing3 = Facts.AddThing(Convert.ToString(Arg3));
                else if (q.arg3Type == "Tbool") Engine.Tbool3 = Convert.ToBoolean(Arg3);
                else if (q.arg3Type == "Tnum")  Engine.Tnum3 = Convert.ToDecimal(Arg3);
                else if (q.arg3Type == "Tstr")  Engine.Tstr3 = Convert.ToString(Arg3);
                else if (q.arg3Type == "Tdate") Engine.Tdate3 = Convert.ToDateTime(Arg3);
                else if (q.arg3Type == "Tset")  Engine.Tset3 = (Tset)Arg3;
                
                // Return the lambda function
                return Interactive.Templates.GetQ(Relationship).theFunc;
            }

            /// <summary>
            /// Returns the Type of Tvar, given the relationship text.
            /// </summary>
            /// <returns>The tvar type.</returns>
            public string GetTvarType()
            {
                Question q = Interactive.Templates.GetQ(Relationship);
                return q.questionType;
            }

            /// <summary>
            /// Displays the asserted fact as a string.
            /// </summary>
            /// <example>
            /// IsParentOf(a,b)
            /// </example>
            public string FormatFactAsString()
            {
                string result = Relationship + "(" + Util.ArgToString(Arg1);
                if (Arg2 != null) result += "," + Util.ArgToString(Arg2);
                if (Arg3 != null) result += "," + Util.ArgToString(Arg3);
                result += ")";
                return result;
            }

            /// <summary>
            /// Returns the value of the fact, as a string.
            /// </summary>
            public string ValueAsString()
            {
                string result = "";
                foreach(KeyValuePair<DateTime,Hval> de in Value().IntervalValues)
                {
                    if (Convert.ToDateTime(de.Key) == Time.DawnOf)
                    {
                        result += "DawnOfTime   " + de.Value.ToString + "\n";
                    }
                    else
                    {
                        result += de.Key.ToString("yyyy-MM-dd") + "   " + de.Value.ToString + "\n"; 
                    }
                }
                return result;
            }
            
            /// <summary>
            /// Returns the fact's question text.
            /// </summary>
            public string QuestionText()
            {
                //                string r = "";
                //
                //                // Get all types in hammurabi.dll
                //                foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                //                {
                //                    // Get all methods for each of those types
                //                    foreach(MethodInfo m in t.GetMethods())
                //                    {
                //                        // Look at the custom attributes in each method
                //                        foreach (Attribute a in m.GetCustomAttributes())
                //                        {
                //                            if (a is FunctionMetadata)
                //                            {
                //                                FunctionMetadata fm = (FunctionMetadata)a;
                //
                //                                if (fm.FullName == Relationship)
                //                                {
                //                                    r = fm.QuestionText;
                //                                    break;
                //                            }
                //                        }
                //                    }
                //
                //                }
                //                string result = r;
                
                // Embed the names of the Things into the question
                string result = Interactive.Templates.GetQ(Relationship).questionText;
                
                return result.Replace("{1}", Util.ArgToString(Arg1))
                    .Replace("{2}", Util.ArgToString(Arg2))
                        .Replace("{3}", Util.ArgToString(Arg3));
            }

            /// <summary>
            /// Displays the cached value of the function as a string (does not re-compute it).
            /// </summary>
            public string GetCachedFcnValue()
            {
                foreach (Fact f in FactBase)
                {
                    if (f.Relationship == this.Relationship && Util.AreEqual(f.Arg1, this.Arg1) && Util.AreEqual(f.Arg2, this.Arg2) && Util.AreEqual(f.Arg3, this.Arg3))
                    {
                        return Convert.ToString(f.v.Out);
                    }
                }
                
                return "?";
            }
        }
    }
}