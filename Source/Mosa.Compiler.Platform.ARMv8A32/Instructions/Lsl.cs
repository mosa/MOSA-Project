// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// Lsl - Shift Left
	/// </summary>
	/// <seealso cref="ARMv8A32Instruction" />
	public sealed class Lsl : ARMv8A32Instruction
	{
		internal Lsl()
			: base(1, 2)
		{
		}

		public override bool IsCarryFlagModified { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(0b1101);
				opcodeEncoder.Append1Bit(node.StatusRegister == StatusRegister.Set ? 1 : 0);
				opcodeEncoder.Append4Bits(0b0000);
				opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(0b1101);
				opcodeEncoder.Append1Bit(node.StatusRegister == StatusRegister.Set ? 1 : 0);
				opcodeEncoder.Append4Bits(0b0000);
				opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append5BitImmediate(node.Operand2);
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
