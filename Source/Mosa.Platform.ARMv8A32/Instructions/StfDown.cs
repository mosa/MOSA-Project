// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// StfDown - Store Floating Point Data Transfer
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class StfDown : ARMv8A32Instruction
	{
		internal StfDown()
			: base(0, 2)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append3Bits(0b110);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append1Bit(node.Operand3.IsR4 ? 0 : 1);
				opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
				opcodeEncoder.Append4Bits(0b0001);
				opcodeEncoder.Append8Bits(0x00);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
