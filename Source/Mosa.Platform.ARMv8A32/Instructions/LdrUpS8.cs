// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// LdrUpS8 - Byte Data Transfer
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class LdrUpS8 : ARMv8A32Instruction
	{
		internal LdrUpS8()
			: base(1, 2)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append3Bits(0b000);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append4Bits(0b0000);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(node.StatusRegister == StatusRegister.Update ? 1 : 0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
