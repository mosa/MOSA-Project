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
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// x86 specific implementation of the prologue instruction.
    /// </summary>
    sealed class PrologueInstruction : IR.PrologueInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="PrologueInstruction"/>.
        /// </summary>
        /// <param name="stackSize">The stack size requirements of the method.</param>
        public PrologueInstruction(int stackSize) :
            base(stackSize)
        {
        }

        #endregion // Construction
    }
}
