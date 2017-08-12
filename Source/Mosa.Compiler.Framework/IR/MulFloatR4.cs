// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the floating point multiplication operation.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	/// <remarks>
	/// The instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) multiplied with the second operand (index 1).
	/// <para />
	/// Both the first and second operand must be the same floating point type. If the second operand
	/// {D255958A-8513-4226-94B9-080D98F904A1}is statically or dynamically equal to or larger than the number of bits in the first
	/// {D255958A-8513-4226-94B9-080D98F904A1}operand, the result is undefined.
	/// </remarks>
	public sealed class MulFloatR4 : BaseThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MulFloatR4"/>.
		/// </summary>
		public MulFloatR4()
		{
		}
	}
}
