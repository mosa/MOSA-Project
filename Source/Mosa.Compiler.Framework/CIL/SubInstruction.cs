// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Sub Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.ArithmeticInstruction" />
	public sealed class SubInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SubInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public SubInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction
	}
}
