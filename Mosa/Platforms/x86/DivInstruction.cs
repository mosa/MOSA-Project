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
using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Platforms.x86
{
    class DivInstruction : IL.DivInstruction
    {
        public DivInstruction(IL.OpCode code) :
            base(code)
        {
        }

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86 = visitor as IX86InstructionVisitor;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.Div(this);
            else
                base.Visit(visitor);
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture architecture = methodCompiler.Architecture;
            if (First.StackType == StackTypeCode.F || Second.StackType == StackTypeCode.F)
                return architecture.CreateInstruction(typeof(x86.SseDivInstruction), IL.OpCode.Div, new Operand[] { First, Second, Results[0] });
            // FIXME
            // Waiting for ConstantPropagation to get shift/optimization to work.
            else if (Second is ConstantOperand)
            {
                int x = (int)(Second as ConstantOperand).Value;
                // Check if it's a power of two
                if ((x & (x - 1)) == 0)
                {
                    ConstantOperand shift = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.I4), (int)System.Math.Log(x, 2));
                    return architecture.CreateInstruction(typeof(x86.ShiftInstruction), IL.OpCode.Shr, new Operand[] { First, shift });
                }
                else
                    return this;
            }
            else
                return this;
        }
    }
}
