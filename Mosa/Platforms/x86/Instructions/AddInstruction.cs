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

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using System.Diagnostics;

namespace Mosa.Platforms.x86
{
    sealed class AddInstruction : IL.AddInstruction
    {
        #region Construction

        public AddInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public AddInstruction(IL.OpCode code, Operand destination, Operand source) :
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
                x86.Add(this, arg);
            else
                base.Visit(visitor, arg);
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            if (First.StackType == StackTypeCode.F || Second.StackType == StackTypeCode.F)
                return methodCompiler.Architecture.CreateInstruction(typeof(x86.SseAddInstruction), IL.OpCode.Add, new Operand[] { First, Second, Results[0] });
            return this;
        }
    }
}
