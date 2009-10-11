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
    /// Intermediate representation of the and instruction.
    /// </summary>
    public sealed class LogicalAndInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalAndInstruction"/> class.
        /// </summary>
        public LogicalAndInstruction()
        {
        }

        #endregion // Construction

        #region Instruction Overrides

		/// <summary>
		/// Returns a string representation of the <see cref="LogicalAndInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A string representation of the and context.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR.and {0} <- {1} & {2}", context.Operand1, context.Operand2, context.Operand3);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.LogicalAndInstruction(context);
        }

        #endregion // Instruction Overrides
    }
}
