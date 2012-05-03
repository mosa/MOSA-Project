/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Implements the internal representation for unary CIL instructions.
	/// </summary>
	public class UnaryInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnaryInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <param name="resultCount">The result count.</param>
		public UnaryInstruction(OpCode opcode, byte resultCount)
			: base(opcode, 1, resultCount)
		{
		}

		#endregion // Construction
	}
}
