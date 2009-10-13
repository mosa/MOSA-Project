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
	/// 
	/// </summary>
    public sealed class NopInstruction : BaseInstruction
    {
        #region Construction

        /// <summary>
		/// Initializes a new instance of the <see cref="NopInstruction"/>.
        /// </summary>
		public NopInstruction() :
            base(0, 0)
        {
        }

        #endregion // Construction

        #region Instruction

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.NopInstruction(context);
		}

        #endregion // Instruction
    }
}
