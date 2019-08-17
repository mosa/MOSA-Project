// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Pextrd32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class Pextrd32 : X86Instruction
	{
		public override int ID { get { return 281; } }

		internal Pextrd32()
			: base(1, 2)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			emitter.OpcodeEncoder.Append4Bits(0b0110);
			emitter.OpcodeEncoder.Append4Bits(0b0110);
			emitter.OpcodeEncoder.Append4Bits(0b0000);
			emitter.OpcodeEncoder.Append4Bits(0b1111);
			emitter.OpcodeEncoder.Append4Bits(0b0011);
			emitter.OpcodeEncoder.Append4Bits(0b1010);
			emitter.OpcodeEncoder.Append4Bits(0b0001);
			emitter.OpcodeEncoder.Append4Bits(0b0110);
			emitter.OpcodeEncoder.Append2Bits(0b11);
			emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
		}
	}
}
