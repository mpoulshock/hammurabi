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
//
// NO REPRESENTATION OR WARRANTY IS MADE THAT THIS PROGRAM ACCURATELY REFLECTS
// OR EMBODIES ANY APPLICABLE LAWS, REGULATIONS, RULES OR EXECUTIVE ORDERS 
// ("LAWS"). YOU SHOULD RELY ONLY ON OFFICIAL VERSIONS OF LAWS PUBLISHED BY THE 
// RELEVANT GOVERNMENT AUTHORITY, AND YOU ASSUME THE RESPONSIBILITY OF 
// INDEPENDENTLY VERIFYING SUCH LAWS. THE USE OF THIS PROGRAM IS NOT A 
// SUBSTITUTE FOR THE ADVICE OF AN ATTORNEY.

using Hammurabi;
using System;

namespace FedRules.FRCP
{
	/// <summary>
	/// Federal Rule of Civil Procedure, Rule 6.
	/// </summary>
	/// <cite>Fed. R. Civ. P. 6 (2010)</cite>
	public class Rule6
	{	 
		// TODO: Implement FRCP 6(a)(6)(B) and (C).
		
		/// <summary>
		/// FRCP due date calculation.
		/// </summary>
		/// <cite>Fed. R. Civ. P. 6(a) (2010)</cite>
		/// <updated>2010-11-11</updated>
		/// <remarks>
		/// Does not implement 6(a)(6)(B) and (C), which state that legal holidays include
		/// days declared a holiday by the President or Congress and holidays in the state 
		/// where the district court is located.
		/// </remarks>
		public static DateTime DueDate(int days, DateTime triggerDate)
		{		
		 	return triggerDate.ToMidnight().AddDays(days).CurrentOrNextBusinessDay();
		}

	}
}

