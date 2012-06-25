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

namespace Hammurabi
{
    /*
     * This class defines a question template.  The idea is that every fact input
     * (usually an entity-relationship-entity triple) can have metadata associated
     * with it so that a question can be rendered to an end user, as in an 
     * interview.
     * 
     * The Question object defines the explanatory text, drop down options, and range
     * of acceptable values for a particular relationship.
     */
    
    /// <summary>
    /// Question object
    /// </summary>
    public class Question
    {
        public string relationship;
        public string questionType;         // bool, string, numvar, dollars, date, calendar
        public string questionText;
        public List<string> dropDownOptions;
        public string explanation;
        public DateTime earliestDate;
        public DateTime latestDate;
        public decimal minValue;
        public decimal maxValue;

        /// <summary>
        /// General question constructor.
        /// </summary>
        public Question(string rel, string type, string text, string explan)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
        }

        /// <summary>
        /// Constructor for numeric questions.
        /// </summary>
        public Question(string rel, string type, string text, string explan, decimal min, decimal max)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
            minValue = min;
            maxValue = max;
        }

        /// <summary>
        /// Constructor for string-based questions with drop down lists.
        /// </summary>
        public Question(string rel, string type, string text, string explan, List<string> ddlOptions)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
            dropDownOptions = ddlOptions;
        }

        /// <summary>
        /// Constructor for date-based questions.
        /// </summary>
        public Question(string rel, string type, string text, string explan, DateTime earliest, DateTime latest)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
            earliestDate = earliest;
            latestDate = latest;
        }
    }
}