// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Branch
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Branch : X64Instruction
{
	internal Branch()
		: base(0, 0)
	{
	}

	public override string AlternativeName { get { return "Jxx"; } }

	public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

	public override bool IsZeroFlagUsed { get { return true; } }

	public override bool IsCarryFlagUsed { get { return true; } }

	public override bool IsSignFlagUsed { get { return true; } }

	public override bool IsOverflowFlagUsed { get { return true; } }

	public override bool IsParityFlagUsed { get { return true; } }

	public override bool AreFlagUseConditional { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append4Bits(0b1000);
		opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
		opcodeEncoder.EmitRelative32(node.BranchTargets[0].Label);
	}
}
