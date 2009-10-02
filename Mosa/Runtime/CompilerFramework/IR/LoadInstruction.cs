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
using Mosa.Runtime.Metadata.Signatures;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Loads a value From a memory pointer.
    /// </summary>
    /// <remarks>
    /// The load instruction is used to load a value From
    /// a memory pointer. The types must be compatible.
    /// </remarks>
    public sealed class LoadInstruction : TwoOperandInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="LoadInstruction"/>.
		/// </summary>
        public LoadInstruction()
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR.load {0}, {1} ; {0} = *{1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.LoadInstruction(context);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
