// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions;

/// <summary>
/// XChg32
/// </summary>
/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
public sealed class XChg32 : X86Instruction
{
	internal XChg32()
		: base(2, 2)
	{
	}

	public override bool IsCommutative { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 2);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		opcodeEncoder.Append8Bits(0x87);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
	}
}
