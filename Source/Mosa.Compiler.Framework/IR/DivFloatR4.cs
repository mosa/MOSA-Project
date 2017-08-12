// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the floating point division operation.
	/// </summary>
	/// <remarks>
	/// The instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) divided by the second operand (index 1).
	/// </remarks>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	public sealed class DivFloatR4 : BaseThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DivFloatR4"/>.
		/// </summary>
		public DivFloatR4()
		{
		}
	}
}
