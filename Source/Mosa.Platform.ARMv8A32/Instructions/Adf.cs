// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions;

/// <summary>
/// Adf - Addition
/// </summary>
/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
public sealed class Adf : ARMv8A32Instruction
{
	internal Adf()
		: base(1, 2)
	{
	}

	public override bool IsCommutative { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b1110);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append1Bit(node.Result.IsR4 ? 0 : 1);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b1110);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append1Bit(node.Result.IsR4 ? 0 : 1);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4BitImmediate(node.Operand2);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
