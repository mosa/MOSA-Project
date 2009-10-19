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
    /// Intermediate represenation of the x86 stosd instruction.
    /// </summary>
    public sealed class StosdInstruction : BaseInstruction
    {

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StosdInstruction"/> class.
        /// </summary>
        public StosdInstruction() :
            base()
        {
        }

        #endregion // Construction

        #region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="codeStream">The code stream.</param>
		public override void Emit(Context ctx, System.IO.Stream codeStream)
		{
			codeStream.WriteByte(0xAB);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Stosd(context);
		}

        #endregion // Methods
    }
}
