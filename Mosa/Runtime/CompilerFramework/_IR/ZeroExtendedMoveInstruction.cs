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
    /// Intermediate representation of a signed conversion instruction.
    /// </summary>
    /// <remarks>
    /// This instruction takes the source operand and converts to the request size maintaining its sign.
    /// </remarks>
    public class ZeroExtendedMoveInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroExtendedMoveInstruction"/>.
        /// </summary>
        public ZeroExtendedMoveInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroExtendedMoveInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand for the conversion.</param>
        /// <param name="source">The source operand for the conversion.</param>
        public ZeroExtendedMoveInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of <see cref="SignExtendedMoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR zconv {0} <- {1}", this.Operand0, this.Operand1);
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

        #endregion // TwoOperandInstruction Overrides
    }
}
