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
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86
{
    class ShiftInstruction : IL.ShiftInstruction, IRegisterConstraint
    {
        public ShiftInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86 = visitor as IX86InstructionVisitor;
            Debug.Assert(null != x86);
            if (null != x86)
            {
                x86.Shift(this);
            }
            else
                base.Visit(visitor);
        }

        #region IRegisterConstraint Members

        Register[] IRegisterConstraint.GetConstraints()
        {
            return null;
        }

        Register[] IRegisterConstraint.GetRegistersUsed()
        {
            return null;
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            return this;
        }

        #endregion // IRegisterConstraint Members
    }
}
