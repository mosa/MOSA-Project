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
    /// Intermediate representation of the x86 pushfd instruction.
    /// </summary>
    public sealed class PushfdInstruction : BaseInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PushfdInstruction"/> class.
        /// </summary>
        public PushfdInstruction()
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
			visitor.Pushfd(context);
		}

        #endregion // Methods
    }
}