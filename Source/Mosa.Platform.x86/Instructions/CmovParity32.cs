// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// CMovParity32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class CMovParity32 : X86Instruction
	{
		public override int ID { get { return 392; } }

		internal CMovParity32()
			: base(1, 1)
		{
		}

		public override string AlternativeName { get { return "CMovP32"; } }

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x4A });

		public override bool IsParityFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X86.CMovNoParity32;
		}

		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.Emit(LegacyOpcode, node.Result, node.Operand1);
		}
	}
}
