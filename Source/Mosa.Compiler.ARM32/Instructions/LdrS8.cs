// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// LdrS8 - Byte Data Transfer
/// </summary>
public sealed class LdrS8 : ARM32Instruction
{
	internal LdrS8()
		: base(1, 2)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsUpDirection ? 1 : 0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(node.IsWriteback ? 1 : 0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4BitImmediateHighNibble(node.Operand2);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4BitImmediate(node.Operand2);
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsUpDirection ? 1 : 0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsWriteback ? 1 : 0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
