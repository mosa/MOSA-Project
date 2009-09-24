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

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of the and instruction.
    /// </summary>
    public class LogicalAndInstruction : ThreeOperandInstruction
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
        /// <returns>A string representation of the and instruction.</returns>
        public override string ToString(ref InstructionData instruction)
        {
            return String.Format(@"IR.and {0} <- {1} & {2}", instruction.Operand1, instruction.Operand2, instruction.Operand3);
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
