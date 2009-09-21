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
    /// Intermediate representation of an arbitrary move instruction.
    /// </summary>
    public class MoveInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        public MoveInstruction()
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="MoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the move instruction.</returns>
        public override string ToString(ref InstructionData instruction)
        {
            return String.Format(@"IR move {0} <- {1}", instruction.Operand1, instruction.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.MoveInstruction(context);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
