// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Stos
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class Stos : X64Instruction
	{
		public override int ID { get { return 586; } }

		internal Stos()
			: base(0, 1)
		{
		}

		public static readonly byte[] opcode = new byte[] { 0xAB };

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.Write(opcode);
		}
	}
}
