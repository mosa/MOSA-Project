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
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Stage used for investigating operand usage in MOSA (experimental)
	/// </summary>
	public class OperandUsageAnalyzerStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Create a list of end blocks
			List<BasicBlock> endBlocks = new List<BasicBlock>();

			foreach (var block in this.basicBlocks)
			{
				if (block.NextBlocks.Count == 0)
					endBlocks.Add(block);
			}

			foreach (var block in this.basicBlocks)
			{
				for (var ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Ignore || ctx.Instruction == null)
						continue;

					RegisterBitmap inputRegisters = new RegisterBitmap();
					RegisterBitmap outputRegisters = new RegisterBitmap();

					GetRegisterUsage(ctx, ref inputRegisters, ref outputRegisters);

					Debug.WriteLine(String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));
					
					if (outputRegisters.HasValue)
					{
						Debug.Write("\t OUTPUT: ");

						Debug.Write(GetRegisterNames(outputRegisters));
					}

					if (inputRegisters.HasValue)
					{
						Debug.Write("\t INPUT: ");
						Debug.Write(GetRegisterNames(inputRegisters));
					}

					if (inputRegisters.HasValue | outputRegisters.HasValue)
					{
						Debug.WriteLine("");
					}

					continue;
				}
			}
		}

		protected void GetRegisterUsage(Context context,  ref RegisterBitmap inputRegisters, ref RegisterBitmap outputRegisters)
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

		public List<Register> GetRegisters(RegisterBitmap registers)
		{
			List<Register> list = new List<Register>();

			foreach (int index in registers)
			{
				list.Add(architecture.RegisterSet[index]);
			}

			return list;
		}

		protected string GetRegisterNames(RegisterBitmap registers)
		{
			StringBuilder list = new StringBuilder();

			foreach (Register register in GetRegisters(registers))
			{
				if (list.Length != 0)
					list.Append(",");

				list.Append(register.ToString());
			}

			return list.ToString();
		}


	}
}
