/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework
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

					Debug.WriteLine(String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));

					Register register = GetResultAssignment(ctx);

					if (register != null)
					{
						Debug.Write("\t ASSIGN: ");
						Debug.Write(register.ToString());
					}

					Register[] registers = GetOperandAssignments(ctx);

					if (registers != null)
					{
						Debug.Write("\t USE: ");

						foreach (var reg in registers)
							Debug.Write(reg.ToString() + " ");
					}

					if (register != null | registers != null)
					{
						Debug.WriteLine("");
					}

					continue;
				}
			}
		}

		protected Register GetResultAssignment(Context ctx)
		{
			RegisterOperand regOperand = ctx.Result as RegisterOperand;

			if (regOperand != null)
				return regOperand.Register;

			return null;
		}

		protected Register GetRegister(Operand operand)
		{
			if (operand == null)
				return null;

			RegisterOperand regOperand = operand as RegisterOperand;

			if (regOperand != null)
				return regOperand.Register;

			MemoryOperand memOperand = operand as MemoryOperand;

			if (memOperand != null)
				return memOperand.Base;

			ParameterOperand paramOperand = operand as ParameterOperand;

			if (paramOperand != null)
				return paramOperand.Base;

			return null;
		}

		protected Register GetResultUseRegister(Operand operand)
		{
			if (operand == null)
				return null;

			MemoryOperand memOperand = operand as MemoryOperand;

			if (memOperand != null)
				return memOperand.Base;

			ParameterOperand paramOperand = operand as ParameterOperand;

			if (paramOperand != null)
				return paramOperand.Base;

			return null;
		}

		protected Register[] GetOperandAssignments(Context ctx)
		{
			Register operand1 = GetRegister(ctx.Operand1);
			Register operand2 = GetRegister(ctx.Operand2);
			Register operand3 = GetRegister(ctx.Operand3);
			Register operand4 = GetResultUseRegister(ctx.Result);

			if (operand1 == operand2)
				operand2 = null;

			if (operand1 == operand3)
				operand3 = null;

			if (operand1 == operand4)
				operand4 = null;

			if (operand2 == operand3)
				operand3 = null;

			if (operand2 == operand4)
				operand4 = null;

			if (operand3 == operand4)
				operand4 = null;

			int operandCnt = (operand1 == null ? 0 : 1) + (operand2 == null ? 0 : 1) + (operand3 == null ? 0 : 1) + (operand4 == null ? 0 : 1);

			if (operandCnt == 0)
				return null;

			Register[] registers = new Register[operandCnt];

			if (operand4 != null)
				registers[--operandCnt] = operand4;
			if (operand3 != null)
				registers[--operandCnt] = operand3;
			if (operand2 != null)
				registers[--operandCnt] = operand2;
			if (operand1 != null)
				registers[--operandCnt] = operand1;

			return registers;
		}

	}
}
