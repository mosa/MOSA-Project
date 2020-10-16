// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Store Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public class StoreInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StlocInstruction" /> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StoreInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction
	}
}
