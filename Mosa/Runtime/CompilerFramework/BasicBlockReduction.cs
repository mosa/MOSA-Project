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
	public class BasicBlockReduction
	{
		#region Data members

		protected BasicBlock first;
		protected IArchitecture arch;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes common fields of the BasicBlockReduction.
		/// </summary>
		private BasicBlockReduction()
		{

		}

		#endregion // Construction

		#region Properties

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Run(MethodCompilerBase compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			// Retreive the first block
			first = blockProvider.FromLabel(-1);

			// Architecture
			arch = compiler.Architecture;

			// Iterate all blocks and combine blocks

			// Loop until no changes are made - this can be improved in the future
			bool changed = true;

			do {
				foreach (BasicBlock currentBlock in blockProvider) {
					if (TryFoldingRedundantBranch(currentBlock))
						changed = true;

					if (TryCombineBlocks(currentBlock))
						changed = true;
				}
			}
			while (!changed);
		}

		/// <summary>
		/// Tries to folding redundant branch.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		public bool TryFoldingRedundantBranch(BasicBlock block)
		{
			if (block.NextBlocks.Count == 2) {
				if (block.NextBlocks[0] == block.NextBlocks[1]) {
					// Replace conditional branch instruction with an unconditional jump instruction

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
		/// Tries to the remove an empty block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		public bool TryRemoveEmptyBlock(BasicBlock block)
		{
			if (block.Instructions.Count == 1) {
				// Sanity check, a block with one instruction (which should be a JUMP) can only have one branch
				Debug.Assert(block.NextBlocks.Count == 1);

				foreach (BasicBlock previousBlock in block.PreviousBlocks) {
					// Sanity check, previous.next == block!
					Debug.Assert(previousBlock.NextBlocks.Contains(block));

					// Replace label in previous block with new label
					for (int i = previousBlock.NextBlocks.Count - 1; i >= 0; i--) {
						if (previousBlock.NextBlocks[i].Label == block.Label) {
							previousBlock.NextBlocks[i] = block.NextBlocks[0];
						}
					}

					// Sanity check, last instruction must have an IBranchInstruction interface
					Debug.Assert(previousBlock.Instructions[previousBlock.Instructions.Count - 1] is IBranchInstruction);

					// Get the last instruction in the previous block
					IBranchInstruction lastInstruction = previousBlock.Instructions[previousBlock.Instructions.Count - 1] as IBranchInstruction;

					// Replace targets labels in the last insturction in the previous block with new label
					for (int i = lastInstruction.BranchTargets.Length - 1; i >= 0; i++) {
						if (lastInstruction.BranchTargets[i] == block.Label)
							lastInstruction.BranchTargets[i] = block.NextBlocks[i].Label;
					}

				}

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
		public bool TryCombineBlocks(BasicBlock block)
		{
			if (block.NextBlocks.Count == 1) {
				if (block.NextBlocks[0].PreviousBlocks.Count == 1) {
					BasicBlock nextBlock = block.NextBlocks[0];

					// Sanity check
					Debug.Assert(nextBlock.PreviousBlocks[0] == block);

					// Merge next block into current block
					block.AppendBlock(nextBlock);

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
		public bool TryHoistingBranch(BasicBlock block)
		{
			if (block.NextBlocks.Count == 1) {
				if (block.NextBlocks[0].Instructions.Count == 1) {
					// Copy instruction from next block into current block

					// Sanity check, last instruction must have an IBranchInstruction interface
					Debug.Assert(block.NextBlocks[0].Instructions[0] is IBranchInstruction);

					// Remove last instruction of this block
					block.Instructions.RemoveAt(block.Instructions.Count - 1);

					// Get the last instruction in the next block
					Instruction lastInstruction = block.NextBlocks[0].Instructions[0];

					// Get the last instruction as an interface of a branch instruction
					IBranchInstruction lastBranchInstruction = lastInstruction as IBranchInstruction;

					// Clone the last instruction w/ targets
					//Instruction clonedInstruction = arch.CreateInstruction(typeof(IL.BranchInstruction), (lastInstruction as IL.ILInstruction).Code, lastBranchInstruction.BranchTargets);
					Instruction clonedInstruction = CloneBranchInstruction(lastInstruction);

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
			else if (branchInstruction is IL.SwitchInstruction) { // Must be before IL.SwitchInstruction
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
