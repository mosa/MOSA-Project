// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// MovssStore
/// </summary>
public sealed class MovssStore : X64Instruction
{
	internal MovssStore()
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
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append8Bits(0x00);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 5 && node.Operand2.IsPhysicalRegister && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(node.Operand2.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
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
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsResolvedConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.AppendInteger8(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand1.Register.RegisterCode == 4 && node.Operand2.IsConstant && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b100);
			opcodeEncoder.AppendInteger8(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(node.Operand2.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
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
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsResolvedConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127 && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.AppendInteger8(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.AppendInteger32(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant && node.Operand2.IsConstantZero && node.Operand3.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand3.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0x11);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.AppendInteger32(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
