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
using System.Linq;
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

	// TODO: Review accessibility level of these methods
	
	public partial class Facts
	{
		/// <summary>
		/// This is the main global data structure.
		/// It's where all the asserted facts live.
		/// </summary>
		private static List<Fact> FactBase = new List<Fact>();

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
		}



        // TODO: Move these to a separate file?



        /// <summary>
        /// Counts how many known facts there are. 
        /// </summary>
        public static int Count()
        {
            return FactBase.Count;
        }
        
		/// <summary>
		/// Retracts all facts in the factbase
		/// </summary>
		public static void Clear()
		{
			FactBase.Clear();
            ThingBase.Clear();
		}
        
        /// <summary>
        /// Restores factbase to its virgin state.
        /// </summary>
        public static void Reset()
        {
            FactBase.Clear();
            Facts.Unknowns.Clear();
            Facts.GetUnknowns = false;
        }

        /// <summary>
        /// Returns true if a symmetrical fact has been assserted.
        /// </summary>
        public static bool HasBeenAssertedSym(object e1, string rel, object e2)
        {
            return HasBeenAsserted(rel, e1, e2) ||
                HasBeenAsserted(rel, e2, e1);
        }

        /// <summary>
        /// Returns true if a fact has been assserted - 1 entity.
        /// </summary>
        public static bool HasBeenAsserted(string rel, object e1)
        {
            return HasBeenAsserted(rel, e1, null, null);
        }
        
        /// <summary>
        /// Returns true if a fact has been assserted - 2 entities.
        /// </summary>
        public static bool HasBeenAsserted(string rel, object e1, object e2)
        {
            return HasBeenAsserted(rel, e1, e2, null);
        }
        
        /// <summary>
        /// Returns true if a fact has been assserted - 3 entities.
        /// </summary>
        public static bool HasBeenAsserted(string rel, object e1, object e2, object e3)
        {
            // Look up fact in table of facts
            foreach (Fact f in FactBase)
            {
                if (f.Relationship == rel && Util.AreEqual(f.Arg1, e1) && Util.AreEqual(f.Arg2, e2) && Util.AreEqual(f.Arg3, e3))
                {
                    return true;
                }
            }
            
            // If fact is not found...
            return false;
        }

        /// <summary>
        /// Displays a crude list of all facts that have been asserted.
        /// To be used for diagnostic purposes only.
        /// </summary>
        public static string AssertedFacts()
        {
            string result = "";

            foreach (Fact f in FactBase)
            {
                result += ((Thing)f.Arg1).Id + " " + f.Relationship;

                if ((Thing)f.Arg2 != null)
                {
                    result += " " + ((Thing)f.Arg2).Id;
                }

                result += " = " + Convert.ToString(f.v.Out) + "\n";
            }

            return result;
        }

        /// <summary>
        /// Displays the cached value of the Tvar as a string.
        /// </summary>
        public static string GetCachedFcnValue(Facts.Fact f)
        {
            foreach (Fact f2 in FactBase)
            {
                if (f.Relationship == f2.Relationship && Util.AreEqual(f.Arg1, f2.Arg1) && Util.AreEqual(f.Arg2, f2.Arg2) && Util.AreEqual(f.Arg3, f2.Arg3))
                {
                    return Convert.ToString(f2.v.Out);
                }
            }

            return "?";
        }
	}
}