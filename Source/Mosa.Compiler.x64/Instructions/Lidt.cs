// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// Lidt
/// </summary>
public sealed class Lidt : X64Instruction
{
	internal Lidt()
		: base(0, 1)
	{
	}

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister)
		{
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x01);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b011);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0x01);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append3Bits(0b011);
			opcodeEncoder.Append3Bits(0b101);
			opcodeEncoder.Append64BitImmediate(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
