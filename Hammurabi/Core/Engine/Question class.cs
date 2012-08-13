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

namespace Interactive
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
        public string questionType;
        public string questionText;
        public string explanation;
        public string filePath;
        public string fullMethod;
        public string arg1Type;
        public string arg2Type;
        public string arg3Type;


        public Question(string rel, string type, string text, string explan)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
        }

        public Question(string rel, string type, string text, string explan,
                        string file, string method, string arg1, string arg2, string arg3)
        {
            relationship = rel;
            questionType = type;
            questionText = text;
            explanation = explan;
            filePath = file;
            fullMethod = method;
            arg1Type = arg1;
            arg2Type = arg2;
            arg3Type = arg3;
        }
    }
}