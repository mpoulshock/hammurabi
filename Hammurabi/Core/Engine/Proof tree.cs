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

using System.Collections.Generic;
using System.Diagnostics;
using Akkadian;

namespace Interactive
{
    public partial class Engine
    {
        /// <summary>
        /// A node on the proof tree.
        /// </summary>
        public class ProofTreeNode
        {
            public Facts.Fact TheFact;
            public int Depth;

            public ProofTreeNode(Facts.Fact fact, int depth)
            {
                TheFact = fact;
                Depth = depth;
            }
        }

        // Keeps track of the list of traversed facts
        public static List<ProofTreeNode> ProofTree = new List<ProofTreeNode>();

        /// <summary>
        /// Initializes the proof tree.
        /// </summary>
        public static void InitializeProofTree()
        {
            ProofTree.Clear();
        }

        /// <summary>
        /// Adds a fact to the proof tree.
        /// </summary>
        public static void AddToProofTree(Facts.Fact f)
        {
            // Fact should be added if it is a regular rule that is
            // not already on the tree (to avoid repeats), or if it
            // is an input rule.
            bool addIt = true;
            foreach (ProofTreeNode n in ProofTree)
            {
                Facts.Fact f2 = n.TheFact;
                if (f.Relationship == f2.Relationship && Util.AreEqual(f.Arg1, f2.Arg1) && Util.AreEqual(f.Arg2, f2.Arg2) && Util.AreEqual(f.Arg3, f2.Arg3))
                {
                    addIt = false;
                }
            }

            // Approximates Akkadian function depth
            int depth = new StackTrace().FrameCount;

            // Sometimes the stack trace overestimates the function depth
            // (for example, in Switch() statements).  The following code
            // compensates for this by de-indenting facts so they're not
            // more than one level deeper than their parent level.
//            int depthOfLastFact = 0; 
//            if (ProofTree.Count > 0) depthOfLastFact = ProofTree[ProofTree.Count-1].Depth;
//            depth = Math.Min(depth, depthOfLastFact+1);

            if (addIt)
            // TODO: Filter out repeats, etc.
            ProofTree.Add(new ProofTreeNode(f, depth));
        }

        /// <summary>
        /// Displays the proof tree.
        /// </summary>
        public static string ShowProofTree()
        {
            string result = "";

            foreach (ProofTreeNode n in ProofTree)
            {
                // Gets the value of the fcn, as stored in the FactBase (no re-computation)
                string v = n.TheFact.GetCachedFcnValue();

                // Display the fact, indented one space per depth level on the function tree
                result += H.PadNSpaces(n.Depth) + n.TheFact.FormatFactAsString() + " = " + v + "\n";
            }

            return result;
        }
    }
}