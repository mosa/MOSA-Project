// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Instructions
{
	/// <summary>
	/// Popcnt32
	/// </summary>
	/// <seealso cref="X86Instruction" />
	public sealed class Popcnt32 : X86Instruction
	{
		internal Popcnt32()
			: base(1, 1)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override bool IsZeroFlagUnchanged { get { return true; } }

		public override bool IsZeroFlagUndefined { get { return true; } }

		public override bool IsCarryFlagCleared { get { return true; } }

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagCleared { get { return true; } }

		public override bool IsSignFlagModified { get { return true; } }

		public override bool IsOverflowFlagCleared { get { return true; } }

		public override bool IsOverflowFlagModified { get { return true; } }

		public override bool IsParityFlagCleared { get { return true; } }

		public override bool IsParityFlagModified { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0xB8);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		}
	}
}
