// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Mulss
/// </summary>
/// <seealso cref="Mosa.Compiler.x86.X86Instruction" />
public sealed class Mulss : X86Instruction
{
	internal Mulss()
		: base(1, 2)
	{
	}

	public override bool IsCommutative => true;

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		opcodeEncoder.Append8Bits(0xF3);
		opcodeEncoder.Append8Bits(0x0F);
		opcodeEncoder.Append8Bits(0x59);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
	}
}
