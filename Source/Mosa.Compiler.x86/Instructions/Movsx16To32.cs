// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Movsx16To32
/// </summary>
public sealed class Movsx16To32 : X86Instruction
{
	internal Movsx16To32()
		: base(1, 1)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		opcodeEncoder.StartOpcode();
		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append8Bits(0b10111111);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
	}
}
