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
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;


namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 pause instruction.
    /// </summary>
    public sealed class RdpmcInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RdpmcInstruction"/> class.
        /// </summary>
        public RdpmcInstruction()
        {
        }

        #endregion // Construction

        #region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rdpmc(context);
		}

        #endregion // Methods
    }
}
