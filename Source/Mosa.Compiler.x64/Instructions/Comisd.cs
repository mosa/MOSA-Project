// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Comisd
/// </summary>
public sealed class Comisd : X64Instruction
{
	internal Comisd()
		: base(0, 2)
	{
	}

	public override bool IsZeroFlagModified => true;

	public override bool IsCarryFlagModified => true;

	public override bool IsSignFlagCleared => true;

	public override bool IsSignFlagModified => true;

	public override bool IsOverflowFlagCleared => true;

	public override bool IsOverflowFlagModified => true;

	public override bool IsParityFlagModified => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand2.Register.RegisterCode >> 3);
		opcodeEncoder.Append8Bits(0b01100110);
		opcodeEncoder.Append8Bits(0b00001111);
		opcodeEncoder.Append8Bits(0b00101111);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
	}
}
