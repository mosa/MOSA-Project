/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage, IMethodCompilerStage, IVisitor
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public virtual void Run()
		{
			for (int index = 0; index < basicBlocks.Count; index++)
				for (Context ctx = new Context(instructionSet, basicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						ctx.Clone().Visit(this);
		}

		#endregion // IMethodCompilerStage Members

		#region Block Operations

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, BasicBlock destination)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, Context destination)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, Context destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, BasicBlock destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, BasicBlock destination, BasicBlock destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateEmptyBlockContext(int label)
		{
			Context ctx = new Context(instructionSet);
			BasicBlock block = basicBlocks.CreateBlock();
			ctx.BasicBlock = block;

			// Need a dummy instruction at the start of each block to establish a starting point of the block
			ctx.AppendInstruction(null);
			ctx.Label = label;
			block.Index = ctx.Index;

			return ctx;
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context[] CreateEmptyBlockContexts(int label, int blocks)
		{
			// Allocate the block array
			Context[] result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlockContext(label);

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="addJump">if set to <c>true</c> [add jump].</param>
		/// <returns></returns>
		protected Context SplitContext(Context ctx, bool addJump)
		{
			Context current = ctx.Clone();

			BasicBlock nextBlock = basicBlocks.CreateBlock();

			foreach (BasicBlock block in current.BasicBlock.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			current.BasicBlock.NextBlocks.Clear();

			if (addJump)
			{
				current.BasicBlock.NextBlocks.Add(nextBlock);
				nextBlock.PreviousBlocks.Add(ctx.BasicBlock);
			}

			if (current.IsLastInstruction)
			{
				current.AppendInstruction(null);
				nextBlock.Index = current.Index;
				current.SliceBefore();
			}
			else
			{
				nextBlock.Index = current.Next.Index;
				current.SliceAfter();
			}

			if (addJump)
				current.AppendInstruction(IR.IRInstruction.Jmp, nextBlock);

			return CreateContext(nextBlock);
		}

		#endregion

	}
}
