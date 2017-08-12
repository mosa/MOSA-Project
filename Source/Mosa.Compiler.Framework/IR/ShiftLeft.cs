// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the left shift operation.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	/// <remarks>
	/// The shift instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) shifted by the number of bits specified by
	/// </remarks>
	public sealed class ShiftLeft : BaseThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ShiftLeft"/>.
		/// </summary>
		public ShiftLeft()
		{
		}

		#endregion Construction
	}
}
