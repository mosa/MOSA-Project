// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Instructions
{
	/// <summary>
	/// IMul3o32
	/// </summary>
	/// <seealso cref="X86Instruction" />
	public sealed class IMul3o32 : X86Instruction
	{
		internal IMul3o32()
			: base(1, 3)
		{
		}

		public override bool IsZeroFlagUnchanged { get { return true; } }

		public override bool IsZeroFlagUndefined { get { return true; } }

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagUnchanged { get { return true; } }

		public override bool IsSignFlagUndefined { get { return true; } }

		public override bool IsOverflowFlagModified { get { return true; } }

		public override bool IsParityFlagUnchanged { get { return true; } }

		public override bool IsParityFlagUndefined { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 3);
			System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister && (node.Operand3.IsConstant && node.Operand3.ConstantSigned32 >= -128 && node.Operand3.ConstantSigned32 <= 127))
			{
				opcodeEncoder.Append8Bits(0x6B);
				opcodeEncoder.Append2Bits(0b11);
				opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				opcodeEncoder.Append8BitImmediate(node.Operand3);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister && node.Operand3.IsConstant)
			{
				opcodeEncoder.Append8Bits(0x69);
				opcodeEncoder.Append8Bits(0xAF);
				opcodeEncoder.Append2Bits(0b11);
				opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				opcodeEncoder.Append32BitImmediate(node.Operand3);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
