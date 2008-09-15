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

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    sealed class MoveInstruction : IR.MoveInstruction
    {
        #region Construction

        public MoveInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand of the move instruction.</param>
        /// <param name="source">The source operand of the move instruction.</param>
        public MoveInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        #endregion // Construction
    }
}
