// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// MovssLoad
/// </summary>
public sealed class MovssLoad : X86Instruction
{
	internal MovssLoad()
		: base(1, 2)
	{
	}

	public override bool IsMemoryRead => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsConstantZero)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstantZero)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append8BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append8BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstantZero)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append8BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstantZero)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediate(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x10);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediateWithOffset(node.Operand1, node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
