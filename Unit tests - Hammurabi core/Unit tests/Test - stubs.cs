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
// INDEPENDENTLY VERIFYING SUCH LAWS.

using Hammurabi;
using NUnit.Framework;

namespace Hammurabi.UnitTests.CoreFcns
{
    [TestFixture]
    public class Stubs : H
    {
        // Stub
        
        [Test]
        public void Stub_1()
        {
            Tbool result = Stub();
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        // StubIf

        [Test]
        public void Stub_2()
        {
            Tbool condition = new Tbool(true);
            Tbool result = StubIf(condition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }

        [Test]
        public void Stub_3()
        {
            Tbool condition = new Tbool(false);
            Tbool result = StubIf(condition);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void Stub_4()
        {
            Tbool condition = new Tbool();
            Tbool result = StubIf(condition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Stub_5()
        {
            Tbool prongA = new Tbool(true);
            Tbool prongB = StubIf(true);
            Tbool result = prongA & prongB;
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Stub_6()
        {
            Tbool prongA = new Tbool(true);
            Tbool prongB = StubIf(false);
            Tbool result = prongA & prongB;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void Stub_7()
        {
            Tbool prongA = new Tbool(true);
            Tbool prongB = StubIf(new Tbool());
            Tbool result = prongA & prongB;
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Stub_8()
        {
            Tbool prongA = new Tbool(false);
            Tbool prongB = StubIf(false);
            Tbool result = prongA & prongB;
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void Stub_9()
        {
            Tbool prongA = new Tbool(false);
            Tbool prongB = StubIf(true);
            Tbool result = prongA | prongB;
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void Stub_10()
        {
            Tbool prongA = new Tbool(false);
            Tbool prongB = StubIf(false);
            Tbool result = prongA | prongB;
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void Stub_11()
        {
            Tbool prongA = new Tbool(false);
            Tbool prongB = StubIf(new Tbool());
            Tbool result = prongA | prongB;
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        // TestOrStubIf

        [Test]
        public void TestOrStubIf_1()
        {
            Tbool testCondition = new Tbool(true);
            Tbool stubCondition = new Tbool(true);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_2()
        {
            Tbool testCondition = new Tbool(true);
            Tbool stubCondition = new Tbool(false);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_3()
        {
            Tbool testCondition = new Tbool(true);
            Tbool stubCondition = new Tbool();
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_4()
        {
            Tbool testCondition = new Tbool(false);
            Tbool stubCondition = new Tbool(true);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_5()
        {
            Tbool testCondition = new Tbool(false);
            Tbool stubCondition = new Tbool(false);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_6()
        {
            Tbool testCondition = new Tbool(false);
            Tbool stubCondition = new Tbool();
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_7()
        {
            Tbool testCondition = new Tbool();
            Tbool stubCondition = new Tbool(true);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_8()
        {
            Tbool testCondition = new Tbool();
            Tbool stubCondition = new Tbool(false);
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void TestOrStubIf_9()
        {
            Tbool testCondition = new Tbool();
            Tbool stubCondition = new Tbool();
            Tbool result = TestOrStubIf(testCondition, stubCondition);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        // IsFullTimeEmployee (which uses TestOrStubIf)
        
        [Test]
        public void IsFullTimeEmployee_1()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 0);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }

        [Test]
        public void IsFullTimeEmployee_2()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 29);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM False ", result.TestOutput);
        }
        
        [Test]
        public void IsFullTimeEmployee_3()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 30);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("Unknown", result.TestOutput);
        }
        
        [Test]
        public void IsFullTimeEmployee_4()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 35);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void IsFullTimeEmployee_5()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 38);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
        [Test]
        public void IsFullTimeEmployee_6()
        {
            Person e = new Person("e");
            Corp c = new Corp("c");
            Facts.Reset();
            Facts.Assert(e, "HoursWorkedPerWeek", c, 40);
            Tbool result = Econ.IsFullTimeEmployee(e,c);
            Assert.AreEqual("1/1/0001 12:00:00 AM True ", result.TestOutput);
        }
        
    }
}