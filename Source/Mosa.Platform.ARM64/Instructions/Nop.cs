// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Instructions;

/// <summary>
/// Nop - No Operation
/// </summary>
/// <seealso cref="Mosa.Compiler.ARM64.ARM64Instruction" />
public sealed class Nop : ARM64Instruction
{
	internal Nop()
		: base(0, 0)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.AppendShort(0xBF00);
	}
}
