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
using System.Diagnostics;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Stores a value to a memory pointer.
    /// </summary>
    /// <remarks>
    /// The store instruction stores the value in the given memory pointer.
    /// </remarks>
    public sealed class StoreInstruction : TwoOperandInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="StoreInstruction"/>.
		/// </summary>
        public StoreInstruction() 
        {
        }

        #endregion // Construction

        #region Methods

		/// <summary>
		/// Visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.StoreInstruction(context);
        }

        #endregion // Methods
    }
}
