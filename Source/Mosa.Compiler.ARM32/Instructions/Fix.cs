// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// Fix - Convert Floating-Point to Integer
/// </summary>
public sealed class Fix : ARM32Instruction
{
	internal Fix()
		: base(1, 1)
	{
	}

	public override bool IsCommutative => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		opcodeEncoder.StartOpcode();
		if (node.Operand1.IsPhysicalRegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b1110);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
