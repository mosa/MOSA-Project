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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class RefanytypeInstruction : UnaryInstruction
    {
        #region Construction

        public RefanytypeInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Refanytype == code);
            if (OpCode.Refanytype != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // FIXME: Validate operands & verify instruction
            SetResult(0, CreateResultOperand(decoder.Architecture, new SigType(CilElementType.I4)));
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Refanytype(this);
        }

        #endregion // Methods
    }
}
