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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    sealed class MoveInstruction : IR.MoveInstruction
    {
        #region Construction

        public MoveInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand of the move instruction.</param>
        /// <param name="source">The source operand of the move instruction.</param>
        public MoveInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        #endregion // Construction

        #region MoveInstruction Overrides

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            // We need to replace ourselves in case of a Memory -> Memory transfer
            Operand dst = this.Destination;
            Operand src = this.Source;
            if (dst is MemoryOperand && src is MemoryOperand)
            {
                RegisterOperand rop;
                if (dst.StackType == StackTypeCode.F)
                {
                    rop = new RegisterOperand(dst.Type, SSE2Register.XMM0);
                }
                else if (dst.StackType == StackTypeCode.Int64)
                {
                    rop = new RegisterOperand(dst.Type, SSE2Register.XMM0);
                }
                else
                {
                    rop = new RegisterOperand(dst.Type, GeneralPurposeRegister.EAX);
                }

                return new Instruction[] {
                    new MoveInstruction(rop, src),
                    new MoveInstruction(dst, rop)
                };
            }
            else
            {
                return base.Expand(methodCompiler);
            }
        }

        #endregion // MoveInstruction Overrides
    }
}
