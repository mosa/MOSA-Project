// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Movdi64sd
/// </summary>
public sealed class Movdi64sd : X64Instruction
{
	internal Movdi64sd()
		: base(1, 1)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b1);
		opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0000);
		opcodeEncoder.Append4Bits(0b1111);
		opcodeEncoder.Append8Bits(0x7E);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
	}
}
