/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System;

namespace Mosa.Compiler.Framework
{
	//[Obsolete]
	public static class ContextExtension
	{
		//[Obsolete]
		public static Context CreateNewBlock(this InstructionSet instructionSet, BasicBlocks basicBlocks, int label)
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

		//[Obsolete]
		public static Context CreateNewBlock(this InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			var ctx = new Context(instructionSet);

			ctx.AppendInstruction(IRInstruction.BlockStart);
			int start = ctx.Index;
			ctx.AppendInstruction(IRInstruction.BlockEnd);
			int last = ctx.Index;

			var block = basicBlocks.CreateBlockWithAutoLabel(start, last);
			ctx.BasicBlock = block;

			ctx.GotoPrevious();

			return ctx;
		}
	}
}
