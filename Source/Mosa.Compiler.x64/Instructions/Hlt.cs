// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Hlt
/// </summary>
/// <seealso cref="Mosa.Compiler.x64.X64Instruction" />
public sealed class Hlt : X64Instruction
{
	internal Hlt()
		: base(0, 0)
	{
	}

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append8Bits(0xF4);
	}
}
