// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// PolR8 - Polar Angle
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class PolR8 : ARMv8A32Instruction
	{
		public override int ID { get { return 745; } }

		internal PolR8()
			: base(1, 2)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
			{
				emitter.OpcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				emitter.OpcodeEncoder.Append4Bits(0b1110);
				emitter.OpcodeEncoder.Append4Bits(0b1100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append4Bits(0b0001);
				emitter.OpcodeEncoder.Append1Bit(0b1);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
