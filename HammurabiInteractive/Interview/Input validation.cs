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
	/*
	 * The code below validates user input data.
	 */
	
	public static partial class Interview
	{
		/// <summary>
		/// Determines whether a user's answer is valid.
		/// </summary>
		public static bool AnswerIsValid(Question question, string input)
		{
            // Allow "unstated"
            if (input.Trim() == "?") return true;

            // Otherwise, see if the input is consistent with the question type
			string qType = question.questionType;
			if (qType == "bool") return BoolIsValid(input);
			else if (qType == "numvar") return NumberIsValid(question, input);
			else if (qType == "date") return DateIsValid(question, input);
			return true;
		}

		/// <summary>
		/// Validates Boolean inputs.
		/// </summary>
		private static bool BoolIsValid(string input)
		{
			string i = input.Trim();
			if (i == "true" || i == "false") return true;
			return false;
		}

		/// <summary>
		/// Validates numeric inputs.
		/// </summary>
		private static bool NumberIsValid(Question question, string input)
		{
			if (input.Trim() == "") return false;

			// Is the value a number at all?
			string answer = input.Trim('$');
			if (!IsNumber(input))
			{
				return false;
			}
			
			decimal val = Convert.ToDecimal(answer);
			decimal min = question.minValue;
			decimal max = question.maxValue;

			// Is the value more than the maximum acceptable value?
			if (val > max)
			{
				return false;
			}

			// Is the value less than the minimum acceptable value?
			else if (val < min)
			{
				return false;
			}
			
			return true;
		}

		/// <summary>
		/// Determines whether a string is a valid number
		/// </summary>
		private static bool IsNumber(string s)
		{
			try
			{
				double.Parse(s);
				return true;
			}
			catch
			{
			}

			return false;
		}

		/// <summary>
		/// Validates date inputs.
		/// </summary>
		private static bool DateIsValid(Question question, string input)
		{
			DateTime theDate;

			// Is the date a validly-formatted date?
			try
			{
				theDate = DateTime.Parse(input); 
			}
			catch
			{
				return false;
			}

			// Is the date after the latest acceptable date for this question?
//			if (theDate > question.latestDate)
//			{
//				return false;
//			}
//
//			// Is the date before the earliest acceptable date for this question?
//			else if (theDate < question.earliestDate)
//			{
//				return false;
//			}	
			
			return true;
		}
	}
}

