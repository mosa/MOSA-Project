// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Instructions
{
	/// <summary>
	/// XChg64
	/// </summary>
	/// <seealso cref="X64Instruction" />
	public sealed class XChg64 : X64Instruction
	{
		internal XChg64()
			: base(2, 2)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 2);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit((node.Operand2.Register.RegisterCode >> 3));
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3));
			opcodeEncoder.Append8Bits(0x87);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		}
	}
}
