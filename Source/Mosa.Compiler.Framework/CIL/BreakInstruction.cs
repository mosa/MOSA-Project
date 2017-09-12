// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Break Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public sealed class BreakInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BreakInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BreakInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion Construction
	}
}
