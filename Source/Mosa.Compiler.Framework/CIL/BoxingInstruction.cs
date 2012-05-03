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
	public class BoxingInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxingInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BoxingInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

	}
}
