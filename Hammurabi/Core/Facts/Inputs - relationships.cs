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
        /*
         * The following string constants represent the text of relationships that
         * can exist between legal entities.  
         * 
         * They are coded as strings to promote referential integrity, to make 
         * maintenance easier, and to take advantage of IntelliSense code completion.
         * Consolidating them also makes it easier to visually scan the relationships.
         * 
         * Incidentally, using constant may also provide small performance boost.
         * 
         * Because there are so many relationships that are possible, it is expected
         * that this list will become quite long.  Consequently, this has something
         * of a "Cyc" feel.
         * 
         * TODO: Get relationships from law-related files.
         */


        // Properties related to one person

        public const string BranchOfArmedForces             = "BranchOfArmedForces";
        public const string CountryOfCitizenship            = "CountryOfCitizenship";
        public const string CountryOfResidence              = "CountryOfResidence";
        public const string DateOfBirth                     = "DateOfBirth";
        public const string Gender                          = "Gender";
        public const string IsDeployedToCombatZone          = "IsDeployedToCombatZone";
        public const string IsDisabled                      = "IsDisabled";
        public const string IsIncapableOfSelfCare           = "IsIncapableOfSelfCare"; 
        public const string IsMarried                       = "IsMarried";                  // pref: MaritalStatus
        public const string IsStudent                       = "IsStudent";
        public const string MaritalStatus                   = "MaritalStatus";  // married, single/unmarried, widowed, legally separated, divorced, domestic partnership
        public const string PercentSelfSupport              = "PercentSelfSupport";
        public const string USImmigrationStatus             = "USImmigrationStatus";

        // Properties relating to one corporate entity

        public const string NumberOfEmployees               = "NumberOfEmployees";

        // Relationships between two people

        public const string ActsInLocoParentisOf            = "ActsInLocoParentisOf";
        public const string FamilyRelationship              = "FamilyRelationship";
        public const string HasCustodyOf                    = "HasCustodyOf";
        public const string HasDayToDayResponsibilityFor    = "HasDayToDayResponsibilityFor";
        public const string IsLegalGuardianOf               = "IsLegalGuardianOf";
        public const string IsMarriedTo                     = "IsMarriedTo";
        public const string IsNextOfKinOf                   = "IsNextOfKinOf";
        public const string IsParentOf                      = "IsParentOf";
        public const string LivesWith                       = "LivesWith";
        public const string ProvidesSupportFor              = "ProvidesSupportFor";
        public const string SharesHouseholdWith             = "SharesHouseholdWith";
        public const string SharesPrincipalAbodeWith        = "SharesPrincipalAbodeWith";

        // Relationships between two legal entities

        public const string DateStartedWorkingAt            = "DateStartedWorkingAt";
        public const string HoursWorkedPerWeek              = "HoursWorkedPerWeek";
        public const string IsAirlineFlightCrew             = "IsAirlineFlightCrew";
        public const string IsEmployedBy                    = "IsEmployedBy";
        public const string IsIndependentContractor         = "IsIndependentContractor";    // pref: NatureOfEmploymentRelationship
        public const string NatureOfEmploymentRelationship  = "NatureOfEmploymentRelationship";
        public const string StateJurisdiction               = "StateJurisdiction";

    }
}