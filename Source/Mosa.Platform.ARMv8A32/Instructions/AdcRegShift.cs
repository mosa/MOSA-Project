// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions;

/// <summary>
/// AdcRegShift - Addition with Carry
/// </summary>
/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
public sealed class AdcRegShift : ARMv8A32Instruction
{
	internal AdcRegShift()
		: base(1, 4)
	{
	}

	public override bool IsCommutative => true;

	public override bool IsCarryFlagUsed => true;

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 4);

		if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister && node.Operand3.IsCPURegister && node.GetOperand(3).IsConstant)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append4Bits(0b0101);
			opcodeEncoder.Append1Bit(node.StatusRegister == StatusRegister.Set ? 1 : 0);
			opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Operand3.Register.RegisterCode);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2BitImmediate(node.GetOperand(3));
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
