// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Setcc
/// </summary>
public sealed class Setcc : X64Instruction
{
	internal Setcc()
		: base(1, 0)
	{
	}

	public override string AlternativeName => "Setcc";

	public override bool IsZeroFlagUsed => true;

	public override bool IsCarryFlagUsed => true;

	public override bool IsSignFlagUsed => true;

	public override bool IsOverflowFlagUsed => true;

	public override bool IsParityFlagUsed => true;

	public override bool AreFlagUseConditional => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.StartOpcode();
		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append4Bits(0b1001);
		opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(0b000);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
	}
}
