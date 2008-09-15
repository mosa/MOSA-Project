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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    sealed class LogicalOrInstruction : IR.TwoOperandInstruction
    {
        #region Construction

        public LogicalOrInstruction()
        {
        }

        public LogicalOrInstruction(Operand result, Operand op1) :
            base(result, op1)
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        public override string ToString()
        {
            return String.Format(@"x86 or {0}, {1} ; {0} |= {1}", this.Operand0, this.Operand1);
        }

        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Or(this, arg);
            else
                base.Visit((IInstructionVisitor<ArgType>)visitor, arg);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
