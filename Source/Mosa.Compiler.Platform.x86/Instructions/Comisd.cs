// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Instructions
{
	/// <summary>
	/// Comisd
	/// </summary>
	/// <seealso cref="X86Instruction" />
	public sealed class Comisd : X86Instruction
	{
		internal Comisd()
			: base(0, 2)
		{
		}

		public override bool IsZeroFlagModified { get { return true; } }

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagCleared { get { return true; } }

		public override bool IsSignFlagModified { get { return true; } }

		public override bool IsOverflowFlagCleared { get { return true; } }

		public override bool IsOverflowFlagModified { get { return true; } }

		public override bool IsParityFlagModified { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			opcodeEncoder.Append4Bits(0b0110);
			opcodeEncoder.Append4Bits(0b0110);
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append4Bits(0b1111);
			opcodeEncoder.Append4Bits(0b0010);
			opcodeEncoder.Append4Bits(0b1111);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
		}
	}
}
