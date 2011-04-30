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
	class MainClass : H
	{
		/// <summary>
		/// Main
		/// </summary>
		public static void Main (string[] args)
		{
			TimeSpan duration = new TimeSpan();
			DateTime startTime = DateTime.Now;

            Demo();          
            
			DateTime stopTime = DateTime.Now;
			duration = stopTime - startTime;
			Console.WriteLine("Execution time: " + duration);
		}

				
		/// <summary>
		/// Stupid, legally incomplete demo to illustrate the basic concept
		/// </summary>
		private static void Demo()
		{		
			// Instantiate some legal entities (people, property, institutions)
			Person P1 = new Person("P1");
			Person P2 = new Person("P2");

			// Assert some facts about those entities
			Facts.Assert(P1, "IsPermanentlyAndTotallyDisabled");
            Facts.Assert(P1, "IsMarriedTo", P2);
            Facts.Assert(P1, "FedTaxFilingStatus", P2, "Married filing jointly");
			
			// Get a legal determination (and display it)
			Console.WriteLine(USC.Tit26.Sec152.IsDependentOf(P1,P2).Timeline);
			Console.WriteLine(USC.Tit26.Sec152.CannotBeADependentOf(P1,P2).Timeline);
		}
			
	}
}
