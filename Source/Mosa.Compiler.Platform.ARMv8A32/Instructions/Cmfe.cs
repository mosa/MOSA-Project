// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// Cmfe - Compare floating with exception compare
	/// </summary>
	/// <seealso cref="ARMv8A32Instruction" />
	public sealed class Cmfe : ARMv8A32Instruction
	{
		internal Cmfe()
			: base(0, 2)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append4Bits(0b1110);
				opcodeEncoder.Append3Bits(0b110);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append4Bits(0b1111);
				opcodeEncoder.Append4Bits(0b0001);
				opcodeEncoder.Append1Bit(node.Operand1.IsR4 ? 0 : 1);
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append4Bits(0b1110);
				opcodeEncoder.Append3Bits(0b110);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append4Bits(0b1111);
				opcodeEncoder.Append4Bits(0b0001);
				opcodeEncoder.Append1Bit(node.Operand1.IsR4 ? 0 : 1);
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append4BitImmediate(node.Operand2);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
