// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Comisd
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Comisd : X64Instruction
{
	internal Comisd()
		: base(0, 2)
	{
	}

	public override bool IsZeroFlagModified { get { return true; } }

	public override bool IsCarryFlagModified { get { return true; } }

	public override bool IsSignFlagCleared { get { return true; } }

	public override bool IsSignFlagModified { get { return true; } }

	public override bool IsOverflowFlagCleared { get { return true; } }

	public override bool IsOverflowFlagModified { get { return true; } }

	public override bool IsParityFlagModified { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand2.Register.RegisterCode >> 3);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0110);
		opcodeEncoder.Append4Bits(0b0000);
		opcodeEncoder.Append4Bits(0b1111);
		opcodeEncoder.Append4Bits(0b0010);
		opcodeEncoder.Append4Bits(0b1111);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
	}
}
