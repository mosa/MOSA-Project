// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// MovStore32
/// </summary>
public sealed class MovStore32 : X86Instruction
{
	internal MovStore32()
		: base(0, 3)
	{
	}

	public override bool IsMemoryWrite => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 3);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsConstantZero && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsPhysicalRegister && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstantZero && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append8BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append32BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstantZero && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append8BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstantZero && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediate(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsConstantZero && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsPhysicalRegister && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstantZero && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstantZero && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand2);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstantZero && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediate(node.Operand1);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstant && node.Operand3.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xC7);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediateWithOffset(node.Operand1, node.Operand2);
			opcodeEncoder.Append32BitImmediate(node.Operand3);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstant && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x89);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append32BitImmediateWithOffset(node.Operand1, node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
