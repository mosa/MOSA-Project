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

					RegisterBitmap resultRegisters = GetResultAssignment(ctx);
					RegisterBitmap operandRegisters = GetOperandAssignments(ctx);

					Debug.WriteLine(String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));

					if (resultRegisters.HasValue)
					{
						Debug.Write("\t ASSIGN: ");
						Debug.Write(GetRegisterNames(resultRegisters));
					}

					if (operandRegisters.HasValue)
					{
						Debug.Write("\t USE: ");

						Debug.Write(GetRegisterNames(operandRegisters));
					}

					if (resultRegisters.HasValue | operandRegisters.HasValue)
					{
						Debug.WriteLine("");
					}

					continue;
				}
			}
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

		protected RegisterBitmap GetResultAssignment(Context ctx)
		{
			RegisterBitmap registers = new RegisterBitmap();

			RegisterOperand regOperand = ctx.Result as RegisterOperand;

			if (regOperand != null)
				registers.Set(regOperand.Register);

			return registers;
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

		protected RegisterBitmap GetOperandAssignments(Context ctx)
		{
			RegisterBitmap registers = new RegisterBitmap();

			registers.Set(GetRegister(ctx.Operand1));
			registers.Set(GetRegister(ctx.Operand2));
			registers.Set(GetRegister(ctx.Operand3));
			registers.Set(GetResultUseRegister(ctx.Result));

			return registers;
		}

	}
}
