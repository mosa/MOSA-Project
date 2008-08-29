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
using System.Text;
using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using System.Diagnostics;

namespace Mosa.Platforms.x86
{
    sealed class SubInstruction : IL.SubInstruction
    {
        #region Construction

        public SubInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public SubInstruction(IL.OpCode code, Operand destination, Operand source) :
            base(code, destination, source)
        {
        }

        #endregion // Construction

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86 = visitor as IX86InstructionVisitor;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.Sub(this);
            else
                base.Visit(visitor);
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            if (First.StackType == StackTypeCode.F || Second.StackType == StackTypeCode.F)
                return methodCompiler.Architecture.CreateInstruction(typeof(x86.SseSubInstruction), IL.OpCode.Sub, new Operand[] { First, Second, Results[0] });
            return this;
        }
    }
}
