/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;

//using Mosa.Compiler.CompilerFramework;
//using Mosa.Compiler.Linker;
//using Mosa.Compiler.Metadata;
//using Mosa.Compiler.Metadata.Signatures;

//using IR = Mosa.Compiler.CompilerFramework.IR;

//namespace Mosa.Compiler.CompilerFramework.Stages
//{
//    /// <summary>
//    /// BasicBlockReduction attempts to eliminate useless control flow created as a side effect of other compiler optimizations.
//    /// </summary>
//    public class BlockReductionStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
//    {
//        #region Data members

//        /// <summary>
//        /// 
//        /// </summary>
//        protected BasicBlock prologue;

//        /// <summary>
//        /// 
//        /// </summary>
//        protected BasicBlock epilogue;
//        /// <summary>
//        /// 
//        /// </summary>
//        protected IArchitecture arch;
//        /// <summary>
//        /// 
//        /// </summary>
//        protected BitArray workArray;
//        /// <summary>
//        /// 
//        /// </summary>
//        protected Stack<BasicBlock> workList;

//        #endregion // Data members

//        #region Properties

//        #region IPipelineStage Members

//        //<summary>
//        //Retrieves the name of the compilation stage.
//        //</summary>
//        //<value>The name of the compilation stage.</value>
//        string IPipelineStage.Name { get { return @"Basic Block Reduction"; } }

//        #endregion // IPipelineStage Members

//        #region IMethodCompilerStage Members

//        /// <summary>
//        /// Runs the specified compiler.
//        /// </summary>
//        /// <param name="compiler">The compiler.</param>
//        public void Run(IMethodCompiler compiler)
//        {
//            // Retrieve the prologue and epilogue Blocks
//            prologue = FindBlock(-1);
//            epilogue = FindBlock(Int32.MaxValue);

//            // Contains a list of Blocks which need to be review during the second pass
//            workArray = new BitArray(basicBlocks.Count);
//            workList = new Stack<BasicBlock>();

//            // Pass One
//            // Iterate all Blocks, remove and/or combine Blocks
//            // Loop backwards to improve performance and reduce looping
//            for (int i = basicBlocks.Count - 1; i >= 0; i--)
//            {
//                BasicBlock block = basicBlocks[i];

//                while (ProcessBlock(block)) ;

//                workArray.Set(block.Index, false);
//            }

//            // Pass Two
//            while (workList.Count != 0)
//            {
//                BasicBlock block = workList.Pop();

//                if (workArray.Get(block.Index))
//                {

//                    while (ProcessBlock(block)) ;

//                    workArray.Set(block.Index, false);
//                }
//            }
//        }

//        /// <summary>
//        /// Processes the block.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool ProcessBlock(BasicBlock block)
//        {
//            bool changed = false;

//            if (TryToRemoveUnreferencedBlock(block))
//                changed = true;

//            if (TryToFoldRedundantBranch(block))
//                changed = true;

//            if (TryToFoldRedundantBranch2(block))
//                changed = true;

//            if (TryToRemoveSelfCycleBlock(block))
//                changed = true;

//            if (TryToCombineBlocks(block))
//                changed = true;

//            //if (TryToRemoveEmptyBlock(block))
//            // 	changed = true;

//            //if (TryToHoistBranch(block))
//            //	changed = true;

//            return changed;
//        }

//        /// <summary>
//        /// Marks the block for review.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        protected void MarkBlockForReview(BasicBlock block)
//        {
//            if (!workArray.Get(block.Index))
//            {
//                workArray.Set(block.Index, true);
//                workList.Push(block);
//            }
//        }

//        /// <summary>
//        /// Marks the Blocks for review.
//        /// </summary>
//        /// <param name="blocks">The Blocks.</param>
//        protected void MarkBlocksForReview(List<BasicBlock> blocks)
//        {
//            foreach (BasicBlock block in blocks)
//                MarkBlockForReview(block);
//        }

//        /// <summary>
//        /// Marks the related Blocks for review.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <param name="self">if set to <c>true</c> [self].</param>
//        protected void MarkRelatedBlocksForReview(BasicBlock block, bool self)
//        {
//            foreach (BasicBlock previousBlock in block.PreviousBlocks)
//                MarkBlocksForReview(previousBlock.NextBlocks);

//            MarkBlocksForReview(block.PreviousBlocks);
//            MarkBlocksForReview(block.NextBlocks);

//            foreach (BasicBlock nextBlock in block.NextBlocks)
//                MarkBlocksForReview(nextBlock.NextBlocks);

//            if (self)
//                MarkBlockForReview(block);
//        }

//        /// <summary>
//        /// Tries to remove unused Blocks.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToRemoveUnreferencedBlock(BasicBlock block)
//        {
//            if ((block.PreviousBlocks.Count == 0) && (block != prologue) && (block != epilogue) && (block.instructions.Count != 0))
//            {

//                //Mark Blocks for review in second pass
//                MarkBlocksForReview(block.NextBlocks);

//                foreach (BasicBlock nextblock in block.NextBlocks)
//                    while (nextblock.PreviousBlocks.Remove(block)) ;

//                block.Instructions.Clear();
//                block.NextBlocks.Clear();
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to folding redundant branch.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToFoldRedundantBranch(BasicBlock block)
//        {
//            if (block.NextBlocks.Count == 2)
//            {
//                if (block.NextBlocks[0] == block.NextBlocks[1])
//                {
//                    // Replace conditional branch instruction with an unconditional jump instruction

//                    // Mark Blocks for review in second pass
//                    MarkRelatedBlocksForReview(block, true);

//                    // Remove last instruction of this block
//                    block.Instructions.RemoveAt(block.Instructions.Count - 1);

//                    // Create target list
//                    int[] targets = new int[1];
//                    targets[0] = block.NextBlocks[0].Label;

//                    // Create JUMP instruction
//                    LegacyInstruction instruction = arch.CreateInstruction(typeof(Mosa.Compiler.Framework.CIL.BranchInstruction), Mosa.Compiler.Framework.CIL.OpCode.Br, targets);

//                    // Assign block index to instruction
//                    instruction.Block = block.Index;

//                    // Add JUMP instruction to the next block
//                    block.Instructions.Add(instruction);

//                    // Remove duplicate next block
//                    block.NextBlocks.RemoveAt(1);

//                    return true;
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to folding redundant branch.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToFoldRedundantBranch2(BasicBlock block)
//        {
//            if ((block.NextBlocks.Count != 0) && (block.Instructions.Count != 0))
//            {
//                IBranchInstruction lastInstruction = block.LastInstruction as IBranchInstruction;
//                if (lastInstruction.BranchTargets.Length == 2)
//                {
//                    if (lastInstruction.BranchTargets[0] == lastInstruction.BranchTargets[1])
//                    {
//                        // Replace conditional branch instruction with an unconditional jump instruction

//                        // Mark Blocks for review in second pass
//                        MarkRelatedBlocksForReview(block, true);

//                        // Remove last instruction of this block
//                        block.Instructions.RemoveAt(block.Instructions.Count - 1);

//                        // Create target list
//                        int[] targets = new int[1];
//                        targets[0] = block.NextBlocks[0].Label;

//                        // Create JUMP instruction
//                        LegacyInstruction instruction = arch.CreateInstruction(typeof(Mosa.Compiler.Framework.CIL.BranchInstruction), Mosa.Compiler.Framework.CIL.OpCode.Br, targets);

//                        // Assign block index to instruction
//                        instruction.Block = block.Index;

//                        // Add JUMP instruction to the next block
//                        block.Instructions.Add(instruction);

//                        return true;
//                    }
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to remove self cycle block.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToRemoveSelfCycleBlock(BasicBlock block)
//        {
//            if ((block.PreviousBlocks.Count == 1) && (block.NextBlocks.Count == 1))
//                if (block.NextBlocks[0] == block)
//                    if (block.PreviousBlocks[0] == block)
//                    {
//                        block.Instructions.Clear();
//                        block.NextBlocks.Clear();
//                        block.PreviousBlocks.Clear();
//                    }

//            return false;
//        }

//        /// <summary>
//        /// Tries to remove empty block.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToRemoveEmptyBlock(BasicBlock block)
//        {
//            if ((block.Instructions.Count == 1) && (block != prologue) && (block != epilogue) && (block.NextBlocks.Count != 0))
//            {
//                // Sanity check, a block with one instruction (which should be a JUMP) can only have one branch
//                //Debug.Assert(block.NextBlocks.Count == 1);

//                // Mark Blocks for review in second pass
//                MarkRelatedBlocksForReview(block, false);

//                foreach (BasicBlock previousBlock in block.PreviousBlocks)
//                {
//                    // Sanity check, previous.next == block!
//                    Debug.Assert(previousBlock.NextBlocks.Contains(block));

//                    // Sanity check, last instruction must have an IBranchInstruction interface
//                    Debug.Assert(previousBlock.LastInstruction is IBranchInstruction);

//                    // Remove all references to this block (which is being bypassed)
//                    while (previousBlock.NextBlocks.Remove(block)) ;

//                    // Add the next block to the previous block (thereby bypassing this block)
//                    previousBlock.NextBlocks.Add(block.NextBlocks[0]);

//                    // Get the last instruction in the previous block
//                    IBranchInstruction lastInstruction = previousBlock.LastInstruction as IBranchInstruction;

//                    // Replace targets labels in the last instruction in the previous block with new label
//                    for (int i = lastInstruction.BranchTargets.Length - 1; i >= 0; --i)
//                    {
//                        if (lastInstruction.BranchTargets[i] == block.Label)
//                            lastInstruction.BranchTargets[i] = block.NextBlocks[0].Label;
//                    }

//                    // Add the previous block to the next Blocks
//                    if (!block.NextBlocks[0].PreviousBlocks.Contains(previousBlock))
//                        block.NextBlocks[0].PreviousBlocks.Add(previousBlock);
//                }

//                // Remove this block from all the next Blocks
//                while (block.NextBlocks[0].PreviousBlocks.Remove(block)) ;

//                // Clear out this block
//                block.Instructions.Clear();
//                block.PreviousBlocks.Clear();
//                block.NextBlocks.Clear();
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to combine Blocks.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToCombineBlocks(BasicBlock block)
//        {
//            if ((block.NextBlocks.Count == 1) && (block != prologue))
//            {
//                if ((block.NextBlocks[0].PreviousBlocks.Count == 1) && (block.NextBlocks[0] != epilogue))
//                {
//                    // Merge next block into current block
//                    BasicBlock nextBlock = block.NextBlocks[0];

//                    // Sanity check
//                    Debug.Assert(nextBlock.PreviousBlocks[0] == block);

//                    // Sanity check
//                    Debug.Assert(block.LastInstruction is IBranchInstruction);

//                    // Mark Blocks for review in second pass
//                    MarkRelatedBlocksForReview(block, false);

//                    // Remove last instruction of current block
//                    block.Instructions.RemoveAt(block.Instructions.Count - 1);

//                    // Copy instructions from next block into the current block
//                    foreach (LegacyInstruction instruction in nextBlock.Instructions)
//                    {
//                        instruction.Block = block.Index;
//                        block.Instructions.Add(instruction);
//                    }

//                    // Copy block list from next block to the current block
//                    block.NextBlocks.Clear();
//                    foreach (BasicBlock next in nextBlock.NextBlocks)
//                    {
//                        if (!block.NextBlocks.Contains(next))
//                            block.NextBlocks.Add(next);

//                        // Also, remove all references to nextblock (which is being bypassed)
//                        while (next.PreviousBlocks.Remove(nextBlock)) ;

//                        // Add this block to the children of next block's
//                        if (!next.PreviousBlocks.Contains(block))
//                            next.PreviousBlocks.Add(block);
//                    }

//                    // Clear out the next block
//                    nextBlock.Instructions.Clear();
//                    nextBlock.PreviousBlocks.Clear();
//                    nextBlock.NextBlocks.Clear();

//                    //if (!(block.LastInstruction is IBranchInstruction))
//                    //	Console.WriteLine();

//                    // Sanity check
//                    //Debug.Assert(block.LastInstruction is IBranchInstruction);

//                    return true;
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to the hoist a branch.
//        /// </summary>
//        /// <param name="block">The block.</param>
//        /// <returns></returns>
//        protected bool TryToHoistBranch(BasicBlock block)
//        {
//            if ((block.NextBlocks.Count == 1) && (block != prologue))
//            {
//                if ((block.NextBlocks[0].Instructions.Count == 1) && (block.NextBlocks[0] != epilogue))
//                {
//                    // Copy instruction from next block into current block

//                    // Sanity check, last instruction must have an IBranchInstruction interface
//                    Debug.Assert(block.NextBlocks[0].LastInstruction is IBranchInstruction);

//                    // Mark Blocks for review in second pass
//                    MarkRelatedBlocksForReview(block, true);

//                    // Remove last instruction of this block
//                    block.Instructions.RemoveAt(block.Instructions.Count - 1);

//                    // Get the only instruction in the next block
//                    LegacyInstruction lastInstruction = block.NextBlocks[0].Instructions[0];

//                    // Get the branch instruction interface
//                    IBranchInstruction lastBranchInstruction = lastInstruction as IBranchInstruction;

//                    // Clone the last instruction w/ targets
//                    LegacyInstruction clonedInstruction = CloneBranchInstruction(lastBranchInstruction);

//                    // Assign cloned instruction to this block
//                    clonedInstruction.Block = block.Index;

//                    // Add the cloned instruction to the this block
//                    block.Instructions.Add(clonedInstruction);

//                    // Remove this block from the next block
//                    block.NextBlocks[0].PreviousBlocks.Remove(block);

//                    // Add the next block's next block list to this block
//                    foreach (BasicBlock nextBlock in block.NextBlocks[0].NextBlocks)
//                        block.NextBlocks.Add(nextBlock);

//                    // Remove next block from this block
//                    block.NextBlocks.Remove(block.NextBlocks[0]);

//                    return true;
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// Clones the branch instruction
//        /// </summary>
//        /// <param name="branchInstruction">The branch instruction.</param>
//        /// <returns></returns>
//        protected LegacyInstruction CloneBranchInstruction(IBranchInstruction branchInstruction)
//        {
//            LegacyInstruction result;
//            if (branchInstruction is CIL.BinaryBranchInstruction)
//            {
//                result = arch.CreateInstruction(
//                       typeof(CIL.BinaryBranchInstruction),
//                       ((CIL.BinaryBranchInstruction)branchInstruction).Code
//                );
//            }
//            else if (branchInstruction is CIL.LeaveInstruction)
//            { // Must be before IL.BranchInstruction
//                result = arch.CreateInstruction(
//                       typeof(CIL.LeaveInstruction),
//                       ((CIL.LeaveInstruction)branchInstruction).Code
//                );
//            }
//            else if (branchInstruction is IL.BranchInstruction)
//            {
//                result = arch.CreateInstruction(
//                       typeof(IL.BranchInstruction),
//                       ((CIL.BranchInstruction)branchInstruction).Code
//                );
//            }
//            else if (branchInstruction is CIL.ReturnInstruction)
//            {
//                result = arch.CreateInstruction(
//                       typeof(CIL.ReturnInstruction),
//                       ((CIL.ReturnInstruction)branchInstruction).Code
//                );
//            }
//            else if (branchInstruction is CIL.SwitchInstruction)
//            { // Must be before IL.UnaryBranchInstruction
//                result = arch.CreateInstruction(
//                       typeof(CIL.SwitchInstruction),
//                       ((CIL.SwitchInstruction)branchInstruction).Code
//                );
//            }
//            else if (branchInstruction is CIL.UnaryBranchInstruction)
//            {
//                result = arch.CreateInstruction(
//                       typeof(CIL.UnaryBranchInstruction),
//                       ((CIL.UnaryBranchInstruction)branchInstruction).Code
//                );
//            }
//            else
//            {
//                throw new NotImplementedException();
//            }

//            (result as IBranchInstruction).BranchTargets = branchInstruction.BranchTargets;
//            return result;
//        }

//        #endregion // Methods
//    }
//}
