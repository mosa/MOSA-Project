// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// Wfs - Write Floating-Point Status Register
	/// </summary>
	/// <seealso cref="ARMv8A32Instruction" />
	public sealed class Wfs : ARMv8A32Instruction
	{
		internal Wfs()
			: base(1, 1)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			if (node.Operand1.IsCPURegister)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append4Bits(0b1110);
				opcodeEncoder.Append4Bits(0b0010);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(0b0001);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b1);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
