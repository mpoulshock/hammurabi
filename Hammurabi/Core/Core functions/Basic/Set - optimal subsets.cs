// Copyright (c) 2013 Hammura.bi LLC
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
using System.Collections;
using System.Collections.Generic;

namespace Akkadian
{
    public partial class Tset
    {
        /// <summary>
        /// Tries all combinations of Things in the input Tset and returns the
        /// combo that yields the highest value of the input function (fcn).
        /// </summary>
        /// <remarks>
        /// 1. Ties are broken based on the maximal combo that's found first.  Custom
        ///    tie-breaking logic can be built into the input function.
        /// 2. Performance is proportional to 2^n, where n is the number of Things
        ///    in the input Tset.
        /// 3. At present, it does not pass an empty set as an arg to the input fcn.
        /// </remarks>
        public Tset OptimalSubset(Func<Tset,Tnum> fcn)
        {
            return OptimalSubsetCore(this, x => fcn(x));
        }

        /// <summary>
        /// Implements Tset.OptimalSubset(Tnum fcn).
        /// </summary>
        private static Tset OptimalSubsetCore(Tset theSet, Func<Tset,Tnum> fcn)
        {
            Tset result = new Tset();

            // For each time period in the Tset
            for (int i=0; i < theSet.IntervalValues.Count; i++)
            {
                // Get some useful values
                Hval thisSetVal = theSet.IntervalValues.Values[i];
                DateTime start = theSet.IntervalValues.Keys[i];

                // Handle uncertainty of theSet
                if (!thisSetVal.IsKnown)
                {
                    result.AddState(start, thisSetVal);
                }
                else
                {
                    // Date parameters and set members of that time interval
                    List<Thing> mems = (List<Thing>)thisSetVal.Val;
                    DateTime end = Time.EndOf; 
                    try {end = theSet.IntervalValues.Keys[i+1];} catch {}

                    // For each combination of set members, get the associated fcn val
                    List<Tuple<Tset,Tnum>> setFcnVals = new List<Tuple<Tset,Tnum>>();
                    Tnum maxVal = new Tnum(Decimal.MinValue);
                    foreach (Tset s in Combos(mems))
                    {
                        // Invoke the fcn for that subset
                        Tnum val = fcn(s);

                        // Save the result of the fcn and the associated Tset
                        setFcnVals.Add(Tuple.Create(s, val));

                        // Update the running maximum value
                        maxVal = Max(maxVal, val);
                    }

                    // Foreach changepoint in maxVal, find the associated Tset
                    for (int j=0; j < maxVal.IntervalValues.Count; j++)
                    {
                        DateTime mDate = maxVal.IntervalValues.Keys[j];
                        if (mDate >= start && mDate < end)
                        {
                            // Get the associated Tset
                            Hval outSet = AssociatedTset(setFcnVals, maxVal, mDate);

                            // Add the change point
                            result.AddState(mDate, outSet);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the Tset associated with a given Tnum value on a given date.
        /// </summary>
        private static Hval AssociatedTset(List<Tuple<Tset,Tnum>> setFcnVals, Tnum val, DateTime asOfDate)
        {
            foreach(Tuple<Tset,Tnum> t in setFcnVals)
            {
                // Handle uncertainty
                Hval s = t.Item2.AsOf(asOfDate).FirstValue;
                Hval v = val.AsOf(asOfDate).FirstValue;
                if (s.IsUncertain && v.IsUncertain)     return new Hval(null);  // Not hitting this for some reason...
                if (s.IsUnstated && v.IsUnstated)       return new Hval(null, Hstate.Unstated);
                if (s.IsStub && v.IsStub)               return new Hval(null, Hstate.Stub);

                // Compare numeric values
                if (t.Item2.AsOf(asOfDate) == val.AsOf(asOfDate))
                {
                    // Tsets created in Combos() are eternal, so FirstValue is ok here
                    return t.Item1.FirstValue;
                }
            }
            return new Hval(null, Hstate.Stub);
        }

        /// <summary>
        /// Given a list of Things, returns a list of all possible Tset combinations.
        /// </summary>
        private static List<Tset> Combos(List<Thing> thingList)
        {
            List<Tset> result = new List<Tset>();

            // Count in binary to explore all combinations of included/excluded Things
            // Beware: number of combos = 2^n
            // TODO: If n > 9, use probabilistic algorithm?
            for (int i=0; i < Math.Pow(2,thingList.Count); i++)
            {
                // Create a bit array representing the number
                BitArray bits = new BitArray(new int[] { i });

                // Convert the bit array into a Tset combo
                List<Thing> tsetVal = new List<Thing>();
                for(int b=0; b < bits.Count; b++)
                {
                    if (bits[b]) tsetVal.Add(thingList[b]);
                }

                // Create a new Tset out of the combo and add it to the output list
                if (tsetVal.Count > 0)  // TODO: Consider not omitting null sets?
                {
                    result.Add(new Tset(tsetVal));
                }
            }

            return result;
        }
    }
}