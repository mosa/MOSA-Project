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
	/// 
	/// </summary>
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

		#endregion // Construction

	}
}
