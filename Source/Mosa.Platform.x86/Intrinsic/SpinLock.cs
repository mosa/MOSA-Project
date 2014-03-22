/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a spin lock
	/// </summary>
	public sealed class SpinLock : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		private Context Split(Context ctx, BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			Context current = ctx.Clone();

			Context next = ctx.Clone();
			next.AppendInstruction(IRInstruction.BlockStart);
			BasicBlock nextBlock = basicBlocks.CreateBlockWithAutoLabel(next.Index, current.BasicBlock.EndIndex);
			Context nextContext = new Context(instructionSet, nextBlock);

			foreach (BasicBlock block in current.BasicBlock.NextBlocks)
			{
				nextBlock.NextBlocks.Add(block);
				block.PreviousBlocks.Remove(current.BasicBlock);
				block.PreviousBlocks.Add(nextBlock);
			}

			current.BasicBlock.NextBlocks.Clear();

			current.AppendInstruction(IRInstruction.BlockEnd);
			current.BasicBlock.EndIndex = current.Index;

			return nextContext;
		}

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			// Get blocks
			Context nextBlock = Split(context, methodCompiler.BasicBlocks, methodCompiler.InstructionSet);
			Context retryCtx = methodCompiler.InstructionSet.CreateNewBlock(methodCompiler.BasicBlocks);

			// Create constant operand and pointer to variable containing the lock
			Operand const1 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x1);
			Operand locked = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Pointer, context.Operand1, 0);

			// Jump to retryCtx
			context.SetInstruction(X86.Jmp, retryCtx.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(context.BasicBlock, retryCtx.BasicBlock);

			// Test to acquire lock, if cant acquire jump to top, if we do then jump to nextBlock and continue normal execution
			// context.Operand1 is the address for the lock variable
			retryCtx.SetInstruction(X86.Test, locked, const1);
			retryCtx.AppendInstruction(X86.Branch, ConditionCode.NotZero, retryCtx.BasicBlock);
			retryCtx.AppendInstruction(X86.Mov, locked, const1);
			retryCtx.AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(retryCtx.BasicBlock, nextBlock.BasicBlock);
		}

		#endregion Methods
	}
}