// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// BochsDebug
/// </summary>
/// <seealso cref="Mosa.Compiler.x86.X86Instruction" />
public sealed class BochsDebug : X86Instruction
{
	internal BochsDebug()
		: base(0, 0)
	{
	}

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 0);

		opcodeEncoder.Append8Bits(0x66);
		opcodeEncoder.Append8Bits(0x87);
		opcodeEncoder.Append8Bits(0xdb);
	}
}
