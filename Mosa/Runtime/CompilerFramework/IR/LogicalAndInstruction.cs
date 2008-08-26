using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    public class LogicalAndInstruction : Instruction
    {
        #region Construction

        public LogicalAndInstruction() :
            base(2, 1)
        {
        }

        public LogicalAndInstruction(Operand result, Operand op1, Operand op2) :
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
        public Operand Destination
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

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="LogicalAndInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the and instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR and {0} <- {1} & {2}", this.Destination, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Implementation of the visitor pattern.
        /// </summary>
        /// <param name="visitor">The visitor requesting visitation. The object must implement <see cref="IIrVisitor"/>.</param>
        public override void Visit(IInstructionVisitor visitor)
        {
            IIrVisitor irv = visitor as IIrVisitor;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIrVisitor!", @"visitor");

            irv.Visit(this);
        }

        #endregion // Instruction Overrides
    }
}
