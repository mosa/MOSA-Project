// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions;

/// <summary>
/// Pop32
/// </summary>
/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
public sealed class Pop32 : X86Instruction
{
	internal Pop32()
		: base(1, 0)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append4Bits(0b0101);
		opcodeEncoder.Append1Bit(0b1);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
	}
}
