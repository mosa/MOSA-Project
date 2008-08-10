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

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class BreakInstruction : ILInstruction
    {
        #region Construction

        public BreakInstruction(OpCode code) :
            base(code)
        {
            Debug.Assert(OpCode.Break == code, @"Wrong opcode for BreakInstruction.");
            if (OpCode.Break != code)
                throw new ArgumentException(@"Wrong opcode.", @"code");
        }

        #endregion // Construction

        #region ILInstruction Overrides

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Break(this);
        }

        #endregion // ILInstruction Overrides
    }
}
