// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Branch
/// </summary>
public sealed class Branch : X86Instruction
{
	internal Branch()
		: base(0, 0)
	{
	}

	public override string AlternativeName => "Jxx";

	public override bool IsFlowNext => false;

	public override bool IsConditionalBranch => true;

	public override bool IsZeroFlagUsed => true;

	public override bool IsCarryFlagUsed => true;

	public override bool IsSignFlagUsed => true;

	public override bool IsOverflowFlagUsed => true;

	public override bool IsParityFlagUsed => true;

	public override bool AreFlagUseConditional => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append4Bits(0b1000);
		opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
		opcodeEncoder.EmitRelative32(node.BranchTargets[0].Label);
	}
}
