// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Instructions
{
	/// <summary>
	/// Cdq32
	/// </summary>
	/// <seealso cref="X64Instruction" />
	public sealed class Cdq32 : X64Instruction
	{
		internal Cdq32()
			: base(1, 1)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			opcodeEncoder.Append8Bits(0x99);
		}
	}
}
