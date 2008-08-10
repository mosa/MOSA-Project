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

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class EndfilterInstruction : UnaryInstruction
    {
        #region Construction

        public EndfilterInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Endfilter == code);
            if (OpCode.Endfilter != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Validate the operand
            throw new NotImplementedException();
            //Debug.Assert(_operands[0].Type.StackType == StackTypeCode.Int32);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Endfilter(this);
        }

        #endregion // Methods
    }
}
