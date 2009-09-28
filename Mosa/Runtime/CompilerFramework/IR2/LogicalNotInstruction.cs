/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of the not instruction.
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

        #endregion // Construction

        #region Instruction Overrides

		/// <summary>
		/// Returns a string representation of the <see cref="MoveInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
			return String.Format(@"IR.not {0} <- ~{1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.LogicalNotInstruction(context);
        }

        #endregion // Instruction Overrides
    }
}
