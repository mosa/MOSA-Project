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

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
	/// Abstract base class for x86 instructions with one operand.
    /// </summary>
    /// <remarks>
    /// The <see cref="OneOperandInstruction"/> is the base class for
	/// x86 instructions using one operand.
    /// </remarks>
    public abstract class OneOperandInstruction : BaseInstruction
    {
  
        #region Construction

        /// <summary>
		/// Initializes a new instance of <see cref="OneOperandInstruction"/>.
        /// </summary>
        public OneOperandInstruction() :
            base(1, 0)
        {
        }

        #endregion // Construction

    }
}
