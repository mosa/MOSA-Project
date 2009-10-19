/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 push instruction.
    /// </summary>
    public sealed class InvlpgInstruction : OneOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PushInstruction"/> class.
        /// </summary>
        public InvlpgInstruction() :
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
			visitor.Invlpg(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}
