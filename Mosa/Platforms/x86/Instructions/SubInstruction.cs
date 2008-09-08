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

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.Sub(this, arg);
            else
                base.Visit(visitor, arg);
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            if (First.StackType == StackTypeCode.F || Second.StackType == StackTypeCode.F)
                return methodCompiler.Architecture.CreateInstruction(typeof(x86.SseSubInstruction), IL.OpCode.Sub, new Operand[] { First, Second, Results[0] });
            return this;
        }
    }
}
