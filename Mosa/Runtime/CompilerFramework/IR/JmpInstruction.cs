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
    /// Intermediate representation of an unconditional branch context.
    /// </summary>
    public sealed class JmpInstruction : BaseInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="JmpInstruction"/> class.
		/// </summary>
        public JmpInstruction()
        {
        }

        #endregion // Construction

        #region IRInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
			return String.Format(@"IR.jmp {0}", context.Branch.Targets[0]);
        }

		/// <summary>
		/// Visits the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.JmpInstruction(context);
        }

        #endregion // IRInstruction Overrides
    }
}
