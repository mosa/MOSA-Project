// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Cdq64
/// </summary>
public sealed class Cdq64 : X64Instruction
{
	internal Cdq64()
		: base(1, 1)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		opcodeEncoder.SuppressByte(0x40);
		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b1);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append1Bit(0b0);
		opcodeEncoder.Append8Bits(0x99);

		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
	}
}
