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

			//foreach (var block in this.basicBlocks)
			//{
			//    for (var ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
			//    {
			//        if (ctx.Ignore || ctx.Instruction == null)
			//            continue;

			//        bool odd = false;

			//        if (ctx.Instruction.DefaultResultCount == 0 && ctx.Result != null)
			//            odd = true;
			//        if (ctx.Instruction.DefaultResultCount == 1 && ctx.Result == null)
			//            odd = true;
			//        if (ctx.Instruction.DefaultOperandCount == 0 && ctx.Operand1 != null)
			//            odd = true;
			//        if (ctx.Instruction.DefaultOperandCount == 0 && ctx.Operand2 != null)
			//            odd = true;
			//        if (ctx.Instruction.DefaultOperandCount == 1 && ctx.Operand1 == null)
			//            odd = true;
			//        if (ctx.Instruction.DefaultOperandCount == 1 && ctx.Operand2 != null)
			//            odd = true;

			//        if (ctx.ResultCount == 0 && ctx.Result != null)
			//            odd = true;
			//        if (ctx.ResultCount == 1 && ctx.Result == null)
			//            odd = true;
			//        if (ctx.OperandCount == 0 && ctx.Operand1 != null)
			//            odd = true;
			//        if (ctx.OperandCount == 0 && ctx.Operand2 != null)
			//            odd = true;
			//        if (ctx.OperandCount == 1 && ctx.Operand1 == null)
			//            odd = true;
			//        if (ctx.OperandCount == 1 && ctx.Operand2 != null)
			//            odd = true;
			//        if (ctx.OperandCount == 2 && ctx.Operand1 == null)
			//            odd = true;
			//        if (ctx.OperandCount == 2 && ctx.Operand2 == null)
			//            odd = true;

			//        if (!odd)
			//            continue;

			//        //if (ctx.Instruction.ToString().Contains("X86.Call"))
			//        //    continue;

			//        //if (ctx.Instruction.ToString().Contains("X86.Jmp"))
			//        //    continue;

			//        Debug.WriteLine(String.Format("===> L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));
			//    }
			//}

			if (basicBlocks.Count >= 0)
				return;

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

					RegisterBitmap assignedRegisters = GetResultAssignment(ctx);
					RegisterBitmap usedRegisters = GetOperandAssignments(ctx);

					UpdateOpcodeRegisterUsage(ctx, ref assignedRegisters, ref usedRegisters);

					Debug.WriteLine(String.Format("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx)));

					if (ctx.Instruction.ToString().Contains(".Div"))
						Debug.Write("");

					if (assignedRegisters.HasValue)
					{
						Debug.Write("\t ASSIGNED: ");
						Debug.Write(GetRegisterNames(assignedRegisters));
					}

					if (usedRegisters.HasValue)
					{
						Debug.Write("\t USED: ");

						Debug.Write(GetRegisterNames(usedRegisters));
					}

					if (assignedRegisters.HasValue | usedRegisters.HasValue)
					{
						Debug.WriteLine("");
					}

					continue;
				}
			}
		}

		protected void UpdateOpcodeRegisterUsage(Context context, ref RegisterBitmap assignedRegisters, ref RegisterBitmap usedRegisters)
		{
			BasePlatformInstruction instruction = context.Instruction as BasePlatformInstruction;

			if (instruction == null)
				return;

			IInstructionRegisterUsage usage = instruction.InstructionRegisterUsage;

			if (usage == null)
				return;

			RegisterOperand resultOperand = context.Result as RegisterOperand;

			if (resultOperand != null)
			{

				Register[] modifiedRegister = usage.GetModifiedRegisters(resultOperand.Register);

				if (modifiedRegister != null)
				{
					foreach (Register register in modifiedRegister)
						assignedRegisters.Set(register);
				}

				Register[] unspecifiedRegisters = usage.GetUnspecifiedSourceRegisters(resultOperand.Register);

				if (unspecifiedRegisters != null)
				{
					foreach (Register register in modifiedRegister)
						usedRegisters.Set(register);
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
