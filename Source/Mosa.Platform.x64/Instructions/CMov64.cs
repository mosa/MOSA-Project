// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// CMov64
/// </summary>
/// <seealso cref="Mosa.Compiler.x64.X64Instruction" />
public sealed class CMov64 : X64Instruction
{
	internal CMov64()
		: base(1, 2)
	{
	}

	public override string AlternativeName => "CMov";

	public override bool IsZeroFlagUsed => true;

	public override bool IsCarryFlagUsed => true;

	public override bool IsSignFlagUsed => true;

	public override bool IsOverflowFlagUsed => true;

	public override bool IsParityFlagUsed => true;

	public override bool AreFlagUseConditional => true;

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b1);
		opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(node.Operand2.Register.RegisterCode >> 3);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
	}
}
