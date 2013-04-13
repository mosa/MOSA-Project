/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework
{
	internal static class ContextHelper
	{
		public static Context CreateNewBlockWithContext(InstructionSet instructionSet, BasicBlocks basicBlocks, int label)
		{
			Context ctx = new Context(instructionSet);

			ctx.AppendInstruction(IRInstruction.BlockStart);
			int start = ctx.Index;
			ctx.AppendInstruction(IRInstruction.BlockEnd);
			int last = ctx.Index;

			BasicBlock block = basicBlocks.CreateBlock(label, start, last);
			ctx.BasicBlock = block;

			ctx.GotoPrevious();

			return ctx;
		}

		public static Context CreateNewBlockWithContext(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			Context ctx = new Context(instructionSet);

			ctx.AppendInstruction(IRInstruction.BlockStart);
			int start = ctx.Index;
			ctx.AppendInstruction(IRInstruction.BlockEnd);
			int last = ctx.Index;

			BasicBlock block = basicBlocks.CreateBlockWithAutoLabel(start, last);
			ctx.BasicBlock = block;

			ctx.GotoPrevious();

			return ctx;
		}
	}
}