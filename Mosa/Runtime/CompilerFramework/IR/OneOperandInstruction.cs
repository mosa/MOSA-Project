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
    public abstract class OneOperandInstruction : IRInstruction
    {
        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
        /// </summary>
        public OneOperandInstruction() :
            base(1, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwoOperandInstruction"/>.
        /// </summary>
        /// <param name="op">The unary operand of this instruction.</param>
        public OneOperandInstruction(Operand op) :
            base(1, 0)
        {
            SetOperand(0, op);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the destination operand of the move instruction.
        /// </summary>
        public Operand Operand0
        {
            get { return this.Operands[0]; }
            set { this.SetOperand(0, value); }
        }

        #endregion // Properties
    }
}
