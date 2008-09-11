using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Abstract base class for IR instructions with three operands.
    /// </summary>
    /// <remarks>
    /// The <see cref="ThreeOperandInstruction"/> is the base class for
    /// IR instructions using three operands. It provides properties to
    /// easily access the individual operands.
    /// </remarks>
    public abstract class ThreeOperandInstruction : IRInstruction
    {
        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
        /// </summary>
        public ThreeOperandInstruction() :
            base(2, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeOperandInstruction"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="op1">The op1.</param>
        /// <param name="op2">The op2.</param>
        public ThreeOperandInstruction(Operand result, Operand op1, Operand op2) :
            base(2, 1)
        {
            SetResult(0, result);
            SetOperand(0, op1);
            SetOperand(1, op2);
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

        /// <summary>
        /// Gets or sets the source operand of the move instruction.
        /// </summary>
        public Operand Operand2
        {
            get { return this.Operands[1]; }
            set { this.SetOperand(1, value); }
        }

        #endregion // Properties
    }
}
