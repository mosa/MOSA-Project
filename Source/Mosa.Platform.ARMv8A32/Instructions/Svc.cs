// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions;

/// <summary>
/// Svc - Supervisor Call
/// </summary>
/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
public sealed class Svc : ARMv8A32Instruction
{
	internal Svc()
		: base(1, 3)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 3);

		opcodeEncoder.Append32Bits(0x00000000);
	}
}
