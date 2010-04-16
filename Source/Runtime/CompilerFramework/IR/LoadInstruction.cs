/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
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
    /// The load instruction is used to load a value from a memory pointer and an offset. The types must be compatible.
    /// </remarks>
    public sealed class LoadInstruction : ThreeOperandInstruction
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

