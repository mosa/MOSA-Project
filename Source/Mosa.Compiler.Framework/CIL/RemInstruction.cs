// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Rem Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.ArithmeticInstruction" />
	public sealed class RemInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public RemInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction
	}
}
