// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Pop Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class PopInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PopInstruction" /> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public PopInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction
	}
}
