using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Abstract base class for IR instructions with two operands.
    /// </summary>
    /// <remarks>
    /// The <see cref="TwoOperandInstruction"/> is the base class for
    /// IR instructions using two operands. It provides properties to
    /// easily access the individual operands.
    /// </remarks>
    public abstract class TwoOperandInstruction : IRInstruction
    {
        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
        /// </summary>
        public TwoOperandInstruction() :
            base(1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwoOperandInstruction"/>.
        /// </summary>
        /// <param name="result">The operand, which receives the result of the instruction.</param>
        /// <param name="op1">The unary operand of this instruction.</param>
        public TwoOperandInstruction(Operand result, Operand op1) :
            base(1, 1)
        {
            SetResult(0, result);
            SetOperand(0, op1);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the destination operand of the move instruction.
        /// </summary>
        public Operand Operand0
        {
            get { return this.Results[0]; }
            set { this.SetResult(0, value); }
        }

        /// <summary>
        /// Gets or sets the source operand of the move instruction.
        /// </summary>
        public Operand Operand1
        {
            get { return this.Operands[0]; }
            set { this.SetOperand(0, value); }
        }

        #endregion // Properties
    }
}
