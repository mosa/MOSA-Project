// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the floating point add operation.
	/// </summary>
	/// <remarks>
	/// The add instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) added with the second operand (index 1).
	/// </remarks>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	public sealed class AddFloatR4 : BaseThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddFloatR4" />.
		/// </summary>
		public AddFloatR4()
		{
		}
	}
}
