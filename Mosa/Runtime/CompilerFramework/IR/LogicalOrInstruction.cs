/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// 
    /// </summary>
    public class LogicalOrInstruction : Instruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalOrInstruction"/> class.
        /// </summary>
        public LogicalOrInstruction() :
            base(2, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalOrInstruction"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="op1">The op1.</param>
        /// <param name="op2">The op2.</param>
        public LogicalOrInstruction(Operand result, Operand op1, Operand op2) :
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
        /// <value>The destination.</value>
        public Operand Destination
        {
            get { return this.Results[0]; }
            set { this.SetResult(0, value); }
        }

        /// <summary>
        /// Gets or sets the source operand of the move instruction.
        /// </summary>
        /// <value>The operand1.</value>
        public Operand Operand1
        {
            get { return this.Operands[0]; }
            set { this.SetOperand(0, value); }
        }

        /// <summary>
        /// Gets or sets the source operand of the move instruction.
        /// </summary>
        /// <value>The operand2.</value>
        public Operand Operand2
        {
            get { return this.Operands[1]; }
            set { this.SetOperand(1, value); }
        }

        #endregion // Properties

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="MoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the move instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR or {0} <- {1} | {2}", this.Destination, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IIRVisitor<ArgType> irv = visitor as IIRVisitor<ArgType>;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIRVisitor!", @"visitor");

            irv.Visit(this, arg);
        }

        #endregion // Instruction Overrides
    }
}
