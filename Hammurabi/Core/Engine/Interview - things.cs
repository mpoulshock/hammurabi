// Copyright (c) 2012-2013 Hammura.bi LLC
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

using Akkadian;

namespace Interactive
{
    public partial class Engine
    {
        // Lame, temporary Things and Tvars that are placeholders 
        // for argument values in first-order functions.  
        // These are used in Hammurabi Interactive and Web Service.
        public static Thing Thing1, Thing2, Thing3;
        public static Tbool Tbool1, Tbool2, Tbool3;
        public static Tnum Tnum1, Tnum2,Tnum3;
        public static Tstr Tstr1, Tstr2, Tstr3;
        public static Tdate Tdate1, Tdate2, Tdate3;
        public static Tset Tset1, Tset2, Tset3;

        // Extend to C# types?
        public static string string1, string2, string3;
    }
}