// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions;

/// <summary>
/// Roundss
/// </summary>
/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
public sealed class Roundss : X86Instruction
{
	internal Roundss()
		: base(1, 2)
	{
	}

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x66);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x3A);
			opcodeEncoder.Append8Bits(0x0A);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
