// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Pextrd32
/// </summary>
public sealed class Pextrd32 : X64Instruction
{
	internal Pextrd32()
		: base(1, 2)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
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
