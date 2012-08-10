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
using System.Collections.Generic;
using System.Globalization;
using Hammurabi;
	
namespace Interactive
{
	/// <summary>
	/// Generates a DOS-based, utterly primitive interview designed
	/// to illustrate how Hammurabi can simulate backwards chaining.
	/// </summary>
	public static partial class Interview 
	{
		// Create some entities that will be the subject of the interview.
		// Later, this will have to be made more dynamic...
		public static Thing p = new Thing("p");
		public static Thing e = new Thing("the person"); 
		public static Thing c   = new Thing("the employer");
		public static Thing p1 = new Thing("the seller"); 
		public static Thing p2 = new Thing("the buyer"); 
		public static Thing prop = new Thing("the item"); 

		/// <summary>
		/// Assesses the state of asserted facts and then decides what to do next.
		/// </summary>
		public static void ProcessRequest() 
		{
			// Clear fact base and assert any assumed facts
			Facts.Clear();
            AssertPreliminaryFacts();	

			// Initialize the .akk unit test
			AkkTest.testStr = "";
			AkkTest.InitializeUnitTest();

			// Load the goals to be investigated (onto the goals list)
			List<Func<Tvar>> goals = new List<Func<Tvar>>();

//			goals.Add(()=> USC.Tit8.Sec1403.IsUSCitizenPerCanalZone(e));
			goals.Add(()=> USC.Tit8.Sec1404.IsUSCitizenPerAlaska(p));

//			goals.Add(()=> Hammurabi.Sandbox.IsCitizen(e));
//			goals.Add(()=> Facts.QueryTvar<Tbool>("IsAlive", e));
//			goals.Add(()=> Facts.QueryTvar<Tdate>("DateOfBirth", e));
//			goals.Add(()=> USC.Tit29.Sec2612.IsEntitledToLeaveFrom(e,c));

			while (true)
			{
				// Get the response object from the interview engine
				Engine.Response response = Engine.Investigate(goals);

				// Ask the current question, or display the results
				if (!response.InvestigationComplete)
                {
                    // Ask the next question
                    DisplayQuestion(response); 

                    // Get and validate the answer
                    GetAndParseAnswer(response);
                } 
                else
                {
                    DisplayResults(goals);
                    break;
                }
            }

			// Display all facts that have been asserted (for diagnostic purposes)
			// Console.WriteLine("\nFacts: \n" + Facts.AssertedFacts());
		}

		/// <summary>
		/// Gets the and validates the user's answer to a question.
		/// </summary>
		private static void GetAndParseAnswer(Engine.Response response)
		{
			// Get data pertaining to the current question
			string currentRel = response.NextFact.relationship;
			Question currentQuestion = Templates.GetQ(currentRel);
			string currentQType = currentQuestion.questionType;

			// Read (and gently massage) the answer
			string answer = Console.ReadLine();
			answer = CleanBooleans(currentQType, answer);

			// Validate answer, then assert it
			if (AnswerIsValid(currentQuestion, answer))
			{
				AssertAnswer(response, answer);
			}
			else
			{
				GetAndParseAnswer(response);
			}
		}

		/// <summary>
		/// Asserts facts (for testing or pre-seeding purposes).
		/// </summary>
		private static void AssertPreliminaryFacts()   
		{
			// Put any facts to be asserted here, like this:
			// Facts.Assert(e, r.HoursWorkedPerWeek, 44);
		}
		
		/// <summary>
		/// Displays a question to the user.
		/// </summary>
		private static void DisplayQuestion(Engine.Response response)  
		{
			// Get data about the next question
			Facts.Factlet theFact = response.NextFact;
			Question theQ = Templates.GetQ(theFact.relationship);

			// TODO: Display appropriate question control (using theQ.questionType)

			// White space
			Console.WriteLine();

			// Display progress percentage
			Console.WriteLine("Percent complete: " + response.PercentComplete);

			// Display question text
			string qText = QuestionText(theFact, theQ);
			Console.WriteLine(qText);
			AkkTest.AddUnitTestAssertRel(theFact, theQ);

			// Display an explanation (if any) 
			if (theQ.explanation != "")
			{
				Console.WriteLine("Note: " + theQ.explanation);
			}
		}
		
        /// <summary>
        /// Returns the text of the question that should be displayed.
        /// </summary>
        public static string QuestionText(Facts.Factlet theF, Question theQ)
        {
            // Embed the name of the subject into the question
            string qText = theQ.questionText.Replace("{1}", theF.subject.Id);

            // If there is a direct object, embed its name into the question
            if (theF.object1 != null)
            {
                qText = qText.Replace("{2}", theF.object1.Id);
            }
        
            return qText;
        }

        /// <summary>
        /// Displays the engine's results of the interview session.
        /// </summary>
        private static void DisplayResults(List<Func<Tvar>> goals)
        {
            Console.WriteLine("\nResults: \n");
            Console.WriteLine(ResultsText(goals));
        }

		/// <summary>
        /// Displays the results of each goal.
        /// </summary>
        private static string ResultsText(List<Func<Tvar>> goals)
        {
            string result = "";

            // TODO: Does not correctly display Tset.TestOutput
            foreach (Func<Tvar> g in goals)
            {
                result += g.Invoke().TestOutput + "\n";
            }

            // Add result to test case
            Tvar testResult = goals[0].Invoke();
            AkkTest.CloseUnitTest(testResult);

            return result;
        }

		/// <summary>
		/// Converts free-text DOS answers into valid booleans, if applicable.
		/// </summary>
		private static string CleanBooleans(string currentQType, string input)
		{
			string i = input.Trim();

			if (currentQType == "bool")
			{
				i = i.ToLower();
				if (i == "t" || i == "yes" || i == "y") return "true";
				if (i == "f" || i == "no" || i == "n") return "false";
			}

			return i;
		}
	}
}