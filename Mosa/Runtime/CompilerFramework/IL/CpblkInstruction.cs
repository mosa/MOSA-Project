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
using Mosa.Runtime.CompilerFramework.IL;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class CpblkInstruction : NaryInstruction
    {
        #region Construction

        public CpblkInstruction(OpCode code)
            : base(code, 3)
        {
            Debug.Assert(OpCode.Cpblk == code);
            if (OpCode.Cpblk != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base
            base.Decode(decoder);

            // FIXME: Validate & verify
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Cpblk(this);
        }

        #endregion // Methods
    }
}
