// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of an SSE based subtraction instruction.
	/// </summary>
	public sealed class Subsd : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			return opcode;
		}

		#endregion Methods
	}
}
