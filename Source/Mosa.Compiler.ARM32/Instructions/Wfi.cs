// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// Wfi - Wait for Interrupt
/// </summary>
public sealed class Wfi : ARM32Instruction
{
	internal Wfi()
		: base(1, 3)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 3);

		opcodeEncoder.StartOpcode();
		opcodeEncoder.Append32Bits(0x00000000);
	}
}
