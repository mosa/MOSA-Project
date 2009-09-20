/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class CodeTransformationStage : IMethodCompilerStage, IInstructionVisitor<Context>
	{
		
		#region Data members

		/// <summary>
		/// The architecture of the compilation process.
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the list of basic Blocks.
		/// </summary>
		protected List<BasicBlock> Blocks;

		/// <summary>
		/// Holds the executing method compiler.
		/// </summary>
		protected IMethodCompiler Compiler;

		/// <summary>
		/// Holds the current block.
		/// </summary>
		protected int CurrentBlock;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public abstract string Name { get; }

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
			if (null == compiler)
				throw new ArgumentNullException(@"compiler");
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			if (null == blockProvider)
				throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");

			// Save the architecture & compiler
			Architecture = compiler.Architecture;
			Compiler = compiler;
			Blocks = blockProvider.Blocks;

			for (CurrentBlock = 0; CurrentBlock < Blocks.Count; CurrentBlock++) {
				BasicBlock block = Blocks[CurrentBlock];
				Context ctx = new Context(block);
				//ctx.Block = block;
				for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++) {
					block.Instructions[ctx.Index].Visit(this, ctx);
				}
			}
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public abstract void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline);

		#endregion // IMethodCompilerStage Members

		#region IInstructionVisitor<Context> Members

		/// <summary>
		/// Visitation method for instructions not caught by more specific visitation methods.
		/// </summary>
		/// <param name="instruction">The visiting instruction.</param>
		/// <param name="arg">A visitation context argument.</param>
		public virtual void Visit(Instruction instruction, Context arg)
		{
			Trace.WriteLine(String.Format(@"Unknown instruction {0} has visited stage {1}.", instruction.GetType().FullName, Name));
			throw new NotSupportedException();
		}

		#endregion // IInstructionVisitor<Context> Members

		#region Methods

		/// <summary>
		/// Removes the current instruction From the instruction stream.
		/// </summary>
		/// <param name="ctx">The context of the instruction to remove.</param>
		protected void Remove(Context ctx)
		{
			Remove(ctx.BasicBlock.Instructions[ctx.Index]);
			ctx.BasicBlock.Instructions.RemoveAt(ctx.Index--);
		}

		/// <summary>
		/// Removes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction to remove.</param>
		protected void Remove(Instruction instruction)
		{
			// FIXME: Remove this sequence, once we have a smart instruction collection which does this.
			for (int i = 0; i < instruction.Operands.Length; i++)
				instruction.SetOperand(i, null);
			for (int i = 0; i < instruction.Results.Length; i++)
				instruction.SetResult(i, null);
		}

		/// <summary>
		/// Replaces the currently processed instruction with another instruction.
		/// </summary>
		/// <param name="arg">The transformation context.</param>
		/// <param name="instruction">The instruction to replace with.</param>
		protected void Replace(Context arg, Instruction instruction)
		{
			arg.BasicBlock.Instructions[arg.Index] = instruction;
		}

		/// <summary>
		/// Replaces the currently processed instruction with a set of instruction.
		/// </summary>
		/// <param name="arg">The transformation context.</param>
		/// <param name="instructions">The instructions to replace with.</param>
		protected void Replace(Context arg, IEnumerable<Instruction> instructions)
		{
			List<Instruction> insts = arg.BasicBlock.Instructions;
			insts.RemoveAt(arg.Index);
			int oldCount = insts.Count;
			insts.InsertRange(arg.Index, instructions);
			arg.Index += (insts.Count - oldCount) - 1;
		}

		#endregion // Methods

		#region Block Operations

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="from">The block issuing the jump.</param>
		/// <param name="to">The block, where From is jumping to.</param>
		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(false == from.NextBlocks.Contains(to), @"A link already exists?");
			Debug.Assert(false == to.PreviousBlocks.Contains(from), @"A link already exists?");
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		/// <summary>
		/// Links the new Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="currentBlock">The current block.</param>
		/// <param name="nextBlock">The next block.</param>
		protected void LinkBlocks(BasicBlock[] blocks, BasicBlock currentBlock, BasicBlock nextBlock)
		{
			// Create label to block dictionary
			Dictionary<int, BasicBlock> blockLabels = new Dictionary<int, BasicBlock>();

			foreach (BasicBlock block in blocks)
				blockLabels.Add(block.Label, block);

			AddBlockLabels(blockLabels, nextBlock);	
			AddBlockLabels(blockLabels, currentBlock);
			UpdateBlocks(blocks, blockLabels);
		}
		
		private static void AddBlockLabels(IDictionary<int, BasicBlock> blockLabels, BasicBlock basicBlock)
		{
			if (basicBlock != null) {
				foreach (BasicBlock block in basicBlock.NextBlocks)
					if (!blockLabels.ContainsKey(block.Label))
						blockLabels.Add(block.Label, block);

				if (!blockLabels.ContainsKey(basicBlock.Label))
					blockLabels.Add(basicBlock.Label, basicBlock);
			}
		}
		
		private static void UpdateBlocks(BasicBlock[] blocks, IDictionary<int, BasicBlock> blockLabels)
		{
			foreach (BasicBlock block in blocks)
				foreach (Instruction instruction in block.Instructions)
					if (instruction is IBranchInstruction)
						foreach (int label in (instruction as IBranchInstruction).BranchTargets) 
							UpdateBlockLinks(block, blockLabels[label]);
		}
		
		private static void UpdateBlockLinks(BasicBlock block, BasicBlock next)
		{
			if (!block.NextBlocks.Contains(next))
				block.NextBlocks.Add(next);
			if (!next.PreviousBlocks.Contains(block))
				next.PreviousBlocks.Add(block);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected BasicBlock CreateEmptyBlock()
		{
			BasicBlock block = new BasicBlock(Blocks.Count + 0x10000000);
			block.Index = Blocks.Count;
			Blocks.Add(block);
			return block;
		}

		/// <summary>
		/// Creates empty Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected BasicBlock[] CreateEmptyBlocks(int blocks)
		{
			// Allocate the block array
			BasicBlock[] result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlock();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction index to split on.</param>
		/// <param name="insert">The insert to be called after the split.</param>
		/// <returns></returns>
		protected BasicBlock SplitBlock(Context ctx, Instruction instruction, BasicBlock insert)
		{
			int label = Blocks.Count + 0x10000000;

			BasicBlock nextBlock = ctx.BasicBlock.Split(ctx.Index + 1, label);
			nextBlock.Index = Blocks.Count - 1;
			Blocks.Add(nextBlock);

			foreach (BasicBlock block in ctx.BasicBlock.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.BasicBlock.NextBlocks.Clear();

			if (insert != null) 
				InsertLabel(ref ctx, ref insert, insert.Label);
			else 
				InsertLabel(ref ctx, ref nextBlock, label);

			return nextBlock;
		}
		
		private void InsertLabel(ref Context ctx, ref BasicBlock block, int label)
		{
			ctx.BasicBlock.NextBlocks.Add(block);
			block.PreviousBlocks.Add(ctx.BasicBlock);
			ctx.BasicBlock.Instructions.Add(new IR.JmpInstruction(label));
		}

		#endregion

	}
}
