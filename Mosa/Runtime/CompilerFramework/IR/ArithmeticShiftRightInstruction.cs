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
    /// The most significant bits will be filled sign extended by this instruction. To fill
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

        /// <summary>
        /// Initializes a new instance of <see cref="ArithmeticShiftRightInstruction"/>.
        /// </summary>
        /// <param name="result">The result operand of the shift operation.</param>
        /// <param name="value">The value to shift.</param>
        /// <param name="shiftCount">The operand, which holds the number of bits to shift.</param>
        /// <remarks>
        /// Both <paramref name="value"/> and <paramref name="shiftCount"/> must be the same integral type. If op2 is statically
        /// or dynamically equal to or larger than the number of bits in value, the result is undefined. The most significant bits
        /// of <paramref name="result"/> will be filled with the sign. To fill with zeroes, use the
        /// <see cref="ShiftRightInstruction"/> instead.
        /// </remarks>
        public ArithmeticShiftRightInstruction(Operand result, Operand value, Operand shiftCount) :
            base(result, value, shiftCount)
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="ArithmeticShiftRightInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the shr instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR ashr {0} <- {1} & {2}", this.Operand0, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected override void Visit<ArgType>(IIrVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
