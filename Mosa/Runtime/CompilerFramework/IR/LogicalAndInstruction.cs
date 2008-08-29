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
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IIrVisitor<ArgType> irv = visitor as IIrVisitor<ArgType>;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIrVisitor!", @"visitor");

            irv.Visit(this, arg);
        }

        #endregion // Instruction Overrides
    }
}
