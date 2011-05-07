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

namespace Hammurabi
{
    public partial class H
    {   
        /* These methods are used as "stubs" or placeholder functions in the legal
         * code.  Stubs indicate that a portion of law-related logic, usually
         * relating to some obscure factual situation, has not been developed.  
         * Stubs allow the system to provide correct results even when the code
         * is not as complete or granular as the law itself.  Stubs also indicate
         * where further development is needed.
         * 
         * See https://github.com/mpoulshock/hammurabi/wiki/Scope-and-granularity.
         */

        /// <summary>
        /// Indicates the presence of a stub by returning an unknown Tbool.
        /// </summary>
        public static Tbool Stub()
        {
            return new Tbool();
        }
    
        /// <summary>
        /// Indicates the presence of a conditional stub by returning an unknown
        /// Tbool when a given condition applies.
        /// This function should be used with a conjunction (& operator).
        /// </summary>
        /// <remarks>
        /// If condition is T => outputs U
        ///                 U =>         U
        ///                 F =>         T
        /// </remarks>
        public static Tbool StubIf(Tbool condition)
        {
            return IfThen(condition, new Tbool());
        }
        
        public static Tbool StubIf(bool condition)
        {
            return IfThen(new Tbool(condition), new Tbool());
        }
        
        /// <summary>
        /// Determines whether a given test condition is met; if not returns
        /// "unknown" if a second (stub) condition is met.
        /// This function should be used as a substitute for using StubIf()
        /// with a disjunction (| operator).
        /// </summary>
        /// <remarks>
        ///     Truth table:
        /// 
        /// TEST    STUB    => OUTPUT
        ///  T       T      =>  T
        ///  T       F      =>  T
        ///  T       U      =>  T
        ///  F       T      =>  U
        ///  F       F      =>  F
        ///  F       U      =>  U
        ///  U       T      =>  U
        ///  U       F      =>  U
        ///  U       U      =>  U
        /// </remarks>
        public static Tbool TestOrStubIf(Tbool testCondition, Tbool stubCondition)
        {
            return testCondition || (stubCondition & Stub());
        }
        
    }
}