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
    /// Intermediate representation of a pop operation, that removes the topmost element From the stack and
    /// places it in the destination operand.
    /// </summary>
    public class PopInstruction: IRInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PopInstruction"/>.
        /// </summary>
        public PopInstruction() :
            base(0, 1)
        {
        }

        #endregion // Construction

        #region Instruction

		/// <summary>
		/// Returns a string representation of the <see cref="PopInstruction"/>.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A string representation of the instruction.
		/// </returns>
        public override string ToString(ref InstructionData instruction)
        {
            return String.Format(@"IR pop {0}", instruction.Result);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.PopInstruction(context);
		}

        #endregion // Instruction
    }
}
