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
	public abstract class CodeTransformationStage : BaseStage, IMethodCompilerStage, IVisitor
	{

		#region Data members

		/// <summary>
		/// The architecture of the compilation process.
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the executing method compiler.
		/// </summary>
		protected IMethodCompiler Compiler;

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
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Save the architecture & compiler
			Architecture = compiler.Architecture;
			Compiler = compiler;

			for (int index = 0; index < BasicBlocks.Count; index++)
				for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
					ctx.Visit(this);
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public abstract void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline);

		#endregion // IMethodCompilerStage Members

		#region Block Operations

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="from">The block issuing the jump.</param>
		/// <param name="to">The block, where From is jumping to.</param>
		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(!from.NextBlocks.Contains(to), @"A link already exists?");
			Debug.Assert(!to.PreviousBlocks.Contains(from), @"A link already exists?");
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
		}

		/// <summary>
		/// Links the new Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="currentBlock">The current block.</param>
		/// <param name="nextBlock">The next block.</param>
		protected void LinkBlocks(Context[] blocks, Context currentBlock, Context nextBlock)
		{
			// Create label to block dictionary
			Dictionary<int, BasicBlock> blockLabels = new Dictionary<int, BasicBlock>();

			foreach (Context ctx in blocks)
				blockLabels.Add(ctx.BasicBlock.Label, ctx.BasicBlock);

			AddBlockLabels(blockLabels, nextBlock.BasicBlock);
			AddBlockLabels(blockLabels, currentBlock.BasicBlock);
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

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateEmptyBlockContext()
		{
			Context ctx = new Context(InstructionSet, -1);
			BasicBlock block = new BasicBlock(BasicBlocks.Count + 0x10000000);
			block.Index = BasicBlocks.Count;
			ctx.BasicBlock = block;
			BasicBlocks.Add(block);

			// FIXME - Add dummy start of block instruction - so we have an instruction index that never moves
			// ??

			return ctx;
		}

		/// <summary>
		/// Creates empty Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateEmptyBlockContexts(int blocks)
		{
			// Allocate the block array
			Context[] result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlockContext();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		protected Context SplitContext(Context ctx)
		{
			int label = BasicBlocks.Count + 0x10000000;

			BasicBlock nextBlock = new BasicBlock(label);
			nextBlock.Index = BasicBlocks.Count - 1;

			foreach (BasicBlock block in ctx.BasicBlock.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.BasicBlock.NextBlocks.Clear();
			ctx.BasicBlock.NextBlocks.Add(nextBlock);

			ctx.InsertInstructionAfter(IR2.Instruction.JmpInstruction, nextBlock);
			ctx.SliceAfter();

			return new Context(nextBlock);
		}

		#endregion

	}
}
