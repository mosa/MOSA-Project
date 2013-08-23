/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public class FloatPointStage : BaseTransformationStage, IMethodCompilerStage
	{
		/// <summary>
		/// Remove immediate floating point constants - constant must be in registers or memory locations
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (var block in basicBlocks)
			{
				for (var context = new Context(this.instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty || !(context.Instruction is X86Instruction))
						continue;

					if (context.Instruction is Instructions.Jmp || context.Instruction is Instructions.FarJmp)
						continue;

					// Convert any floating point constants into labels
					EmitFloatingPointConstants(context);

					// No floating point opcode allows both the result and operand to be a memory location
					// if necessary, load into register first
					if (!(context.OperandCount == 1 && context.ResultCount == 1 && context.Operand1.IsMemoryAddress && context.Result.IsMemoryAddress))
						continue;

					if (!(context.Result.StackType == StackTypeCode.F || context.Operand1.StackType == StackTypeCode.F))
						continue;

					Operand operand = context.Operand1;
					Operand result = context.Result;

					Operand register = AllocateVirtualRegister(operand.Type);

					context.Operand1 = register;

					context.InsertBefore().SetInstruction(GetMove(register, operand), register, operand);


				}
			}
		}
	}
}