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

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// x86 specific intermediate representation of the <see cref="IR.EpilogueInstruction"/>.
    /// </summary>
    public sealed class EpilogueInstruction : IR.EpilogueInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="EpilogueInstruction"/>.
        /// </summary>
        /// <param name="stackSize">The stacksize requirements of the method.</param>
        public EpilogueInstruction(int stackSize) :
            base(stackSize)
        {
        }

        #endregion // Construction
    }
}
