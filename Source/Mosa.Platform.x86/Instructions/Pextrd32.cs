// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Pextrd32
/// </summary>
/// <seealso cref="Mosa.Compiler.x86.X86Instruction" />
public sealed class Pextrd32 : X86Instruction
{
	internal Pextrd32()
		: base(1, 2)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0000);
		opcodeEncoder.Append4Bits(0b1111);
		opcodeEncoder.Append4Bits(0b0011);
		opcodeEncoder.Append4Bits(0b1010);
		opcodeEncoder.Append4Bits(0b0001);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		opcodeEncoder.Append8BitImmediate(node.Operand2);
	}
}
