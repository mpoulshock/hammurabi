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
    /// <summary>
    /// Relationships pertaining to military service.
    /// </summary>
    public class Mil
    {
        /// <summary>
        /// Indicates whether a person serves in the Armed Forces in any capacity.
        /// </summary>
        public static Tbool InArmedForces(Person p)
        {
            return InActiveArmedForces(p) ||
                   InReserveArmedForces(p);
        }
        
        /// <summary>
        /// Indicates whether a person serves in the Armed Forces (active).
        /// </summary>
        public static Tbool InActiveArmedForces(Person p)
        {
            Tstr branch = Facts.InputTstr(p, r.BranchOfArmedForces);
            
            return branch == "Army" ||
                   branch == "Navy" ||
                   branch == "Air Force" ||
                   branch == "Marine Corps" ||
                   branch == "Coast Guard";
        }
        
        /// <summary>
        /// Indicates whether a person serves in a reserve component of the 
        /// Armed Forces (including the National Guard).
        /// </summary>
        public static Tbool InReserveArmedForces(Person p)
        {
            Tstr branch = Facts.InputTstr(p, r.BranchOfArmedForces);
            
            return branch == "Army National Guard" ||
                   branch == "Army Reserve" ||
                   branch == "Air National Guard" ||
                   branch == "Navy Reserve" ||
                   branch == "Air Force Reserve" ||
                   branch == "Marine Corps Reserve" ||
                   branch == "Coast Guard Reserve";
        }

        /// <summary>
        /// Indicates whether a person is deployed to a combat zone.
        /// </summary>
        public static Tbool IsDeployedToCombatZone(Person p)
        {
            return Facts.InputTbool(p, r.IsDeployedToCombatZone);
        }
    }
}