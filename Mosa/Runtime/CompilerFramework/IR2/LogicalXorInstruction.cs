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
    /// Intermediate representation of the exclusive-or operation.
    /// </summary>
    public class LogicalXorInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalXorInstruction"/> class.
        /// </summary>
        public LogicalXorInstruction()
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="MoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the move instruction.</returns>
        public override string ToString(ref InstructionData instruction)
        {
			return String.Format(@"IR.or {0} <- {1} ^ {2}", instruction.Operand1, instruction.Operand2, instruction.Operand3);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.LogicalXorInstruction(context);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
