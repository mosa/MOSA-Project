/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs dominance calculations on basic Blocks built by a previous compilation stage.
    /// </summary>
    /// <remarks>
    /// The stage exposes the IDominanceProvider interface for other compilation stages to allow
    /// them to use the calculated dominance properties.
    /// <para/>
    /// The implementation is based on "A Simple, Fast Dominance Algorithm" by Keith D. Cooper, 
    /// Timothy J. Harvey, and Ken Kennedy, Rice University in Houston, Texas, USA.
    /// </remarks>
    public sealed class DominanceCalculationStage : IMethodCompilerStage, IDominanceProvider
    {
        #region Data members

        /// <summary>
        /// Holds the dominance information of a block.
        /// </summary>
        private BasicBlock[] _doms;

        /// <summary>
        /// Holds the dominance frontier Blocks.
        /// </summary>
        private BasicBlock[] _domFrontier;

        /// <summary>
        /// Holds the dominance frontier of individual Blocks.
        /// </summary>
        private BasicBlock[][] _domFrontierOfBlock;

        #endregion // Data members

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Dominance Calculation"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            // Check preconditions
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");

            // Retrieve the basic Blocks
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Dominance calculation requires basic Blocks.");

            CalculateDominance(blockProvider);
            CalculateDominanceFrontier(blockProvider.Blocks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<IL.CilToIrTransformationStage>(this);
        }

        /// <summary>
        /// Calculates the immediate dominance of all Blocks in the block provider.
        /// </summary>
        /// <param name="blockProvider">The block provider to calculate with.</param>
        private void CalculateDominance(IBasicBlockProvider blockProvider)
        {
            // Changed flag
            bool changed = true;
            // Get the block list
            List<BasicBlock> blocks = blockProvider.Blocks;
            // Blocks in reverse post order topology
            BasicBlock[] revPostOrder = ReversePostorder(blockProvider, blocks);

            // Allocate a dominance array
            _doms = new BasicBlock[blocks.Count];
            _doms[0] = blocks[0];

            // Calculate the dominance
            while (changed)
            {
                changed = false;
                foreach (BasicBlock b in revPostOrder)
                {
                    if (null != b)
                    {
                        BasicBlock idom = b.PreviousBlocks[0];
                        //Debug.Assert(-1 !=  Array.IndexOf(_doms, idom));

                        for (int idx = 1; idx < b.PreviousBlocks.Count; idx++)
                        {
                            BasicBlock p = b.PreviousBlocks[idx];
                            if (null != _doms[p.Index])
                                idom = Intersect(p, idom);
                        }

                        if (false == ReferenceEquals(_doms[b.Index], idom))
                        {
                            _doms[b.Index] = idom;
                            changed = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the dominance frontier of all Blocks in the block list.
        /// </summary>
        /// <param name="blocks">The list of basic Blocks.</param>
        private void CalculateDominanceFrontier(List<BasicBlock> blocks)
        {
            List<BasicBlock> domFrontier = new List<BasicBlock>();
            List<BasicBlock>[] domFrontiers = new List<BasicBlock>[blocks.Count];
            foreach (BasicBlock b in blocks)
            {
                if (b.PreviousBlocks.Count > 1)
                {
                    foreach (BasicBlock p in b.PreviousBlocks)
                    {
                        BasicBlock runner = p;
                        while (null != runner && false == ReferenceEquals(runner, _doms[b.Index]))
                        {
                            List<BasicBlock> runnerFrontier = domFrontiers[runner.Index];
                            if (null == runnerFrontier)
                                runnerFrontier = domFrontiers[runner.Index] = new List<BasicBlock>();

                            if (false == domFrontier.Contains(b))
                                domFrontier.Add(b);
                            runnerFrontier.Add(b);
                            runner = _doms[runner.Index];
                        }
                    }
                }
            }

            Debug.WriteLine(@"Computed dominance frontiers");
            int idx = 0;
            _domFrontierOfBlock = new BasicBlock[blocks.Count][];
            foreach (List<BasicBlock> frontier in domFrontiers)
            {
                if (null != frontier)
                    _domFrontierOfBlock[idx] = frontier.ToArray();
                idx++;
            }

            _domFrontier = domFrontier.ToArray();
        }

        #endregion // IMethodCompilerStage Members

        #region IDominanceProvider Members

        BasicBlock IDominanceProvider.GetImmediateDominator(BasicBlock block)
        {
            if (null == block)
                throw new ArgumentNullException(@"block");

            Debug.Assert(block.Index < _doms.Length, @"Invalid block index.");
            if (block.Index >= _doms.Length)
                throw new ArgumentException(@"Invalid block index.", @"block");

            return _doms[block.Index];
        }

        BasicBlock[] IDominanceProvider.GetDominators(BasicBlock block)
        {
            if (null == block)
                throw new ArgumentNullException(@"block");
            Debug.Assert(block.Index < _doms.Length, @"Invalid block index.");
            if (block.Index >= _doms.Length)
                throw new ArgumentException(@"Invalid block index.", @"block");

            // Return value
            BasicBlock[] result;
            // Counter
            int count, idx = block.Index;

            // Count the dominators first
            for (count = 1; 0 != idx; count++)
                idx = _doms[idx].Index;

            // Allocate a dominator array
            result = new BasicBlock[count+1];
            result[0] = block;
            for (idx = block.Index, count = 1; 0 != idx; idx = _doms[idx].Index)
                result[count++] = _doms[idx];
            result[count] = _doms[0];

            return result;
        }

        BasicBlock[] IDominanceProvider.GetDominanceFrontier()
        {
            return _domFrontier;
        }

        BasicBlock[] IDominanceProvider.GetDominanceFrontierOfBlock(BasicBlock block)
        {
            if (null == block)
                throw new ArgumentNullException(@"block");

            return _domFrontierOfBlock[block.Index];
        }

        #endregion // IDominanceProvider Members

        #region Internals

        /// <summary>
        /// Retrieves the highest common immediate dominator of the two given Blocks.
        /// </summary>
        /// <param name="b1">The first basic block.</param>
        /// <param name="b2">The second basic block.</param>
        /// <returns>The highest common dominator.</returns>
        private BasicBlock Intersect(BasicBlock b1, BasicBlock b2)
        {
            BasicBlock f1 = b1, f2 = b2;

            while (f2 != null && f1 != null && f1.Index != f2.Index)
            {
                while (f1 != null && f1.Index > f2.Index)
                    f1 = _doms[f1.Index];

                while (f2 != null && f1 != null && f2.Index > f1.Index)
                    f2 = _doms[f2.Index];
            }

            return f1;
        }

        private BasicBlock[] ReversePostorder(IBasicBlockProvider blockProvider, List<BasicBlock> blocks)
        {
            BasicBlock[] result = new BasicBlock[blocks.Count - 1];
            int idx = 0;
            Queue<BasicBlock> workList = new Queue<BasicBlock>(blocks.Count);

            // Add next Blocks
            foreach (BasicBlock next in NextBlocks(blockProvider, blocks[0]))
                workList.Enqueue(next);

            while (0 != workList.Count)
            {
                BasicBlock current = workList.Dequeue();
                if (-1 == Array.IndexOf(result, current))
                {
                    result[idx++] = current;
                    foreach (BasicBlock next in NextBlocks(blockProvider, current))
                        workList.Enqueue(next);
                }
            }

            return result;
        }

        private IEnumerable<BasicBlock> NextBlocks(IBasicBlockProvider blockProvider, BasicBlock basicBlock)
        {
            List<BasicBlock> blocks = new List<BasicBlock>();
            foreach (Instruction i in basicBlock.Instructions)
            {
                switch (i.FlowControl)
                {
                    case FlowControl.Branch:
                        IBranchInstruction branch = (IBranchInstruction)i;
                        foreach (int target in branch.BranchTargets)
                        {
                            blocks.Add(FindLabelledBlock(blockProvider, target));
                        }
                        break;

                    case FlowControl.ConditionalBranch: 
                        goto case FlowControl.Branch;

                }
            }
            return blocks;
        }

        private BasicBlock FindLabelledBlock(IBasicBlockProvider blockProvider, int target)
        {
            BasicBlock result = blockProvider.FromLabel(target);
            if (null == result)
                throw new InvalidOperationException(@"Failed to resolve jump target.");
            return result;
        }

        #endregion // Internals
    }
}
