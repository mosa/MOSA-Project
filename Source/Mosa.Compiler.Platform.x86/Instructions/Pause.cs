// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Instructions
{
	/// <summary>
	/// Pause
	/// </summary>
	/// <seealso cref="X86Instruction" />
	public sealed class Pause : X86Instruction
	{
		internal Pause()
			: base(0, 0)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);

			opcodeEncoder.Append8Bits(0xF3);
			opcodeEncoder.Append8Bits(0x90);
		}
	}
}
