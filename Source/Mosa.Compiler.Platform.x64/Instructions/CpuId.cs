// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Instructions
{
	/// <summary>
	/// CpuId
	/// </summary>
	/// <seealso cref="X64Instruction" />
	public sealed class CpuId : X64Instruction
	{
		internal CpuId()
			: base(1, 2)
		{
		}

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0xA2);
		}
	}
}
