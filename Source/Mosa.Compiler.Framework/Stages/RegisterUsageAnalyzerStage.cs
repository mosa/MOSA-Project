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
		protected BitArray traversed;
		protected RegisterBitmap[] usage;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			//Debug.WriteLine("METHOD: " + this.methodCompiler.Method.ToString());

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


			Debug.WriteLine("METHOD: " + this.methodCompiler.Method.ToString());
			Debug.WriteLine(string.Empty);

			var registers = new Dictionary<int, Register>();
			foreach (var register in architecture.RegisterSet)
				registers.Add(register.Index, register);

			for (int l = 0; l < 3; l++)
			{
				Debug.Write("[");
				for (int r = 15; r >= 0; r--)
				{
					Debug.Write(registers[r].ToString()[l]);
				}

				Debug.WriteLine("]");
			}

			foreach (var block in this.basicBlocks)
			{
				Debug.WriteLine(String.Format("Block #{0} - Label L_{1:X4}", block.Index, block.Label));

				Debug.WriteLine("[" + top[block.Sequence].ToString().Substring(64 - 16, 16) + "] Top");

				for (var ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Ignore || ctx.Instruction == null)
						continue;

					Debug.Write("[" + usage[ctx.Index].ToString().Substring(64 - 16, 16) + "] ");
					Debug.WriteLine(String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));
				}

				Debug.WriteLine("[" + bottom[block.Sequence].ToString().Substring(64 - 16, 16) + "] Bottom");
				Debug.WriteLine(string.Empty);
			}

			return;
		}
		protected void AnalyzeBlock(BasicBlock block)
		{
			if (analyzed.Get(block.Sequence))
				return;

			//Debug.WriteLine(String.Format("Analyzing Block #{0} - Label L_{1:X4}", block.Index, block.Label));

			traversed.Set(block.Sequence, true);

			// Analysis all child blocks first
			foreach (BasicBlock nextBlock in block.NextBlocks)
			{
				if (!traversed.Get(nextBlock.Sequence))
					AnalyzeBlock(nextBlock);
			}

			//Debug.WriteLine(String.Format("Working Block #{0} - Label L_{1:X4}", block.Index, block.Label));

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
					used.Or(top[nextBlock.Sequence]);
			}

			bottom[block.Sequence] = used;

			var ctx = new Context(instructionSet, block);
			ctx.GotoLast();

			for (; ; ctx.GotoPrevious())
			{
				if (ctx.Ignore || ctx.Instruction == null)
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
