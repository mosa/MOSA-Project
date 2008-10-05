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
    public class SignExtendedMoveInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SignExtendedMoveInstruction"/>.
        /// </summary>
        public SignExtendedMoveInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignExtendedMoveInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand for the conversion.</param>
        /// <param name="source">The source operand for the conversion.</param>
        public SignExtendedMoveInstruction(Operand destination, Operand source) :
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
            return String.Format(@"IR sconv {0} <- {1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Implementation of the visitor pattern.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor requesting visitation. The object must implement see IIRVisitor.</param>
        /// <param name="arg">Generic context information to pass to the visitor.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
