// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// Dsb - Data Synchronization Barrier
	/// </summary>
	/// <seealso cref="ARMv8A32Instruction" />
	public sealed class Dsb : ARMv8A32Instruction
	{
		internal Dsb()
			: base(1, 3)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 3);

			opcodeEncoder.Append32Bits(0x00000000);
		}
	}
}
