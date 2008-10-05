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

namespace Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Implements the internal representation for unary CIL instructions.
	/// </summary>
    public abstract class UnaryInstruction : ILInstruction 
    {
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="UnaryInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the unary instruction.</param>
		protected UnaryInstruction(OpCode code)
			: base(code, 1)
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="UnaryInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        /// <param name="resultCount">Number of result operands</param>
        protected UnaryInstruction(OpCode code, int resultCount)
            : base(code, 1, resultCount)
        {
        }

        #endregion // Construction

		#region Methods

		#endregion // Methods
    }
}
