/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Platforms.x86
{
    class SseDivInstruction : x86.MulInstruction
    {
        public SseDivInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public SseDivInstruction(IL.OpCode code, Operand[] ops) :
            base(code)
        {
            if (ops.Length != 3)
                throw new NotSupportedException(@"Wrong number of operands. Should be 3.");
            First = ops[0];
            Second = ops[1];
            Results[0] = ops[2];
        }

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86 = visitor as IX86InstructionVisitor;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.SseDiv(this);
            else
                base.Visit(visitor);
        }

        #region IRegisterConstraint Members

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            return this;
        }

        #endregion // IRegisterConstraint Members
    }
}
