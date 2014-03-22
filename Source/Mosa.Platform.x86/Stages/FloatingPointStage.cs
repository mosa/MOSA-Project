/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public class FloatingPointStage : BaseTransformationStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var context = new Context(this.InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty || !(context.Instruction is X86Instruction))
						continue;

					if (context.Instruction == X86.Jmp || context.Instruction == X86.FarJmp)
						continue;

					// Convert any floating point constants into labels
					EmitFloatingPointConstants(context);

					// No floating point opcode allows both the result and operand to be a memory location
					// if necessary, load into register first
					if (context.OperandCount == 1
						&& context.ResultCount == 1
						&& context.Operand1.IsMemoryAddress
						&& context.Result.IsMemoryAddress
						&& (context.Result.IsR || context.Operand1.IsR))
					{
						LoadFirstOperandIntoRegister(context);
					}
					else
						// No two-operand floating point opcode allows the first operand to a memory operand
						if (context.OperandCount == 2 && context.Operand1.IsMemoryAddress && context.Operand1.IsR)
						{
							if (IsCommutative(context.Instruction))
							{
								// swap operands
								var t = context.Operand2;
								context.Operand2 = context.Operand1;
								context.Operand1 = t;
							}
							else
							{
								LoadFirstOperandIntoRegister(context);
							}
						}
				}
			}
		}

		private void LoadFirstOperandIntoRegister(Context context)
		{
			// load into a register
			Operand operand = context.Operand1;
			Operand result = context.Result;

			Operand register = AllocateVirtualRegister(operand.Type);
			context.Operand1 = register;

			context.InsertBefore().SetInstruction(GetMove(register, operand), register, operand);
		}

		private bool IsCommutative(BaseInstruction instruction)
		{
			return (instruction == X86.Addsd || instruction == X86.Addss || instruction == X86.Mulsd || instruction == X86.Mulss);
		}
	}
}