/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework.IR
{
    using System;

    /// <summary>
    /// Intermediate representation of the unsigned division operation.
    /// </summary>
    public sealed class DivUInstruction : ThreeOperandInstruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivUInstruction"/> class.
        /// </summary>
        public DivUInstruction()
        {
        }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.DivUInstruction(context);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR div.u{0} {1} = {2} / {3}", context.Result.Precision, context.Result, context.Operand1, context.Operand2);
        }
    }
}
