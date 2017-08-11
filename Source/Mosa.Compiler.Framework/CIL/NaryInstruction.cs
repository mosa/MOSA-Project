// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Nary Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public class NaryInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <param name="operandCount">The operand count.</param>
		public NaryInstruction(OpCode opcode, byte operandCount)
			: base(opcode, operandCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NaryInstruction"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		protected NaryInstruction(OpCode code, byte operandCount, byte resultCount)
			: base(code, operandCount, resultCount)
		{
		}

		#endregion Construction
	}
}
