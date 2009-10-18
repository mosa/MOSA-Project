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

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 int instruction.
    /// </summary>
    public sealed class NegInstruction : OneOperandInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IntInstruction"/> class.
		/// </summary>
        public NegInstruction() :
            base()
        {
        }

        #endregion // Construction

        #region OneOperandInstruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Neg(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}
