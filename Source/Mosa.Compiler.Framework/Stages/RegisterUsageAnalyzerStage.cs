/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Stage used for investigating operand usage in MOSA (experimental)
	/// </summary>
	public class RegisterUsageAnalyzerStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		protected RegisterBitmap[] top;
		protected RegisterBitmap[] bottom;
		protected BitArray analyzed;
		protected RegisterBitmap[] usage;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Initialize arrays
			top = new RegisterBitmap[basicBlocks.Count];
			bottom = new RegisterBitmap[basicBlocks.Count];
			analyzed = new BitArray(basicBlocks.Count);
			usage = new RegisterBitmap[instructionSet.Size];

			foreach (var block in basicBlocks)
			{
				if (block.NextBlocks.Count == 0)
					AnalyzeBlock(block);
			}



		}

		protected void AnalyzeBlock(BasicBlock block)
		{
			if (analyzed.Get(block.Index))
				return;

			foreach (BasicBlock nextBlock in block.NextBlocks)
				AnalyzeBlock(nextBlock);

			RegisterBitmap used = new RegisterBitmap();

			if (block.NextBlocks.Count == 0)
			{
				if (block.Label == Int32.MaxValue)
				{
					used.Set(architecture.StackFrameRegister);
					used.Set(architecture.StackPointerRegister);

					used.Set(architecture.CallingConvention.GetReturnRegisters(methodCompiler.Method.Signature.ReturnType.Type));
				}
			}
			else
			{
				foreach (BasicBlock nextBlock in block.NextBlocks)
					used.Or(top[nextBlock.Index]);
			}

			bottom[block.Index] = used;

			var ctx = new Context(instructionSet, block);
			ctx.GotoLast();

			for (; ; ctx.GotoPrevious())
			{
				if (ctx.Ignore || ctx.Instruction == null)
					continue;

				RegisterBitmap inputRegisters = GetRegisterInputUsage(ctx);

				// TODO: ADD MAGIC HERE

				usage[block.Index] = used;

				if (ctx.IsFirstInstruction)
					break;
			}

			top[block.Index] = used;
			analyzed.Set(block.Index, true);
		}

		protected RegisterBitmap GetRegisterInputUsage(Context context)
		{
			BasePlatformInstruction instruction = context.Instruction as BasePlatformInstruction;

			if (instruction == null)
				return new RegisterBitmap();

			IRegisterUsage usage = instruction as IRegisterUsage;

			if (usage == null)
				return new RegisterBitmap();

			return usage.GetInputRegisters(context);
		}

		protected void GetRegisterUsage(Context context, ref RegisterBitmap inputRegisters, ref RegisterBitmap outputRegisters)
		{
			BasePlatformInstruction instruction = context.Instruction as BasePlatformInstruction;

			if (instruction == null)
				return;

			IRegisterUsage usage = instruction as IRegisterUsage;

			if (usage == null)
				return;

			outputRegisters = usage.GetOutputRegisters(context);
			inputRegisters = usage.GetInputRegisters(context);
		}

	}
}
