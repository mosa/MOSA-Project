/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a signed conversion context.
    /// </summary>
    /// <remarks>
    /// This instruction takes the source operand and converts to the request size maintaining its sign.
    /// </remarks>
    public sealed class SignExtendedMoveInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SignExtendedMoveInstruction"/>.
        /// </summary>
        public SignExtendedMoveInstruction()
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of <see cref="SignExtendedMoveInstruction"/>.
        /// </summary>
        /// 
        public override string ToString(Context context)
        {
            return String.Format(@"IR.sconv {0} <- {1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Implementation of the visitor pattern.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.SignExtendedMoveInstruction(context);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
