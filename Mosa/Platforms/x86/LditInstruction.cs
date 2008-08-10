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
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Intrinsic instruction implementation for the x86 ldit instruction.
    /// </summary>
    public sealed class LditInstruction : Instruction
    {
        #region Construction

        public LditInstruction()
        {
        }

        #endregion // Construction

        #region ILInstruction Overrides

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86visitor = visitor as IX86InstructionVisitor;
            Debug.Assert(null != x86visitor);
            if (null != x86visitor)
                x86visitor.Ldit(this);
        }

        #endregion // ILInstruction Overrides
    }
}
