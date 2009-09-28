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
    /// Intermediate representation of a push instruction, that moves 
    /// its argument on the top of a stack.
    /// </summary>
    public class PushInstruction : BaseInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="PushInstruction"/>.
        /// </summary>
        public PushInstruction() :
            base(1, 0)
        {
        }

        #endregion // Construction

        #region Instruction Overrides

		/// <summary>
		/// Returns a string representation of the <see cref="PopInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format("IR.push {0}", context.Operand1);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.PushInstruction(context);
        }

        #endregion // Instruction Overrides
    }
}
