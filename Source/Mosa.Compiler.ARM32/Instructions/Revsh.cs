// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// Revsh - Byte-Reverse Signed Halfword
/// </summary>
public sealed class Revsh : ARM32Instruction
{
	internal Revsh()
		: base(1, 3)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 3);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		opcodeEncoder.Append32Bits(0x00000000);

		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
	}
}
