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
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class NopInstruction : ILInstruction
    {
        #region Construction

        public NopInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Nop == code, @"Wrong opcode for NopInstruction.");
            if (OpCode.Nop != code)
                throw new ArgumentException(@"Wrong opcode.", @"code");
        }

        #endregion // Construction

        #region ILInstruction Overrides

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Nop(this);
        }

        #endregion // ILInstruction Overrides
    }
}
