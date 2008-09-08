/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Platforms.x86
{
    sealed class SseSubInstruction : IL.SubInstruction//, IRegisterConstraint
    {
        public SseSubInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public SseSubInstruction(IL.OpCode code, Operand[] ops) :
            base(code)
        {
            if (ops.Length != 3)
                throw new NotSupportedException(@"Wrong number of operands. Should be 3.");
            First = ops[0];
            Second = ops[1];
            Results[0] = ops[2];
        }

        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.SseSub(this, arg);
            else
                base.Visit(visitor, arg);
        }

        #region IRegisterConstraint Members

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            return this;
        }

        #endregion // IRegisterConstraint Members
    }
}
