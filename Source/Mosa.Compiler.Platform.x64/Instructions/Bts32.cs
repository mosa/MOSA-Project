// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Instructions
{
	/// <summary>
	/// Bts32
	/// </summary>
	/// <seealso cref="X64Instruction" />
	public sealed class Bts32 : X64Instruction
	{
		internal Bts32()
			: base(1, 2)
		{
		}

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagUnchanged { get { return true; } }

		public override bool IsSignFlagUndefined { get { return true; } }

		public override bool IsOverflowFlagUnchanged { get { return true; } }

		public override bool IsOverflowFlagUndefined { get { return true; } }

		public override bool IsParityFlagUnchanged { get { return true; } }

		public override bool IsParityFlagUndefined { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);
			System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

			if (node.Operand2.IsCPURegister)
			{
				opcodeEncoder.SuppressByte(0x40);
				opcodeEncoder.Append4Bits(0b0100);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3));
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit((node.Operand2.Register.RegisterCode >> 3));
				opcodeEncoder.Append8Bits(0x0F);
				opcodeEncoder.Append8Bits(0xBA);
				opcodeEncoder.Append2Bits(0b11);
				opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			if (node.Operand2.IsConstant)
			{
				opcodeEncoder.SuppressByte(0x40);
				opcodeEncoder.Append4Bits(0b0100);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3));
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append1Bit(0b0);
				opcodeEncoder.Append8Bits(0x0F);
				opcodeEncoder.Append8Bits(0xAB);
				opcodeEncoder.Append2Bits(0b11);
				opcodeEncoder.Append3Bits(0b011);
				opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				opcodeEncoder.Append8BitImmediate(node.Operand2);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
