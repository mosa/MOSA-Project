// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// LdrDownImm32 - Single Data Transfer
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class LdrDownImm32 : ARMv8A32Instruction
	{
		internal LdrDownImm32()
			: base(1, 2)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append2Bits(0b01);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append12BitImmediate(node.Operand2);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
