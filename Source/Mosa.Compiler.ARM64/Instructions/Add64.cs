// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Instructions;

/// <summary>
/// Add64 - Add with carry
/// </summary>
public sealed class Add64 : ARM64Instruction
{
	internal Add64()
		: base(1, 2)
	{
	}

	public override bool IsCommutative => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsSetFlags ? 1 : 0);
			opcodeEncoder.Append8Bits(0b01011001);
			opcodeEncoder.Append5Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append5Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append5Bits(node.Result.Register.RegisterCode);
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant && node.Operand2.ConstantUnsigned32 >= 0 && node.Operand2.ConstantUnsigned32 <= 4294967295)
		{
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsSetFlags ? 1 : 0);
			opcodeEncoder.Append4Bits(0b1000);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append12BitImmediate(node.Operand2);
			opcodeEncoder.Append5Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append5Bits(node.Result.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
