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

namespace Mosa.Runtime.CompilerFramework.IR
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
    /// The most significant bits will be filled with zeroes by this instruction. To preserve
    /// the sign of the shifted value (inserting ones if it is negative), use the 
    /// <see cref="ArithmeticShiftRightInstruction"/> instead.
    /// </remarks>
    public sealed class ShiftRightInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftRightInstruction"/>.
        /// </summary>
        public ShiftRightInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ShiftRightInstruction"/>.
        /// </summary>
        /// <param name="result">The result operand of the shift operation.</param>
        /// <param name="value">The value to shift.</param>
        /// <param name="shiftCount">The operand, which holds the number of bits to shift.</param>
        /// <remarks>
        /// Both <paramref name="value"/> and <paramref name="shiftCount"/> must be the same integral type. If op2 is statically
        /// or dynamically equal to or larger than the number of bits in value, the result is undefined. The most significant bits
        /// of <paramref name="result"/> will be filled with zero. To preserve the sign after the shift operation, use the
        /// <see cref="ArithmeticShiftRightInstruction"/> instead.
        /// </remarks>
        public ShiftRightInstruction(Operand result, Operand value, Operand shiftCount) :
            base(result, value, shiftCount)
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="ShiftRightInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the shr instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR shr {0} <- {1} & {2}", this.Operand0, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
