// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Ret
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class Ret : X64Instruction
	{
		public override int ID { get { return 552; } }

		internal Ret()
			: base(0, 0)
		{
		}

		public static readonly byte[] opcode = new byte[] { 0xC3 };

		public override FlowControl FlowControl { get { return FlowControl.Return; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);

			emitter.Write(opcode);
		}
	}
}
