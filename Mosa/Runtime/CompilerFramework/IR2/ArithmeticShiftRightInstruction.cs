/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR2
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
    /// them with zeroes, use <see cref="ShiftRightInstruction"/> instead.
    /// </remarks>
    public sealed class ArithmeticShiftRightInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ArithmeticShiftRightInstruction"/>.
        /// </summary>
        public ArithmeticShiftRightInstruction()
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="ArithmeticShiftRightInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the shr context.</returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR.ashr {0} <- {1} >> {2}", context.Operand1, context.Operand2, context.Operand3);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.ArithmeticShiftRightInstruction(context);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
