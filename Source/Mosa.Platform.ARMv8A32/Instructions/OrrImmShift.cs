// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// OrrImmShift - Or
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class OrrImmShift : ARMv8A32Instruction
	{
		internal OrrImmShift()
			: base(1, 4)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 4);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister && node.Operand3.IsConstant && node.GetOperand(3).IsConstant)
			{
				opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				opcodeEncoder.Append2Bits(0b00);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(0b1100);
				opcodeEncoder.Append1Bit(node.StatusRegister == StatusRegister.Set ? 1 : 0);
				opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append5BitImmediate(node.Operand3);
				opcodeEncoder.Append2BitImmediate(node.GetOperand(3));
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
