/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Ir;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// BasicBlockReduction attempts to eliminate useless control flow created as a side effect of other compiler optimizations.
	/// </summary>
	public class BasicBlockReduction : IMethodCompilerStage
	{
		#region Data members

		protected BasicBlock firstBlock;
		protected IArchitecture arch;
		protected BitArray workArray;
		protected Stack<BasicBlock> workList;

		#endregion // Data members

		#region Properties

		#endregion // Properties

		#region IMethodCompilerStage Members

		string IMethodCompilerStage.Name
		{
			get { return @"Basic Block Reduction"; }
		}

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		void IMethodCompilerStage.Run(MethodCompilerBase compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			// Retreive the first block
			firstBlock = blockProvider.FromLabel(-1);

			// Architecture
			arch = compiler.Architecture;

			// Contains a lsit of blocks which need to be review during the second pass
			workArray = new BitArray(blockProvider.Count);
			workList = new Stack<BasicBlock>();

			// Iterate all blocks, remove and/or combine blocks
			// Loop backwards to improve performance and reduce looping
			for (int i = blockProvider.Count - 1; i >= 0; i--) {
				BasicBlock block = blockProvider[i];

				while (ProcessBlock(block)) ;

				workArray.Set(block.Index, false);
			}

			// Pass Two
			while (workList.Count != 0) {
				BasicBlock block = workList.Pop();

				if (workArray.Get(block.Index)) {

					while (ProcessBlock(block)) ;

					workArray.Set(block.Index, false);
				}
			}

		}

		/// <summary>
		/// Processes the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool ProcessBlock(BasicBlock block)
		{
			bool changed = false;

            if (TryToRemoveUnreferencedBlock(block))
				changed = true;

			if (TryToFoldRedundantBranch(block))
				changed = true;

			if (TryToFoldRedundantBranch2(block))
				changed = true;

			if (TryToRemoveSelfCycleBlock(block))
				changed = true;

			if (TryToCombineBlocks(block))
				changed = true;

			//if (TryToRemoveEmptyBlock(block))
			//	changed = true;

			//if (TryToHoistBranch(block))
			//    changed = true;

			return changed;
		}

		/// <summary>
		/// Marks the block for review.
		/// </summary>
		/// <param name="block">The block.</param>
		protected void MarkBlockForReview(BasicBlock block)
		{
			if (!workArray.Get(block.Index)) {
				workArray.Set(block.Index, true);
				workList.Push(block);
			}
		}

		/// <summary>
		/// Marks the blocks for review.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		protected void MarkBlocksForReview(List<BasicBlock> blocks)
		{
			foreach (BasicBlock block in blocks)
				MarkBlockForReview(block);
		}

		/// <summary>
		/// Marks the related blocks for review.
		/// </summary>
		/// <param name="block">The block.</param>
		protected void MarkRelatedBlocksForReview(BasicBlock block, bool self)
		{
			foreach (BasicBlock previousBlock in block.PreviousBlocks)
				MarkBlocksForReview(previousBlock.NextBlocks);

			MarkBlocksForReview(block.PreviousBlocks);
			MarkBlocksForReview(block.NextBlocks);

			foreach (BasicBlock nextBlock in block.NextBlocks)
				MarkBlocksForReview(nextBlock.NextBlocks);

			if (self)
				MarkBlockForReview(block);
		}

		/// <summary>
		/// Tries to remove unused blocks.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToRemoveUnreferencedBlock(BasicBlock block)
		{
			if ((block.PreviousBlocks.Count == 0) && (block != firstBlock) && (block.NextBlocks.Count != 0)) {

				//Mark blocks for review in second pass
				MarkBlocksForReview(block.NextBlocks);

				foreach (BasicBlock nextblock in block.NextBlocks) {
					while (nextblock.PreviousBlocks.Remove(block)) ;
				}

				block.Instructions.Clear();
				block.NextBlocks.Clear();
			}

			return false;
		}

		/// <summary>
		/// Tries to folding redundant branch.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToFoldRedundantBranch(BasicBlock block)
		{
			if (block.NextBlocks.Count == 2) {
				if (block.NextBlocks[0] == block.NextBlocks[1]) {
					// Replace conditional branch instruction with an unconditional jump instruction

					// Mark blocks for review in second pass
					MarkRelatedBlocksForReview(block, true);

					// Remove last instruction of this block
					block.Instructions.RemoveAt(block.Instructions.Count - 1);

					// Create target list
					int[] targets = new int[1];
					targets[0] = block.NextBlocks[0].Label;

					// Create JUMP instruction
					Instruction instruction = arch.CreateInstruction(typeof(Mosa.Runtime.CompilerFramework.IL.BranchInstruction), Mosa.Runtime.CompilerFramework.IL.OpCode.Br, targets);

					// Assign block index to instruction
					instruction.Block = block.Index;

					// Add JUMP instruction to the next block
					block.Instructions.Add(instruction);

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Tries to folding redundant branch.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToFoldRedundantBranch2(BasicBlock block)
		{
			if ((block.NextBlocks.Count != 0) && (block.Instructions.Count != 0)) {
				IBranchInstruction lastInstruction = block.LastInstruction as IBranchInstruction;
				if (lastInstruction.BranchTargets.Length == 2) {
					if (lastInstruction.BranchTargets[0] == lastInstruction.BranchTargets[1]) {
						// Replace conditional branch instruction with an unconditional jump instruction

						// Mark blocks for review in second pass
						MarkRelatedBlocksForReview(block, true);

						// Remove last instruction of this block
						block.Instructions.RemoveAt(block.Instructions.Count - 1);

						// Create target list
						int[] targets = new int[1];
						targets[0] = block.NextBlocks[0].Label;

						// Create JUMP instruction
						Instruction instruction = arch.CreateInstruction(typeof(Mosa.Runtime.CompilerFramework.IL.BranchInstruction), Mosa.Runtime.CompilerFramework.IL.OpCode.Br, targets);

						// Assign block index to instruction
						instruction.Block = block.Index;

						// Add JUMP instruction to the next block
						block.Instructions.Add(instruction);

						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Tries to remove self cycle block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToRemoveSelfCycleBlock(BasicBlock block)
		{
			if ((block.PreviousBlocks.Count == 1) && (block.NextBlocks.Count == 1))
				if (block.NextBlocks[0] == block)
					if (block.PreviousBlocks[0] == block) {

						block.Instructions.Clear();
						block.NextBlocks.Clear();
						block.PreviousBlocks.Clear();
					}

			return false;
		}

		/// <summary>
		/// Tries to remove empty block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToRemoveEmptyBlock(BasicBlock block)
		{
			if ((block.Instructions.Count == 1) && (block != firstBlock)) {
				// Sanity check, a block with one instruction (which should be a JUMP) can only have one branch
				Debug.Assert(block.NextBlocks.Count == 1);

				// Mark blocks for review in second pass
				MarkRelatedBlocksForReview(block, false);

				foreach (BasicBlock previousBlock in block.PreviousBlocks) {
					// Sanity check, previous.next == block!
					Debug.Assert(previousBlock.NextBlocks.Contains(block));

					// Sanity check, last instruction must have an IBranchInstruction interface
					Debug.Assert(previousBlock.LastInstruction is IBranchInstruction);

					// Remove all references to this block (which is being bypassed)
					while (previousBlock.NextBlocks.Remove(block)) ;

					// Add the next block to the previous block (thereby bypassing this block)
					previousBlock.NextBlocks.Add(block.NextBlocks[0]);

					// Get the last instruction in the previous block
					IBranchInstruction lastInstruction = previousBlock.LastInstruction as IBranchInstruction;

					// Replace targets labels in the last instruction in the previous block with new label
					for (int i = lastInstruction.BranchTargets.Length - 1; i >= 0; --i) {
						if (lastInstruction.BranchTargets[i] == block.Label)
							lastInstruction.BranchTargets[i] = block.NextBlocks[0].Label;
					}

					// Add the previous block to the next blocks
					if (!block.NextBlocks[0].PreviousBlocks.Contains(previousBlock))
						block.NextBlocks[0].PreviousBlocks.Add(previousBlock);
				}

				// Remove this block from all the next blocks
				while (block.NextBlocks[0].PreviousBlocks.Remove(block)) ;

				// Clear out this block
				block.Instructions.Clear();
				block.PreviousBlocks.Clear();
				block.NextBlocks.Clear();
			}

			return false;
		}

		/// <summary>
		/// Tries to combine blocks.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToCombineBlocks(BasicBlock block)
		{
			if (block.NextBlocks.Count == 1) {
				if (block.NextBlocks[0].PreviousBlocks.Count == 1) {
					// Merge next block into current block

					BasicBlock nextBlock = block.NextBlocks[0];

					// Sanity check
					Debug.Assert(nextBlock.PreviousBlocks[0] == block);

					// Sanity check
					Debug.Assert(block.LastInstruction is IBranchInstruction);

					// Mark blocks for review in second pass
					MarkRelatedBlocksForReview(block, false);

					// Remove last instruction of current block
					block.Instructions.RemoveAt(block.Instructions.Count - 1);

					// Copy instructions from next block into the current block
					foreach (Instruction instruction in nextBlock.Instructions) {
						instruction.Block = block.Index;
						block.Instructions.Add(instruction);
					}

					// Copy next block list from next block to the current block
					block.NextBlocks.Clear();
					foreach (BasicBlock next in nextBlock.NextBlocks) {
						if (!block.NextBlocks.Contains(next))
							block.NextBlocks.Add(next);

						// Also, remove all references to nextblock (which is being bypassed)
						while (next.PreviousBlocks.Remove(nextBlock)) ;

						// Add this block to the children of next block's
						if (!next.PreviousBlocks.Contains(block))
							next.PreviousBlocks.Add(block);
					}

					// Clear out the next block
					nextBlock.Instructions.Clear();
					nextBlock.PreviousBlocks.Clear();
					nextBlock.NextBlocks.Clear();

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Tries to the hoist a branch.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected bool TryToHoistBranch(BasicBlock block)
		{
			if (block.NextBlocks.Count == 1) {
				if (block.NextBlocks[0].Instructions.Count == 1) {
					// Copy instruction from next block into current block

					// Sanity check, last instruction must have an IBranchInstruction interface
					Debug.Assert(block.NextBlocks[0].LastInstruction is IBranchInstruction);

					// Mark blocks for review in second pass
					MarkRelatedBlocksForReview(block, true);

					// Remove last instruction of this block
					block.Instructions.RemoveAt(block.Instructions.Count - 1);

					// Get the only instruction in the next block
					Instruction lastInstruction = block.NextBlocks[0].Instructions[0];

					// Get the branch instruction interface
					IBranchInstruction lastBranchInstruction = lastInstruction as IBranchInstruction;

					// Clone the last instruction w/ targets
					Instruction clonedInstruction = CloneBranchInstruction(lastBranchInstruction);

					// Assign clonsed instruction to this block
					clonedInstruction.Block = block.Index;

					// Add the cloned instruction to the this block
					block.Instructions.Add(clonedInstruction);

					// Remove this block from the next block
					block.NextBlocks[0].PreviousBlocks.Remove(block);

					// Add the next block's next block list to this block
					foreach (BasicBlock nextBlock in block.NextBlocks[0].NextBlocks) {
						block.NextBlocks.Add(nextBlock);
					}

					// Remove next block from this block
					block.NextBlocks.Remove(block.NextBlocks[0]);

					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Clones the branch instruction
		/// </summary>
		/// <param name="branchInstruction">The branch instruction.</param>
		/// <returns></returns>
		protected Instruction CloneBranchInstruction(IBranchInstruction branchInstruction)
		{
			Instruction result;
			if (branchInstruction is IL.BinaryBranchInstruction) {
				result = arch.CreateInstruction(
					   typeof(IL.BinaryBranchInstruction),
					   ((IL.BinaryBranchInstruction)branchInstruction).Code
				);
			}
			else if (branchInstruction is IL.LeaveInstruction) { // Must be before IL.BranchInstruction
				result = arch.CreateInstruction(
					   typeof(IL.LeaveInstruction),
					   ((IL.LeaveInstruction)branchInstruction).Code
				);
			}
			else if (branchInstruction is IL.BranchInstruction) {
				result = arch.CreateInstruction(
					   typeof(IL.BranchInstruction),
					   ((IL.BranchInstruction)branchInstruction).Code
				);
			}
			else if (branchInstruction is IL.ReturnInstruction) {
				result = arch.CreateInstruction(
					   typeof(IL.ReturnInstruction),
					   ((IL.ReturnInstruction)branchInstruction).Code
				);
			}
			else if (branchInstruction is IL.SwitchInstruction) { // Must be before IL.UnaryBranchInstruction
				result = arch.CreateInstruction(
					   typeof(IL.SwitchInstruction),
					   ((IL.SwitchInstruction)branchInstruction).Code
				);
			}
			else if (branchInstruction is IL.UnaryBranchInstruction) {
				result = arch.CreateInstruction(
					   typeof(IL.UnaryBranchInstruction),
					   ((IL.UnaryBranchInstruction)branchInstruction).Code
				);
			}
			else {
				throw new NotImplementedException();
			}
			(result as IBranchInstruction).BranchTargets = branchInstruction.BranchTargets;
			return result;
		}

		#endregion // Methods
	}
}
