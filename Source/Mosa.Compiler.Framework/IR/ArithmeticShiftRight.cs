﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the shift right operation.
	/// </summary>
	/// <remarks>
	/// The shift instruction is a three-address instruction, where the result receives
	/// the value of the first operand (index 0) shifted by the number of bits specified by
	/// the second operand (index 1).
	/// <para />
	/// Both the first and second operand must be the same integral type. If the second operand
	/// is statically or dynamically equal to or larger than the number of bits in the first
	/// operand, the result is undefined.
	/// <para/>
	/// The most significant bits will be filled sign extended by this context. To fill
	/// them with zeroes, use <see cref="ShiftRight"/> instead.
	/// </remarks>
	public sealed class ArithmeticShiftRight : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticShiftRight"/>.
		/// </summary>
		public ArithmeticShiftRight()
		{
		}

		#endregion Construction

		#region ThreeOperandInstruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.ArithmeticShiftRight(context);
		}

		#endregion ThreeOperandInstruction Overrides
	}
}