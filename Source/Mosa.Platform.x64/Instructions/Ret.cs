// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Ret
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Ret : X64Instruction
{
	internal Ret()
		: base(0, 0)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsReturn => true;

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append8Bits(0xC3);
	}
}
