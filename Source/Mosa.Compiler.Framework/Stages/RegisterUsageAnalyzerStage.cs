/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
		protected BitArray traversed;
		protected RegisterBitmap[] usage;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			//Trace("METHOD: " + this.methodCompiler.FullName);

			// Initialize arrays
			top = new RegisterBitmap[basicBlocks.Count];
			bottom = new RegisterBitmap[basicBlocks.Count];
			analyzed = new BitArray(basicBlocks.Count);
			traversed = new BitArray(basicBlocks.Count);
			usage = new RegisterBitmap[instructionSet.Size];

			foreach (var block in basicBlocks)
			{
				AnalyzeBlock(block);
			}

			Trace("METHOD: " + this.methodCompiler.Method.FullName);
			Trace(string.Empty);

			var registers = new Dictionary<int, Register>();
			foreach (var register in architecture.RegisterSet)
				registers.Add(register.Index, register);

			StringBuilder line = new StringBuilder();
			for (int l = 0; l < 3; l++)
			{
				line.Append("[");
				for (int r = 15; r >= 0; r--)
				{
					line.Append(registers[r].ToString()[l]);
				}

				line.Append("]");
			}
			Trace(line.ToString());

			foreach (var block in this.basicBlocks)
			{
				Trace(String.Format("Block #{0} - Label L_{1:X4}", block.Index, block.Label));

				Trace("[" + top[block.Sequence].ToString().Substring(64 - 16, 16) + "] Top");

				for (var ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					Trace("[" + usage[ctx.Index].ToString().Substring(64 - 16, 16) + "] " + String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));
				}

				Trace("[" + bottom[block.Sequence].ToString().Substring(64 - 16, 16) + "] Bottom");
				Trace(string.Empty);
			}

			return;
		}
		protected void AnalyzeBlock(BasicBlock block)
		{
			if (analyzed.Get(block.Sequence))
				return;

			//Trace(String.Format("Analyzing Block #{0} - Label L_{1:X4}", block.Index, block.Label));

			traversed.Set(block.Sequence, true);

			// Analysis all child blocks first
			foreach (BasicBlock nextBlock in block.NextBlocks)
			{
				if (!traversed.Get(nextBlock.Sequence))
					AnalyzeBlock(nextBlock);
			}

			//Trace(String.Format("Working Block #{0} - Label L_{1:X4}", block.Index, block.Label));

			RegisterBitmap used = new RegisterBitmap();

			if (block.NextBlocks.Count == 0)
			{
				if (block.Label == Int32.MaxValue)
				{
					used.Set(architecture.StackFrameRegister);
					used.Set(architecture.StackPointerRegister);

					used.Set(architecture.CallingConvention.GetReturnRegisters(methodCompiler.Method.Signature.ReturnType.Type));
					used.Set(architecture.CallingConvention.CalleeSavedRegisters);
				}
			}
			else
			{
				foreach (BasicBlock nextBlock in block.NextBlocks)
					used.Or(top[nextBlock.Sequence]);
			}

			bottom[block.Sequence] = used;

			var ctx = new Context(instructionSet, block);
			ctx.GotoLast();

			for (; ; ctx.GotoPrevious())
			{
				if (ctx.IsEmpty)
					if (ctx.IsFirstInstruction)
						break;
					else
						continue;

				RegisterBitmap inputRegisters = new RegisterBitmap();
				RegisterBitmap outputRegisters = new RegisterBitmap();

				GetRegisterUsage(ctx, ref inputRegisters, ref outputRegisters);

				RegisterBitmap assignment = inputRegisters;
				assignment.Xor(outputRegisters);
				assignment.Not();

				used.And(assignment);

				used.Or(inputRegisters);

				usage[ctx.Index] = used;

				if (ctx.IsFirstInstruction)
					break;
			}

			top[block.Sequence] = used;
			analyzed.Set(block.Sequence, true);
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
