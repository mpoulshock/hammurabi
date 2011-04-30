// Copyright (c) 2011 The Hammurabi Project
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

namespace Hammurabi
{
	public static partial class Facts
	{
		//TODO: Implement data validation of some sort (e.g. consistency checking)
		
		/// <summary>
		/// Asserts a temporal (Tvar) relation between two legal entities
		/// </summary>
		public static void Assert(LegalEntity e1, string rel, LegalEntity e2, Tvar val)
		{			
			Fact f = new Fact(e1, e2, rel, val);
			FactBase.Add(f);
		}
        public static void Assert(LegalEntity e1, string rel, LegalEntity e2, bool val)
        {           
            Fact f = new Fact(e1, e2, rel, new Tbool(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, LegalEntity e2)
        {           
            Fact f = new Fact(e1, e2, rel, new Tbool(true));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, LegalEntity e2, double val)
        {           
            Fact f = new Fact(e1, e2, rel, new Tnum(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, LegalEntity e2, decimal val)
        {           
            Fact f = new Fact(e1, e2, rel, new Tnum(val));
            FactBase.Add(f);
        }
		public static void Assert(LegalEntity e1, string rel, LegalEntity e2, int val)
        {           
            Fact f = new Fact(e1, e2, rel, new Tnum(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, LegalEntity e2, string val)
        {           
            Fact f = new Fact(e1, e2, rel, new Tstr(val));
            FactBase.Add(f);
        }
        
		/// <summary>
		/// Asserts a DateTime relation between two legal entities
		/// </summary>
		public static void Assert(LegalEntity e1, string rel, LegalEntity e2, DateTime dt)
		{			
			Fact f = new Fact(e1, e2, rel, dt);
			FactBase.Add(f);
		}
		
		/// <summary>
		/// Asserts a temporal property (Tvar) of one legal entity
		/// </summary>
		public static void Assert(LegalEntity e1, string rel, Tvar val)
		{			
			Fact f = new Fact(e1, rel, val);
			FactBase.Add(f);
		}
        public static void Assert(LegalEntity e1, string rel, bool val)
        {           
            Fact f = new Fact(e1, rel, new Tbool(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel)
        {           
            Fact f = new Fact(e1, rel, new Tbool(true));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, double val)
        {           
            Fact f = new Fact(e1, rel, new Tnum(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, decimal val)
        {           
            Fact f = new Fact(e1, rel, new Tnum(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, int val)
        {           
            Fact f = new Fact(e1, rel, new Tnum(val));
            FactBase.Add(f);
        }
        public static void Assert(LegalEntity e1, string rel, string val)
        {           
            Fact f = new Fact(e1, rel, new Tstr(val));
            FactBase.Add(f);
        }
        
		/// <summary>
		/// Asserts a DateTime property (Tvar) of one legal entity
		/// </summary>
		public static void Assert(LegalEntity e1, string rel, DateTime dt)
		{			
			Fact f = new Fact(e1, rel, dt);
			FactBase.Add(f);
		}

	}
}
