// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the unsigned add operation.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	/// <remarks>
	/// The add instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) added with the second operand (index 1).
	/// </remarks>
	public sealed class AddUnsigned : BaseThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddUnsigned"/>.
		/// </summary>
		public AddUnsigned()
		{
		}
	}
}
