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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    sealed class SseSubInstruction : IR.TwoOperandInstruction
    {
        public SseSubInstruction()
        {
        }

        public SseSubInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        public override string  ToString()
        {
            return String.Format(@"x86 subsd {0}, {1} ; {0} -= {1}", this.Operand0, this.Operand1);
        }

        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.SseSub(this, arg);
            else
                base.Visit((IInstructionVisitor<ArgType>)visitor, arg);
        }
    }
}
