// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// Bx - Branch to target address
/// </summary>
/// <seealso cref="Mosa.Compiler.ARM32.ARM32Instruction" />
public sealed class Bx : ARM32Instruction
{
	internal Bx()
		: base(0, 1)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		if (node.Operand1.IsCPURegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append4Bits(0b0010);
			opcodeEncoder.Append4Bits(0b1111);
			opcodeEncoder.Append4Bits(0b1111);
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode >> 1);
			opcodeEncoder.Append1Bit(0b0);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
