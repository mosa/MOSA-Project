// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Mul Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.ArithmeticInstruction" />
	public sealed class MulInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MulInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public MulInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction
	}
}
