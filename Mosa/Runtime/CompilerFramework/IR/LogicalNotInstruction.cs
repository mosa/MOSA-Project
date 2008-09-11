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
    public class LogicalNotInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LogicalNotInstruction"/>.
        /// </summary>
        public LogicalNotInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LogicalNotInstruction"/>.
        /// </summary>
        /// <param name="result">The operand, which receives the negated value of <paramref name="op1"/>.</param>
        /// <param name="op1">The operand, which represents the value to negate.</param>
        public LogicalNotInstruction(Operand result, Operand op1) :
            base(result, op1)
        {
        }

        #endregion // Construction

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="MoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the move instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR not {0} <- ~{1}", this.Operand0, this.Operand1);
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

        #endregion // Instruction Overrides
    }
}
