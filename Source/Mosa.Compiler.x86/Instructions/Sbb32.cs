// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Sbb32
/// </summary>
public sealed class Sbb32 : X86Instruction
{
	internal Sbb32()
		: base(1, 2)
	{
	}

	public override bool IsCommutative => true;

	public override bool IsZeroFlagModified => true;

	public override bool IsCarryFlagUsed => true;

	public override bool IsCarryFlagModified => true;

	public override bool IsSignFlagModified => true;

	public override bool IsOverflowFlagModified => true;

	public override bool IsParityFlagModified => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		opcodeEncoder.StartOpcode();
		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 0 && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127)
		{
			opcodeEncoder.Append8Bits(0x83);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b011);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 0 && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x1D);
			opcodeEncoder.Append32BitImmediate(node.Operand2);
			return;
		}

		if (node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x1B);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		if (node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x81);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b011);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand2);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
