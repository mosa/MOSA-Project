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
	/// Abstract base class for x86 instructions with two operands.
    /// </summary>
    /// <remarks>
    /// The <see cref="TwoOperandInstruction"/> is the base class for
    /// x86 instructions using two operands. It provides properties to
    /// easily access the individual operands.
    /// </remarks>
    public abstract class TwoOperandInstruction : BaseInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
        /// </summary>
        public TwoOperandInstruction() :
            base(1, 1)
        {
        }

        #endregion // Construction

    }
}
