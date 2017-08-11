// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Add Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.ArithmeticInstruction" />
	public sealed class AddInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public AddInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction
	}
}
