// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// IDiv32
/// </summary>
public sealed class IDiv32 : X64Instruction
{
	internal IDiv32()
		: base(2, 3)
	{
	}

	public override bool IsZeroFlagUnchanged => true;

	public override bool IsZeroFlagUndefined => true;

	public override bool IsCarryFlagUnchanged => true;

	public override bool IsCarryFlagUndefined => true;

	public override bool IsSignFlagUnchanged => true;

	public override bool IsSignFlagUndefined => true;

	public override bool IsOverflowFlagUnchanged => true;

	public override bool IsOverflowFlagUndefined => true;

	public override bool IsParityFlagUnchanged => true;

	public override bool IsParityFlagUndefined => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 2);
		System.Diagnostics.Debug.Assert(node.OperandCount == 3);
		System.Diagnostics.Debug.Assert(node.Result.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
		opcodeEncoder.Append8Bits(0xF7);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(0b111);
		opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
	}
}
