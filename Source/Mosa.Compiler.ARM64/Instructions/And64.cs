// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Instructions;

/// <summary>
/// And64 - And
/// </summary>
public sealed class And64 : ARM64Instruction
{
	internal And64()
		: base(1, 2)
	{
	}

	public override bool IsCommutative => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append4Bits(0b0101);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append5Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append5Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append5Bits(node.Result.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
