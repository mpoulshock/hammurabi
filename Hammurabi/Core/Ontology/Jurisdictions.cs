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
    /// Enumerations of jurisdictions, such as countries, states,
    /// and provinces. 
    /// </summary>
    public class Juris : LegalEntity
    {
        /// <summary>
        /// List of U.S. states and territories subject to federal jurisdiction. 
        /// </summary>
        enum State
        {
            Alabama,
            Alaska,
            Arizona,
            Arkansas,
            California,
            Colorado,
            Connecticut,
            Delaware,
            Florida,
            Georgia,
            Hawaii,
            Idaho,
            Illinois,
            Indiana,
            Iowa,
            Kansas,
            Kentucky,
            Louisiana,
            Maine,
            Maryland,
            Massachusetts,
            Michigan,
            Minnesota,
            Mississippi,
            Missouri,
            Montana,
            Nebraska,
            Nevada,
            NewHampshire,
            NewJersey,
            NewMexico,
            NewYork,
            NorthCarolina,
            NorthDakota,
            Ohio,
            Oklahoma,
            Oregon,
            Pennsylvania,
            RhodeIsland,
            SouthCarolina,
            SouthDakota,
            Tennessee,
            Texas,
            Utah,
            Vermont,
            Virginia,
            Washington,
            WestVirginia,
            Wisconsin,
            Wyoming,
            DistrictOfColumbia,
            AmericanSamoa,
            Micronesia,
            Guam,
            MarshallIslands,
            NorthernMarianaIslands,
            Palau,
            PuertoRico,
            VirginIslands
        }
        
        /// <summary>
        /// Returns the (U.S.) state jurisdiction applicable to the dispute
        /// between two legal entities.
        /// </summary>
        public static Tbool StateIs(LegalEntity e1, LegalEntity e2, string state)
        {
            return Facts.Sym(e1, r.StateJurisdiction, e2, state);
        }

        /// <summary>
        /// A specialized Switch() function that handles jurisdictional differences.
        /// </summary>
        public static T Switch<T>(Tstr state, T AL, T AK, T AZ, T AR, T CA, T CO, T CT, T DE, T FL, T GA, 
                                  T HI, T ID, T IL, T IN, T IA, T KS, T KY, T LA, T ME, T MD, T MA, T MI, 
                                  T MN, T MS, T MO, T MT, T NE, T NV, T NH, T NJ, T NM, T NY, T NC, T ND, 
                                  T OH, T OK, T OR, T PA, T RI, T SC, T SD, T TN, T TX, T UT, T VT, T VA, 
                                  T WA, T WV, T WI, T WY) where T : Tvar
        {
            return H.Switch<T>(state == "Alabama",          AL,
                               state == "Alaska",           AK,
                               state == "Arizona",          AZ,
                               state == "Arkansas",         AR,
                               state == "California",       CA,
                               state == "Colorado",         CO,
                               state == "Connecticut",      CT,
                               state == "Delaware",         DE,
                               state == "Florida",          FL,
                               state == "Georgia",          GA,
                           H.Switch<T>(
                               state == "Hawaii",           HI,
                               state == "Idaho",            ID,
                               state == "Illinois",         IL,
                               state == "Indiana",          IN,
                               state == "Iowa",             IA,
                               state == "Kansas",           KS,
                               state == "Kentucky",         KY,
                               state == "Louisiana",        LA,
                               state == "Maine",            ME,
                               state == "Maryland",         MD,
                            H.Switch<T>(
                               state == "Massachusetts",    MA,
                               state == "Michigan",         MI,
                               state == "Minnesota",        MN,
                               state == "Mississippi",      MS,
                               state == "Missouri",         MO,
                               state == "Montana",          MT,
                               state == "Nebraska",         NE,
                               state == "Nevada",           NV,
                               state == "New Hampshire",    NH,
                               state == "New Jersey",       NJ,
                             H.Switch<T>(
                               state == "New Mexico",       NM,
                               state == "New York",         NY,
                               state == "North Carolina",   NC,
                               state == "North Dakota",     ND,
                               state == "Ohio",             OH,
                               state == "Oklahoma",         OK,
                               state == "Oregon",           OR,
                               state == "Pennsylvania",     PA,
                               state == "Rhode Island",     RI, 
                               state == "South Carolina",   SC,
                              H.Switch<T>(
                               state == "South Dakota",     SD,
                               state == "Tennessee",        TN,
                               state == "Texas",            TX,
                               state == "Utah",             UT,
                               state == "Vermont",          VT,
                               state == "Virginia",         VA,
                               state == "Washington",       WA,
                               state == "West Virginia",    WV,
                               state == "Wisconsin",        WI,
                               state == "Wyoming",          WY,
                               (T)Auxiliary.ReturnProperTvar<T>()
                               )))));
        }
    }
}
