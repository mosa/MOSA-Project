// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// RdMSR
/// </summary>
public sealed class RdMSR : X86Instruction
{
	internal RdMSR()
		: base(2, 1)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 2);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		opcodeEncoder.StartOpcode();
		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append8Bits(0x32);
	}
}
